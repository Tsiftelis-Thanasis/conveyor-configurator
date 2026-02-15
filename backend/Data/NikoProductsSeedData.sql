-- NIKO Conveyor Systems Product Seed Data
-- Source: NIKO Product Catalogue October 2022
-- Manufacturer: Helm Hellas S.A. (Greece)
-- Note: Prices are placeholder values - actual pricing requires separate quote from manufacturer

-- =============================================
-- Categories
-- =============================================
INSERT INTO Categories (Code, Name, Description, PageReference) VALUES
('PROFILES', 'Track Profiles', 'Enclosed track tapered design profiles for loads up to 2,000 kg', '12'),
('BENDS', 'Track Bends', 'Horizontal and vertical track bends at various angles', '13'),
('BRACKETS', 'Brackets & Support Joints', 'Mounting brackets, splice joints, and support systems', '14-23'),
('TROLLEYS', 'Trolleys', '2-wheel and 4-wheel trolleys with various configurations', '24-39'),
('FLIGHTBARS', 'Flight Bars', 'Load carrying bars with eye nuts and hooks', '33-39'),
('SWITCHES', 'Switches', 'Tongue switches, swivel switches, and turntable switches', '40-48'),
('STOPPERS', 'Stoppers', 'Directional stoppers and spring loaded stoppers', '49-50'),
('SWIVEL', 'Swivel Units', 'Manual and pneumatic swivel units', '50-51'),
('INTERLOCK', 'Bridge Interlocks', 'Bridge interlock and track transfer assemblies', '52-53'),
('DROPLIFT', 'Drop-Lift Units', 'Standard and low headroom drop-lift stations', '54-55'),
('ACCESSORIES', 'Accessories', 'End stops, track covers, dilation joints', '50'),
('BEARINGS', 'Bearing Options', 'Special bearing options for various temperature ranges', '23'),
('PNEUMATIC', 'Pneumatic Controls', 'Pneumatic controls and air distributors', '42'),
('STAINLESS', 'Stainless Steel Series', 'Components in stainless steel 304 and 316', '56-62');

-- =============================================
-- Materials
-- =============================================
INSERT INTO Materials (Code, Name, Description) VALUES
('STL-GOLD', 'Steel Gold Finish', 'Standard steel with gold zinc-plated finish'),
('STL-SILVER', 'Steel Silver Finish', 'Standard steel with silver zinc-plated finish'),
('STL-BLACK', 'Steel Black Finish', 'Steel with black finish'),
('STL-PLAIN', 'Steel Plain/Unplated', 'Plain unplated steel'),
('SS304', 'Stainless Steel 304 (A2)', 'AISI 304 stainless steel, also known as A2'),
('SS316', 'Stainless Steel 316 (A4)', 'AISI 316 stainless steel, also known as A4');

-- =============================================
-- Profile Series
-- =============================================
INSERT INTO ProfileSeries (SeriesCode, HeightMm, WidthMm, SlotWidthMm, WallThicknessMm, MaxLoadKg, MaterialId) VALUES
('21.000', 28.0, 30.0, 8.0, 1.75, 20, (SELECT Id FROM Materials WHERE Code = 'STL-GOLD')),
('23.000', 35.0, 40.0, 11.0, 2.75, 40, (SELECT Id FROM Materials WHERE Code = 'STL-GOLD')),
('24.000', 43.5, 48.5, 15.0, 3.20, 80, (SELECT Id FROM Materials WHERE Code = 'STL-GOLD')),
('25.000', 60.0, 65.0, 18.0, 3.60, 200, (SELECT Id FROM Materials WHERE Code = 'STL-GOLD')),
('26.000', 75.0, 80.0, 22.0, 4.50, 400, (SELECT Id FROM Materials WHERE Code = 'STL-GOLD')),
('27.000', 110.0, 90.0, 25.0, 6.50, 800, (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'));

-- Stainless Steel Profile Series (304)
INSERT INTO ProfileSeries (SeriesCode, HeightMm, WidthMm, SlotWidthMm, WallThicknessMm, MaxLoadKg, MaterialId) VALUES
('21.050', 28.0, 30.0, 8.0, 1.75, 20, (SELECT Id FROM Materials WHERE Code = 'SS304')),
('23.050', 35.0, 40.0, 11.0, 3.00, 40, (SELECT Id FROM Materials WHERE Code = 'SS304')),
('24.050', 43.5, 48.5, 15.0, 3.20, 80, (SELECT Id FROM Materials WHERE Code = 'SS304')),
('25.050', 60.0, 65.0, 18.0, 4.00, 200, (SELECT Id FROM Materials WHERE Code = 'SS304'));

-- Stainless Steel Profile Series (316)
INSERT INTO ProfileSeries (SeriesCode, HeightMm, WidthMm, SlotWidthMm, WallThicknessMm, MaxLoadKg, MaterialId) VALUES
('21.070', 28.0, 30.0, 8.0, 1.75, 20, (SELECT Id FROM Materials WHERE Code = 'SS316')),
('23.070', 35.0, 40.0, 11.0, 3.00, 40, (SELECT Id FROM Materials WHERE Code = 'SS316')),
('24.070', 43.5, 48.5, 15.0, 3.20, 80, (SELECT Id FROM Materials WHERE Code = 'SS316')),
('25.070', 60.0, 65.0, 18.0, 4.00, 200, (SELECT Id FROM Materials WHERE Code = 'SS316'));

-- =============================================
-- Track Profiles (Linear Track Sections)
-- =============================================
-- Standard Steel Profiles (sold per meter, standard lengths 2m, 3m, 4m, 5m, 6m)
INSERT INTO TrackProfiles (PartNumber, SeriesId, MaterialId, LengthMm, PricePerMeter) VALUES
('21.001', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), NULL, NULL),
('23.001', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), NULL, NULL),
('24.001', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), NULL, NULL),
('25.001', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), NULL, NULL),
('26.001', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), NULL, NULL),
('27.001', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), NULL, NULL);

