using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConveyorApi.Services;

public class SketchRecognitionService
{
    private readonly string _apiKey;
    private static readonly HttpClient _http = new();

    private const string Prompt = """
        You are analyzing an engineering sketch of an overhead conveyor track layout.
        Extract all track geometry. Return ONLY valid JSON:
        {
          "segments": [
            {"type":"line","startX":<mm>,"startY":<mm>,"endX":<mm>,"endY":<mm>},
            {"type":"arc","centerX":<mm>,"centerY":<mm>,"radius":<mm>,"startAngleDeg":<deg>,"endAngleDeg":<deg>}
          ],
          "dimensions": [{"label":"<text>","valueMm":<number>}],
          "notes": ["<annotations>"]
        }
        Guidelines:
        - Use annotated dimensions if visible; otherwise estimate (track 5000-50000mm, curves 300-1500mm)
        - All measurements in millimeters, angles in degrees CCW from +X
        - Origin at bottom-left of layout
        - Return ONLY the JSON, no markdown
        """;

    public SketchRecognitionService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<CadImportResult> AnalyzeSketchAsync(Stream imageStream, string fileName)
    {
        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms);
        var base64 = Convert.ToBase64String(ms.ToArray());

        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var mediaType = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png"            => "image/png",
            ".bmp"            => "image/bmp",
            _                 => "image/jpeg"
        };

        var requestBody = new
        {
            model = "claude-opus-4-6",
            max_tokens = 4096,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "image", source = new { type = "base64", media_type = mediaType, data = base64 } },
                        new { type = "text", text = Prompt }
                    }
                }
            }
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
        req.Headers.Add("x-api-key", _apiKey);
        req.Headers.Add("anthropic-version", "2023-06-01");
        req.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        using var resp = await _http.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            var errBody = await resp.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Claude API error {(int)resp.StatusCode}: {errBody}");
        }

        var responseJson = await resp.Content.ReadAsStringAsync();
        var apiResponse  = JsonDocument.Parse(responseJson);
        var textContent  = apiResponse.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString() ?? "{}";

        // Strip any accidental markdown fences
        textContent = textContent.Trim();
        if (textContent.StartsWith("```")) textContent = textContent[(textContent.IndexOf('\n') + 1)..];
        if (textContent.EndsWith("```"))  textContent = textContent[..textContent.LastIndexOf("```")].TrimEnd();

        SketchResponse sketch;
        try
        {
            sketch = JsonSerializer.Deserialize<SketchResponse>(textContent, _jsonOptions)
                     ?? new SketchResponse([], [], []);
        }
        catch
        {
            sketch = new SketchResponse([], [], []);
        }

        return ConvertToImportResult(sketch, fileName);
    }

    // ──────────────────────────────────────────────────────────────────
    // Convert Claude's response → CadImportResult
    // ──────────────────────────────────────────────────────────────────

    private static CadImportResult ConvertToImportResult(SketchResponse sketch, string fileName)
    {
        var entities = new List<CadEntityInfo>();
        var texts    = new List<TextItem>();

        foreach (var seg in sketch.Segments ?? [])
        {
            if (seg.Type == "line" &&
                seg.StartX.HasValue && seg.StartY.HasValue &&
                seg.EndX.HasValue   && seg.EndY.HasValue)
            {
                var p1 = new Point3D(seg.StartX.Value, seg.StartY.Value, 0);
                var p2 = new Point3D(seg.EndX.Value,   seg.EndY.Value,   0);
                double dx = p2.X - p1.X, dy = p2.Y - p1.Y;
                entities.Add(new CadEntityInfo
                {
                    Type       = "Line",
                    Layer      = "0",
                    StartPoint = p1,
                    EndPoint   = p2,
                    Length     = Math.Sqrt(dx * dx + dy * dy)
                });
            }
            else if (seg.Type == "arc" &&
                     seg.CenterX.HasValue && seg.CenterY.HasValue &&
                     seg.Radius.HasValue  &&
                     seg.StartAngleDeg.HasValue && seg.EndAngleDeg.HasValue)
            {
                double angleDiff = Math.Abs(seg.EndAngleDeg.Value - seg.StartAngleDeg.Value);
                entities.Add(new CadEntityInfo
                {
                    Type       = "Arc",
                    Layer      = "0",
                    Center     = new Point3D(seg.CenterX.Value, seg.CenterY.Value, 0),
                    Radius     = seg.Radius.Value,
                    StartAngle = seg.StartAngleDeg.Value,
                    EndAngle   = seg.EndAngleDeg.Value,
                    Length     = seg.Radius.Value * angleDiff * Math.PI / 180.0
                });
            }
        }

        foreach (var dim in sketch.Dimensions ?? [])
        {
            texts.Add(new TextItem
            {
                Content  = $"{dim.Label}: {dim.ValueMm:F0}mm",
                Position = new Point3D(0, 0, 0),
                ItemType = "Dimension",
                Layer    = "0"
            });
        }

        foreach (var note in sketch.Notes ?? [])
        {
            if (!string.IsNullOrWhiteSpace(note))
                texts.Add(new TextItem
                {
                    Content  = note,
                    Position = new Point3D(0, 0, 0),
                    ItemType = "Text",
                    Layer    = "0"
                });
        }

        return CadImportService.BuildResult(entities, texts, $"{Path.GetFileNameWithoutExtension(fileName)} (AI interpreted){Path.GetExtension(fileName)}");
    }

    // ──────────────────────────────────────────────────────────────────
    // Internal DTOs for Claude's JSON response
    // ──────────────────────────────────────────────────────────────────

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private record SketchResponse(
        [property: JsonPropertyName("segments")]   List<SketchSegment>?   Segments,
        [property: JsonPropertyName("dimensions")] List<SketchDimension>? Dimensions,
        [property: JsonPropertyName("notes")]      List<string>?          Notes);

    private record SketchSegment(
        [property: JsonPropertyName("type")]          string  Type,
        [property: JsonPropertyName("startX")]        double? StartX,
        [property: JsonPropertyName("startY")]        double? StartY,
        [property: JsonPropertyName("endX")]          double? EndX,
        [property: JsonPropertyName("endY")]          double? EndY,
        [property: JsonPropertyName("centerX")]       double? CenterX,
        [property: JsonPropertyName("centerY")]       double? CenterY,
        [property: JsonPropertyName("radius")]        double? Radius,
        [property: JsonPropertyName("startAngleDeg")] double? StartAngleDeg,
        [property: JsonPropertyName("endAngleDeg")]   double? EndAngleDeg);

    private record SketchDimension(
        [property: JsonPropertyName("label")]   string Label,
        [property: JsonPropertyName("valueMm")] double ValueMm);
}
