using Microsoft.JSInterop;
using frontend.Models;

namespace frontend.Services;

public class ThreeJsInterop : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public ThreeJsInterop(IJSRuntime js)
    {
        _js = js;
    }

    private async Task EnsureModuleLoadedAsync()
    {
        _module ??= await _js.InvokeAsync<IJSObjectReference>("import", "./js/three-scene.js");
    }

    public async Task InitializeSceneAsync(string containerId)
    {
        await EnsureModuleLoadedAsync();
        await _module!.InvokeVoidAsync("initScene", containerId);
    }

    public async Task BuildOverheadConveyorAsync(OverheadConveyorConfig config)
    {
        await EnsureModuleLoadedAsync();
        await _module!.InvokeVoidAsync("buildOverheadConveyor", config);
    }

    public async Task ClearSceneAsync()
    {
        await EnsureModuleLoadedAsync();
        await _module!.InvokeVoidAsync("clearScene");
    }

    public async Task ResetCameraAsync()
    {
        await EnsureModuleLoadedAsync();
        await _module!.InvokeVoidAsync("resetCamera");
    }

    public async Task SetViewAsync(string view)
    {
        await EnsureModuleLoadedAsync();
        await _module!.InvokeVoidAsync("setView", view);
    }

    public async Task ResizeAsync()
    {
        await EnsureModuleLoadedAsync();
        await _module!.InvokeVoidAsync("resize");
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("dispose");
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Ignore - browser already closed
            }
        }
    }
}