-- Stainless Steel 304 (A2) Profiles
INSERT INTO TrackProfiles (PartNumber, SeriesId, MaterialId, LengthMm, PricePerMeter) VALUES
('21.051', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.050'), (SELECT Id FROM Materials WHERE Code = 'SS304'), NULL, NULL),
('23.051', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.050'), (SELECT Id FROM Materials WHERE Code = 'SS304'), NULL, NULL),
('24.051', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.050'), (SELECT Id FROM Materials WHERE Code = 'SS304'), NULL, NULL),
('25.051', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.050'), (SELECT Id FROM Materials WHERE Code = 'SS304'), NULL, NULL);

-- Stainless Steel 316 (A4) Profiles
INSERT INTO TrackProfiles (PartNumber, SeriesId, MaterialId, LengthMm, PricePerMeter) VALUES
('21.071', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.070'), (SELECT Id FROM Materials WHERE Code = 'SS316'), NULL, NULL),
('23.071', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.070'), (SELECT Id FROM Materials WHERE Code = 'SS316'), NULL, NULL),
('24.071', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.070'), (SELECT Id FROM Materials WHERE Code = 'SS316'), NULL, NULL),
('25.071', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.070'), (SELECT Id FROM Materials WHERE Code = 'SS316'), NULL, NULL);

-- =============================================
-- Track Bends - 90 Degree (Standard Radius)
-- =============================================
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('21.C06', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 650, 490, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C07', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 690, 460, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C06', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 580, 550, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C06', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 580, 550, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C08', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 770, 900, 3000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('27.C10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 1035, 690, 3000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- Track Bends - 90 Degree (Small Radius)
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('21.C02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 205, 590, 1500, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 300, 510, 1500, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 400, 690, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 400, 690, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 440, 650, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C09', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 90, 905, 540, 2500, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- Track Bends - 60 Degree
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('21.C12', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 205, 640, 1500, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 400, 790, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 400, 790, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 440, 770, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 650, 660, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C17', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 690, 640, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 580, 700, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 905, 530, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 580, 700, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 770, 600, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('27.C20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 60, 1035, 960, 3000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- Track Bends - 45 Degree
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('21.C22', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 205, 670, 1500, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 400, 840, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 400, 840, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 440, 830, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 650, 750, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C27', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 690, 730, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 580, 770, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C29', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 905, 640, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 580, 770, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C28', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 770, 700, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('27.C30', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 45, 1035, 1090, 3000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- Track Bends - 30 Degree
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('21.C32', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 205, 700, 1500, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C34', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 400, 900, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C34', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 400, 900, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C34', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 440, 890, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C36', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 650, 830, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C37', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 690, 820, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C36', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 580, 850, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C39', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 905, 760, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C36', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 580, 850, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C38', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 770, 800, 2000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('27.C40', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 30, 1035, 1230, 3000, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- Track Bends - 180 Degree
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('21.C64', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 400, 250, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('21.C66', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 650, 200, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C64', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 400, 300, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C67', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 690, 300, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C64', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 440, NULL, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C66', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 580, NULL, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C66', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 580, 300, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C68', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 180, 770, 130, NULL, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- Vertical Bends - 20 Degree Incline/Decline
INSERT INTO TrackBends (PartNumber, SeriesId, MaterialId, AngleDegrees, RadiusMm, DimensionA_Mm, TotalLengthMm, CategoryId, Price) VALUES
('23.C50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 610, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('23.C51', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 610, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 665, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('24.C51', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 665, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 670, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('25.C51', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 670, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 700, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL),
('26.C51', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 20, 600, 220, 700, (SELECT Id FROM Categories WHERE Code = 'BENDS'), NULL);

-- =============================================
-- Brackets - Splice Joint .B49
-- =============================================
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, CategoryId, Price) VALUES
('21.B49', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Splice Joint', 85, 36, 38, 3, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B49', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Splice Joint', 120, 44, 50, 4, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B49', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Splice Joint', 150, 54, 61, 4.5, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B49', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Splice Joint', 180, 75, 81, 6, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B49', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Splice Joint', 200, 94, 100, 8, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B49', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Splice Joint', 250, 133, 116, 10, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- Brackets - Support Bracket .B00 (Zinc Plated)
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, CategoryId, Price) VALUES
('21.B00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Support Bracket', 40, 36, 38, 3, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Support Bracket', 56, 44, 50, 4, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Support Bracket', 68, 54, 61, 4.5, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Support Bracket', 90, 75, 81, 6, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Support Bracket', 110, 94, 100, 8, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Support Bracket', 120, 133, 116, 10, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- Brackets - Support Bracket .B50 (Black)
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, CategoryId, Price) VALUES
('21.B50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-BLACK'), 'Support Bracket Black', 40, 36, 38, 3, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-BLACK'), 'Support Bracket Black', 56, 44, 50, 4, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-BLACK'), 'Support Bracket Black', 68, 54, 61, 4.5, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-BLACK'), 'Support Bracket Black', 90, 75, 81, 6, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-BLACK'), 'Support Bracket Black', 110, 94, 100, 8, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B50', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-BLACK'), 'Support Bracket Black', 120, 133, 116, 10, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- Brackets - Wall Support Bracket .B01
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, HoleDiameterMm, CategoryId, Price) VALUES
('21.B01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Wall Support Bracket', 40, 60, 38, 3, 8, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Wall Support Bracket', 56, 79, 50, 4, 11, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Wall Support Bracket', 68, 95, 61, 4.5, 13, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Wall Support Bracket', 90, 123, 81, 6, 17, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Wall Support Bracket', 110, 156, 100, 8, 22, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Wall Support Bracket', 120, 205, 116, 10, 27, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- Brackets - Ceiling Support Bracket .B02
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, HoleDiameterMm, CategoryId, Price) VALUES
('21.B02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Ceiling Support Bracket', 64, 39, 90, 3, 8, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Ceiling Support Bracket', 81, 50, 115, 4, 11, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Ceiling Support Bracket', 94, 60, 130, 4.5, 13, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Ceiling Support Bracket', 124, 81, 171, 6, 17, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Ceiling Support Bracket', 148, 104, 210, 8, 22, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Ceiling Support Bracket', 178.5, 145, 260, 10, 22, (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- Brackets - Split Support Bracket .B03
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, ThreadSize, CategoryId, Price) VALUES
('21.B03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Split Support Bracket', 40, 65, 36, 3, 'M10', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Split Support Bracket', 50, 78, 48, 4, 'M12', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Split Support Bracket', 50, 88, 57, 4, 'M12', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Split Support Bracket', 90, 131, 77, 6, 'M16', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Split Support Bracket', 110, 150, 96, 8, 'M16', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B03', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Split Support Bracket', 120, 180, 110, 10, 'M16', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- Brackets - Adjustable Bracket .B04
INSERT INTO Brackets (PartNumber, SeriesId, MaterialId, BracketType, LengthMm, HeightMm, WidthMm, WallThicknessMm, MaxAdjustmentMm, ThreadSize, CategoryId, Price) VALUES
('21.B04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Adjustable Bracket', 40, 100, 38, 3, 32, 'M10', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('23.B04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Adjustable Bracket', 56, 135, 50, 4, 50, 'M16', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('24.B04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Adjustable Bracket', 68, 146, 61, 4.5, 55, 'M16', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('25.B04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Adjustable Bracket', 90, 215, 81, 6, 93, 'M20', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('26.B04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Adjustable Bracket', 110, 295, 100, 8, 140, 'M20', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL),
('27.B04', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Adjustable Bracket', 120, 348, 116, 10, 140, 'M30', (SELECT Id FROM Categories WHERE Code = 'BRACKETS'), NULL);

-- =============================================
-- Trolleys - 4-Wheel Trolley for Welding .T00
-- =============================================
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, CategoryId, Price) VALUES
('21.T00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Welding', 4, 20, 60, 24, 30, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('23.T00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Welding', 4, 40, 80, 25, 40, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Welding', 4, 80, 100, 34, 48.5, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Welding', 4, 200, 120, 34, 65, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Welding', 4, 400, 145, 42.5, 80, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T00', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Welding', 4, 800, 210, 49, 90, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 2-Wheel Trolley for Welding .T01
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, CategoryId, Price) VALUES
('21.T01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel Welding', 2, 10, 25, 24, 30, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('23.T01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel Welding', 2, 20, 30, 25, 40, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel Welding', 2, 40, 40, 34, 48.5, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel Welding', 2, 100, 50, 34, 65, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel Welding', 2, 200, 70, 42.5, 80, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel Welding', 2, 400, 100, 49, 90, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel Trolley with Hole .T10
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, HoleDiameterMm, CategoryId, Price) VALUES
('21.T10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel with Hole', 4, 20, 60, 20.5, 30, 8, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('23.T10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel with Hole', 4, 40, 80, 32, 40, 10, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel with Hole', 4, 80, 100, 40, 48.5, 14, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel with Hole', 4, 200, 120, 42, 65, 18, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel with Hole', 4, 400, 145, 47, 80, 22, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel with Hole', 4, 800, 210, 74, 90, 26, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 2-Wheel Trolley with Hole .T11
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, HoleDiameterMm, CategoryId, Price) VALUES
('21.T11', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel with Hole', 2, 10, 25, 20, 30, 6, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('23.T11', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel with Hole', 2, 20, 30, 32, 40, 10, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T11', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel with Hole', 2, 40, 40, 39, 48.5, 14, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T11', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel with Hole', 2, 100, 50, 42, 65, 18, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T11', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel with Hole', 2, 200, 70, 47, 80, 22, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T11', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '2-Wheel with Hole', 2, 400, 100, 73, 90, 26, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel with Rotating Screw .T14
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, ThreadSize, HasRotatingScrew, CategoryId, Price) VALUES
('23.T14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw', 4, 40, 70, 68, 40, 'M12', 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw', 4, 80, 90, 79, 48.5, 'M16', 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw', 4, 200, 110, 107, 65, 'M20', 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw', 4, 400, 140, 134, 80, 'M24', 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T14', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw', 4, 800, 200, 153, 90, 'M30', 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel with Rotating Screw and Guide Rollers .T42
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, ThreadSize, HasRotatingScrew, HasGuideRollers, CategoryId, Price) VALUES
('24.T42', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw Guide Rollers', 4, 80, 90, 113, 48.5, 'M16', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T42', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw Guide Rollers', 4, 200, 100, 148, 65, 'M20', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T42', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw Guide Rollers', 4, 400, 140, 181, 80, 'M24', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T42', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Screw Guide Rollers', 4, 800, 180, 226, 90, 'M30', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel with Rotating Eye Nut .T20
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, HasRotatingScrew, CategoryId, Price) VALUES
('23.T20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Eye Nut DIN582', 4, 40, 70, 85, 40, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Eye Nut DIN582', 4, 80, 90, 96.5, 48.5, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Eye Nut DIN582', 4, 200, 110, 122, 65, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Eye Nut DIN582', 4, 400, 140, 145, 80, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Eye Nut DIN582', 4, 800, 200, 182, 90, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel with Rotating Ring .T70
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, HeightMm, WidthMm, CategoryId, Price) VALUES
('23.T70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Ring', 4, 40, 79, 40, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('24.T70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Ring', 4, 80, 79, 48.5, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Ring', 4, 200, 79, 65, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Ring', 4, 400, 98, 80, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Ring', 4, 800, 126, 90, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel with Rotating Hook .T71
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, HeightMm, WidthMm, CategoryId, Price) VALUES
('24.T71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Hook', 4, 80, 125, 48.5, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Hook', 4, 200, 125, 65, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Rotating Hook', 4, 400, 152, 80, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- Trolleys - 4-Wheel for Power Chain .T47
INSERT INTO Trolleys (PartNumber, SeriesId, MaterialId, TrolleyType, WheelCount, SafeWorkingLoadKg, LengthMm, HeightMm, WidthMm, ThreadSize, HasRotatingScrew, HasGuideRollers, CategoryId, Price) VALUES
('24.T47', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Power Chain Dog Pin', 4, 80, 90, 148, 48.5, 'M16', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('25.T47', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Power Chain Dog Pin', 4, 200, 100, 182, 65, 'M20', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('26.T47', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Power Chain Dog Pin', 4, 400, 126, 245, 80, 'M24', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL),
('27.T47', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), '4-Wheel Power Chain Dog Pin', 4, 800, 120, 265, 90, 'M30', 1, 1, (SELECT Id FROM Categories WHERE Code = 'TROLLEYS'), NULL);

-- =============================================
-- Flight Bars
-- =============================================
INSERT INTO FlightBars (PartNumber, SeriesId, MaterialId, FlightBarType, SafeWorkingLoadKg, LengthMm, SpanMm, HeightMm, EyeNutDiameterMm, HasRotatingEyeNut, CategoryId, Price) VALUES
('23.T24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Eye Nut', 80, 300, 180, 128, 30, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('24.T24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Eye Nut', 160, 450, 300, 152, 35, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('25.T24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Eye Nut', 400, 600, 420, 190, 40, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('26.T24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Eye Nut', 800, 700, 500, 250, 50, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('27.T24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Eye Nut', 1600, 1000, 700, 310, 60, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL);

INSERT INTO FlightBars (PartNumber, SeriesId, MaterialId, FlightBarType, SafeWorkingLoadKg, LengthMm, SpanMm, HeightMm, EyeNutDiameterMm, HasRotatingEyeNut, CategoryId, Price) VALUES
('23.T26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Eye Nut', 80, 300, 180, 133, 30, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('24.T26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Eye Nut', 160, 450, 300, 160, 35, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('25.T26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Eye Nut', 400, 600, 420, 200, 40, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('26.T26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Eye Nut', 800, 700, 500, 260, 50, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('27.T26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Eye Nut', 1600, 1000, 700, 320, 60, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL);

INSERT INTO FlightBars (PartNumber, SeriesId, MaterialId, FlightBarType, SafeWorkingLoadKg, LengthMm, SpanMm, HeightMm, HasRotatingEyeNut, CategoryId, Price) VALUES
('23.T28', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Hook', 80, 300, 180, 200, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('24.T28', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Hook', 160, 450, 300, 230, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('25.T28', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Hook', 400, 600, 420, 280, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('26.T28', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Hook', 800, 700, 500, 330, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('27.T28', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Fixed Hook', 1600, 1000, 700, 410, 0, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL);

INSERT INTO FlightBars (PartNumber, SeriesId, MaterialId, FlightBarType, SafeWorkingLoadKg, LengthMm, SpanMm, HeightMm, HasRotatingEyeNut, CategoryId, Price) VALUES
('23.T38', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Hook', 80, 300, 180, 205, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('24.T38', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Hook', 160, 450, 300, 235, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('25.T38', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Hook', 400, 600, 420, 290, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('26.T38', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Hook', 800, 700, 500, 330, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL),
('27.T38', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Rotating Hook', 1600, 1000, 700, 415, 1, (SELECT Id FROM Categories WHERE Code = 'FLIGHTBARS'), NULL);

-- =============================================
-- Switches - Single Tongue Switch with 90° Bend .A45/.A46
-- =============================================
INSERT INTO Switches (PartNumber, SeriesId, MaterialId, SwitchType, RadiusMm, WidthMm, LengthMm, AngleDegrees, IsAutomatic, CategoryId, Price) VALUES
('23.A45', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Right', 690, 700, 800, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('23.A46', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Left', 690, 700, 800, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('24.A45', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Right', 580, 700, 800, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('24.A46', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Left', 580, 700, 800, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('25.A45', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Right', 580, 800, 800, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('25.A46', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Left', 580, 800, 800, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('26.A45', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Right', 770, 1100, 1000, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('26.A46', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Left', 770, 1100, 1000, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('27.A45', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Right', 1035, 1350, 1300, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('27.A46', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Single Tongue Left', 1035, 1350, 1300, 90, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

-- Switches - Triple Tongue Switch with Pneumatic Turntable .A17
INSERT INTO Switches (PartNumber, SeriesId, MaterialId, SwitchType, RadiusMm, WidthMm, LengthMm, IsPneumatic, IncludesCylinder, CategoryId, Price) VALUES
('24.A17', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Tongue Pneumatic Turntable', 580, 700, 950, 1, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('24.A18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Tongue Pneumatic Turntable', 440, 590, 840, 1, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('25.A17', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Tongue Pneumatic Turntable', 580, 760, 1110, 1, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('26.A17', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Tongue Pneumatic Turntable', 770, 1060, 1460, 1, 0, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('27.A17', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Tongue Pneumatic Turntable', 1035, 1285, 1785, 1, 1, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

-- Switches - Triple Swivel Switch .A35
INSERT INTO Switches (PartNumber, SeriesId, MaterialId, SwitchType, LengthMm, WidthMm, CategoryId, Price) VALUES
('21.A35', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Swivel', 830, 40, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('23.A35', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Swivel', 1180, 55, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('24.A35', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Swivel', 1050, 63.5, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('25.A35', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Swivel', 1380, 85, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('26.A35', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Swivel', 1480, 100, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('27.A35', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Triple Swivel', 1750, 115, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

-- =============================================
-- Turn Table Switches
-- =============================================
INSERT INTO TurnTableSwitches (PartNumber, SeriesId, MaterialId, SwitchType, DimensionA_Mm, DimensionB_Mm, ProfileHeightMm, OperationType, SupportPoints, CategoryId, Price) VALUES
('23.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform', 450, 700, 35, 'Manual', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('24.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform', 450, 850, 43.5, 'Manual', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('25.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform', 600, 1100, 60, 'Manual', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('26.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform', 700, 1300, 75, 'Manual', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('27.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform', 1000, 1800, 110, 'Manual', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

INSERT INTO TurnTableSwitches (PartNumber, SeriesId, MaterialId, SwitchType, DimensionA_Mm, DimensionB_Mm, OperationType, SupportPoints, CategoryId, Price) VALUES
('EL.23.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Electric', 450, 700, 'Electric', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('EL.24.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Electric', 450, 850, 'Electric', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('EL.25.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Electric', 600, 1100, 'Electric', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('EL.26.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Electric', 700, 1300, 'Electric', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('EL.27.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Electric', 1000, 1800, 'Electric', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

INSERT INTO TurnTableSwitches (PartNumber, SeriesId, MaterialId, SwitchType, DimensionA_Mm, DimensionB_Mm, OperationType, SupportPoints, CategoryId, Price) VALUES
('PN.23.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Pneumatic', 450, 700, 'Pneumatic', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('PN.24.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Pneumatic', 450, 850, 'Pneumatic', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('PN.25.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Pneumatic', 600, 1100, 'Pneumatic', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('PN.26.A41', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Cruciform Pneumatic', 700, 1340, 'Pneumatic', 8, (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

-- Pneumatic Turn Table Switch .A44
INSERT INTO TurnTableSwitches (PartNumber, SeriesId, MaterialId, SwitchType, DimensionA_Mm, DimensionB_Mm, OperationType, CategoryId, Price) VALUES
('23.A44', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic 90 Degree', 150, 500, 'Pneumatic', (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('24.A44', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic 90 Degree', 150, 500, 'Pneumatic', (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('25.A44', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic 90 Degree', 200, 700, 'Pneumatic', (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('26.A44', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic 90 Degree', 250, 800, 'Pneumatic', (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL),
('27.A44', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic 90 Degree', 350, 1000, 'Pneumatic', (SELECT Id FROM Categories WHERE Code = 'SWITCHES'), NULL);

-- =============================================
-- Stoppers
-- =============================================
INSERT INTO Stoppers (PartNumber, SeriesId, MaterialId, StopperType, LengthMm, HeightMm, IsSpringLoaded, CategoryId, Price) VALUES
('23.H10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Spring Loaded', 50, 78, 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('24.H10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Spring Loaded', 60, 80, 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('25.H10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Spring Loaded', 90, 120, 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('26.H10', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Spring Loaded', 110, 145, 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL);

INSERT INTO Stoppers (PartNumber, SeriesId, MaterialId, StopperType, LengthMm, HeightMm, CategoryId, Price) VALUES
('23.H20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Directional', 120, 70, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('24.H20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Directional', 150, 79, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('25.H20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Directional', 180, 115, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('26.H20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Directional', 200, 133, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('27.H20', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Directional', 250, 183, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL);

INSERT INTO Stoppers (PartNumber, SeriesId, MaterialId, StopperType, LengthMm, HeightMm, CategoryId, Price) VALUES
('23.H60', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Hand Lever', 120, 70, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('24.H60', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Hand Lever', 150, 79, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('25.H60', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Hand Lever', 180, 115, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('26.H60', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Hand Lever', 200, 133, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('27.H60', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Hand Lever', 250, 183, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL);

INSERT INTO Stoppers (PartNumber, SeriesId, MaterialId, StopperType, IsPneumatic, CategoryId, Price) VALUES
('23.H63', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic', 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('24.H63', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic', 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('25.H63', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic', 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('26.H63', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic', 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL),
('27.H63', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic', 1, (SELECT Id FROM Categories WHERE Code = 'STOPPERS'), NULL);

-- =============================================
-- Swivel Units
-- =============================================
INSERT INTO SwivelUnits (PartNumber, SeriesId, MaterialId, SwivelType, Direction, L1_Mm, L2_Mm, L3_Mm, CategoryId, Price) VALUES
('23.H15', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Right', 500, 500, 500, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('23.H25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Left', 500, 500, 500, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('24.H15', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Right', 500, 500, 500, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('24.H25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Left', 500, 500, 500, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('25.H15', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Right', 500, 500, 500, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('25.H25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Left', 500, 500, 500, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('26.H15', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Right', 700, 500, 700, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('26.H25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Left', 700, 500, 700, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('27.H15', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Right', 850, 600, 850, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('27.H25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Manual Lateral', 'Left', 850, 600, 850, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL);

INSERT INTO SwivelUnits (PartNumber, SeriesId, MaterialId, SwivelType, Direction, L1_Mm, L2_Mm, L3_Mm, IsPneumatic, CategoryId, Price) VALUES
('23.H16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Right', 500, 350, 500, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('23.H26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Left', 500, 350, 500, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('24.H16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Right', 500, 350, 500, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('24.H26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Left', 500, 350, 500, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('25.H16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Right', 500, 350, 500, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('25.H26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Left', 500, 350, 500, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('26.H16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Right', 700, 400, 700, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('26.H26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Left', 700, 400, 700, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('27.H16', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Right', 900, 450, 900, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('27.H26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Lateral', 'Left', 900, 450, 900, 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL);

INSERT INTO SwivelUnits (PartNumber, SeriesId, MaterialId, SwivelType, IsPneumatic, CategoryId, Price) VALUES
('23.H18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Vertical Upward', 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('24.H18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Vertical Upward', 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('25.H18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Vertical Upward', 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('26.H18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Vertical Upward', 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL),
('27.H18', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Pneumatic Vertical Upward', 1, (SELECT Id FROM Categories WHERE Code = 'SWIVEL'), NULL);

-- =============================================
-- Bridge Interlocks
-- =============================================
INSERT INTO BridgeInterlocks (PartNumber, SeriesId, MaterialId, PartType, WidthMm, HeightMm, LengthMm, CategoryId, Price) VALUES
('23.H01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 1', 50, 117, 120, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('24.H01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 1', 61, 126, 150, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('25.H01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 1', 81, 170, 180, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('26.H01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 1', 100, 188, 200, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('27.H01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 1', 116, 229, 250, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL);

INSERT INTO BridgeInterlocks (PartNumber, SeriesId, MaterialId, PartType, WidthMm, MinHeightMm, MaxHeightMm, LengthMm, CategoryId, Price) VALUES
('23.H02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 2', 50, 117, 147, 56, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('24.H02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 2', 61, 127, 157, 68, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('25.H02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 2', 81, 168, 210, 90, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('26.H02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 2', 100, 186, 233, 110, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('27.H02', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Part 2', 116, 291, 360, 120, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL);

-- Track Transfer Assembly
INSERT INTO BridgeInterlocks (PartNumber, SeriesId, MaterialId, PartType, WidthMm, HeightMm, LengthMm, CategoryId, Price) VALUES
('24.H21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 1', 190, 160, 150, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('25.H21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 1', 230, 220, 180, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('26.H21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 1', 260, 255, 200, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('27.H21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 1', 400, 295, 250, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('24.H22', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 2', 225, 215, 150, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('25.H22', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 2', 280, 260, 180, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('26.H22', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 2', 300, 315, 200, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL),
('27.H22', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Transfer Part 2', 400, 370, 250, (SELECT Id FROM Categories WHERE Code = 'INTERLOCK'), NULL);

-- =============================================
-- Drop-Lift Units
-- =============================================
INSERT INTO DropLiftUnits (PartNumber, SeriesId, MaterialId, LiftType, MinTrackLengthMm, ApproachLengthMm, HeightMm, SafeWorkingLoadKg, CategoryId, Price) VALUES
('24.H70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Standard', 1000, 550, 1150, 160, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL),
('25.H70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Standard', 1000, 650, 1200, 400, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL),
('26.H70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Standard', 1000, 750, 1250, 800, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL),
('27.H70', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Standard', 1000, 900, 1350, 1600, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL);

INSERT INTO DropLiftUnits (PartNumber, SeriesId, MaterialId, LiftType, MinTrackLengthMm, ApproachLengthMm, HeightMm, WidthMm, SafeWorkingLoadKg, CategoryId, Price) VALUES
('24.H71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Low Headroom', 1000, 550, 850, 600, 160, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL),
('25.H71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Low Headroom', 1000, 650, 900, 600, 400, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL),
('26.H71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Low Headroom', 1000, 750, 950, 700, 800, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL),
('27.H71', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Low Headroom', 1000, 900, 1050, 700, 1600, (SELECT Id FROM Categories WHERE Code = 'DROPLIFT'), NULL);

-- =============================================
-- Accessories
-- =============================================
INSERT INTO Accessories (PartNumber, SeriesId, MaterialId, AccessoryType, Description, LengthMm, HeightMm, WidthMm, CategoryId, Price) VALUES
('23.X01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track End Stop', 'End stop with rubber buffer', 70, 24, 20, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('24.X01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track End Stop', 'End stop with rubber buffer', 75, 30, 30, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('25.X01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track End Stop', 'End stop with rubber buffer', 120, 47, 40, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('26.X01', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track End Stop', 'End stop with rubber buffer', 130, 60, 50, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL);

INSERT INTO Accessories (PartNumber, SeriesId, MaterialId, AccessoryType, Description, HeightMm, WidthMm, CategoryId, Price) VALUES
('21.X19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Cover', 'End cap for sealing profile ends', 28, 30, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('23.X19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Cover', 'End cap for sealing profile ends', 35, 40, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('24.X19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Cover', 'End cap for sealing profile ends', 43.5, 48.5, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('25.X19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Cover', 'End cap for sealing profile ends', 60, 65, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('26.X19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Cover', 'End cap for sealing profile ends', 75, 80, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('27.X19', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Track Cover', 'End cap for sealing profile ends', 110, 90, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL);

INSERT INTO Accessories (PartNumber, SeriesId, MaterialId, AccessoryType, Description, LengthMm, CategoryId, Price) VALUES
('23.H30', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Dilation Joint', 'Thermal expansion/contraction joint', 240, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('24.H30', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Dilation Joint', 'Thermal expansion/contraction joint', 300, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('25.H30', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Dilation Joint', 'Thermal expansion/contraction joint', 360, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('26.H30', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Dilation Joint', 'Thermal expansion/contraction joint', 400, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('27.H30', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Dilation Joint', 'Thermal expansion/contraction joint', 500, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL);

-- Articulation Joint
INSERT INTO Accessories (PartNumber, SeriesId, MaterialId, AccessoryType, Description, LengthMm, CategoryId, Price) VALUES
('23.H08', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Articulation Joint', 'For lifting/lowering loads at track end', 120, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('24.H08', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Articulation Joint', 'For lifting/lowering loads at track end', 150, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('25.H08', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Articulation Joint', 'For lifting/lowering loads at track end', 180, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('26.H08', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Articulation Joint', 'For lifting/lowering loads at track end', 200, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL),
('27.H08', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), (SELECT Id FROM Materials WHERE Code = 'STL-GOLD'), 'Articulation Joint', 'For lifting/lowering loads at track end', 250, (SELECT Id FROM Categories WHERE Code = 'ACCESSORIES'), NULL);

-- =============================================
-- Bearing Options
-- =============================================
INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, MinTemperatureC, MaxTemperatureC, Description, Price) VALUES
('TL.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'Low Temperature', -50, 80, 'Special grease for low temperature operation', NULL),
('TL.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'Low Temperature', -50, 80, 'Special grease for low temperature operation', NULL),
('TL.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'Low Temperature', -50, 80, 'Special grease for low temperature operation', NULL),
('TL.26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), 'Low Temperature', -50, 80, 'Special grease for low temperature operation', NULL),
('TL.27', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), 'Low Temperature', -50, 80, 'Special grease for low temperature operation', NULL);

INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, MinTemperatureC, MaxTemperatureC, Description, Price) VALUES
('TH.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'High Temperature', -40, 260, 'Special grease for high temperature operation', NULL),
('TH.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'High Temperature', -40, 260, 'Special grease for high temperature operation', NULL),
('TH.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'High Temperature', -40, 260, 'Special grease for high temperature operation', NULL),
('TH.26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), 'High Temperature', -40, 260, 'Special grease for high temperature operation', NULL),
('TH.27', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), 'High Temperature', -40, 260, 'Special grease for high temperature operation', NULL);

INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, Description, Price) VALUES
('PL.21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), 'Nylon Tyred', 'Nylon tyred bearings for quiet operation', NULL),
('PL.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'Nylon Tyred', 'Nylon tyred bearings for quiet operation', NULL),
('PL.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'Nylon Tyred', 'Nylon tyred bearings for quiet operation', NULL),
('PL.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'Nylon Tyred', 'Nylon tyred bearings for quiet operation', NULL);

INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, Description, Price) VALUES
('PLN.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'Heavy Duty Polyamide', 'Heavy duty polyamide tyred bearings', NULL),
('PLN.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'Heavy Duty Polyamide', 'Heavy duty polyamide tyred bearings', NULL),
('PLN.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'Heavy Duty Polyamide', 'Heavy duty polyamide tyred bearings', NULL),
('PLN.26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), 'Heavy Duty Polyamide', 'Heavy duty polyamide tyred bearings', NULL),
('PLN.27', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), 'Heavy Duty Polyamide', 'Heavy duty polyamide tyred bearings', NULL);

INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, MinTemperatureC, MaxTemperatureC, Description, Price) VALUES
('AM.21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), 'Ammonia Resistant', -20, 130, 'Special grease for ammonia environments', NULL),
('AM.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'Ammonia Resistant', -20, 130, 'Special grease for ammonia environments', NULL),
('AM.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'Ammonia Resistant', -20, 130, 'Special grease for ammonia environments', NULL),
('AM.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'Ammonia Resistant', -20, 130, 'Special grease for ammonia environments', NULL),
('AM.26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), 'Ammonia Resistant', -20, 130, 'Special grease for ammonia environments', NULL),
('AM.27', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), 'Ammonia Resistant', -20, 130, 'Special grease for ammonia environments', NULL);

INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, MinTemperatureC, MaxTemperatureC, Description, Price) VALUES
('IN.21', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '21.000'), 'Stainless Steel', -20, 80, 'Stainless steel bearings', NULL),
('IN.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'Stainless Steel', -20, 80, 'Stainless steel bearings', NULL),
('IN.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'Stainless Steel', -20, 80, 'Stainless steel bearings', NULL),
('IN.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'Stainless Steel', -20, 80, 'Stainless steel bearings', NULL);

INSERT INTO BearingOptions (PartNumber, SeriesId, BearingType, Description, Price) VALUES
('PB.23', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '23.000'), 'Phosphor Bronze', 'Phosphor bronze tyred bearing', NULL),
('PB.24', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '24.000'), 'Phosphor Bronze', 'Phosphor bronze tyred bearing', NULL),
('PB.25', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '25.000'), 'Phosphor Bronze', 'Phosphor bronze tyred bearing', NULL),
('PB.26', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '26.000'), 'Phosphor Bronze', 'Phosphor bronze tyred bearing', NULL),
('PB.27', (SELECT Id FROM ProfileSeries WHERE SeriesCode = '27.000'), 'Phosphor Bronze', 'Phosphor bronze tyred bearing', NULL);

-- =============================================
-- Pneumatic Controls
-- =============================================
INSERT INTO PneumaticControls (PartNumber, ControlType, Description, TubeLengthM, CategoryId, Price) VALUES
('X7.003', 'Manual Valve Control', 'Manual valve with 10m tube for tongue switch control', 10, (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('X7.023', 'Air Distributor', '3+1 ways air distributor/manifold', NULL, (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('X7.024', 'Air Distributor', '4+1 ways air distributor/manifold', NULL, (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('X7.025', 'Air Distributor', '5+1 ways air distributor/manifold', NULL, (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL);

INSERT INTO PneumaticControls (PartNumber, ControlType, Description, CategoryId, Price) VALUES
('SL.23', 'Switch Lever', 'Switch lever for 23.000 series - 500mm reach', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('SL.24', 'Switch Lever', 'Switch lever for 24.000 series - 500mm reach', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('SL.25', 'Switch Lever', 'Switch lever for 25.000 series - 500mm reach', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('SL.26', 'Switch Lever', 'Switch lever for 26.000 series - 500mm reach', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('DL.23', 'Double Pulling Chain', 'Double pulling chain for 23.000 series - 1000mm length', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('DL.24', 'Double Pulling Chain', 'Double pulling chain for 24.000 series - 1000mm length', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('DL.25', 'Double Pulling Chain', 'Double pulling chain for 25.000 series - 1000mm length', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL),
('DL.26', 'Double Pulling Chain', 'Double pulling chain for 26.000 series - 1000mm length', (SELECT Id FROM Categories WHERE Code = 'PNEUMATIC'), NULL);
