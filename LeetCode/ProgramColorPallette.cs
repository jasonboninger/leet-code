using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace LeetCode
{
	public static class ProgramColorPallette
	{
		private const int _IMAGE_SIZE = 2048;
		private const byte _COLOR_INCREMENT = 4;
		private const int _ZONE_SIZE = 3;

		public static void Run()
		{
			// Create image and lookup
			var (image, lookup) = _CreateColorPalette();
			// Get file
			var file = new FileInfo("./Colors.png");
			// Save image
			image.Save(file.FullName, ImageFormat.Png);
			// Log image
			Console.WriteLine();
			Console.WriteLine($"Colors image saved to {file.FullName}.");
			Console.WriteLine();
			// Create log instructions
			static void logInstructions()
			{
				Console.WriteLine("Enter any RGB color (e.g. 255, 255, 255) to lookup its UV coordinates:");
				Console.WriteLine();
			}
			// Log instructions
			logInstructions();
			// Create regular expression
			var regularExpression = new Regex("^[^0-9]*(?<RED>[0-9]{1,3})[^0-9]+(?<GREEN>[0-9]{1,3})[^0-9]+(?<BLUE>[0-9]{1,3})[^0-9]*$");
			// Provide lookup
			while (true)
			{
				// Get input
				var input = Console.ReadLine() ?? string.Empty;
				// Check if clear
				switch (input.ToUpper())
				{
					case "CLEAR":
					case "CLS":
						// Clear console
						Console.Clear();
						// Add line
						Console.WriteLine();
						// Log instructions
						logInstructions();
						// Continue loop
						continue;
				}
				// Get input and match with regular expression
				var regularExpressionMatch = regularExpression.Match(input);
				// Create round RGB value
				static int roundRgbValue(byte value) => value == 255 ? 255 : (value / _COLOR_INCREMENT * _COLOR_INCREMENT);
				// Check if match is success and color can be parsed
				if
				(
					regularExpressionMatch.Success
					&& byte.TryParse(regularExpressionMatch.Groups["RED"].Value, out var red)
					&& byte.TryParse(regularExpressionMatch.Groups["GREEN"].Value, out var green)
					&& byte.TryParse(regularExpressionMatch.Groups["BLUE"].Value, out var blue)
					&& lookup.TryGetValue(Color.FromArgb(roundRgbValue(red), roundRgbValue(green), roundRgbValue(blue)), out var uv)
				)
				{
					// Get x and y
					var (x, y) = uv;
					// Invert y
					y = Math.Max(0, Math.Min(1, 1 - y));
					// Log UV
					Console.WriteLine($"({x}, {y})");
					Console.WriteLine();
				}
				else
				{
					// Log error
					Console.WriteLine("Invalid RGB color input.");
					Console.WriteLine();
				}
			}
		}

		private static (Bitmap image, Dictionary<Color, (float x, float y)> lookup) _CreateColorPalette()
		{
			// Create image
			var image = new Bitmap(width: _IMAGE_SIZE, height: _IMAGE_SIZE);
			// Create lookup
			var lookup = new Dictionary<Color, (float x, float y)>();
			// Get zone colors
			var zoneColors = _GetColorPaletteColors();
			// Get zone length
			var zoneLength = _IMAGE_SIZE - _ZONE_SIZE;
			// Create zone indexes
			int x, y, xZone, yZone;
			// Get empty zone color
			var zoneColorEmpty = Color.FromArgb(alpha: 255, red: 0, green: 0, blue: 0);
			// Run through zones
			for (x = 0; x < zoneLength; x += _ZONE_SIZE)
			{
				for (y = 0; y < zoneLength; y += _ZONE_SIZE)
				{
					// Get zone color exists
					var zoneColorExists = zoneColors.MoveNext();
					// Get zone color
					var zoneColor = zoneColorExists ? zoneColors.Current : zoneColorEmpty;
					// Set zone
					for (xZone = 0; xZone < _ZONE_SIZE; xZone++)
					{
						for (yZone = 0; yZone < _ZONE_SIZE; yZone++)
						{
							// Set pixel color
							image.SetPixel(x + xZone, y + yZone, zoneColor);
						}
					}
					// Check if zone color exists
					if (zoneColorExists)
					{
						// Get pixel offset
						var pixelOffset = 0.5f;
						// Get zone offset
						var zoneOffset = (_ZONE_SIZE - 1) * 0.5f;
						// Add lookup
						lookup.Add(zoneColor, ((x + pixelOffset + zoneOffset) / _IMAGE_SIZE, (y + pixelOffset + zoneOffset) / _IMAGE_SIZE));
					}
				}
			}
			// Run through wasted area
			for (x = zoneLength; x < _IMAGE_SIZE; x++)
			{
				for (y = 0; y < _IMAGE_SIZE; y++)
				{
					// Set pixel color
					image.SetPixel(x, y, zoneColorEmpty);
				}
			}
			for (x = 0; x < _IMAGE_SIZE; x++)
			{
				for (y = zoneLength; y < _IMAGE_SIZE; y++)
				{
					// Set pixel color
					image.SetPixel(x, y, zoneColorEmpty);
				}
			}
			// Check if next zone color exists
			if (zoneColors.MoveNext())
			{
				// Throw error
				throw new InvalidOperationException("Too many zone colors to fit into the image.");
			}
			// Return image and lookup
			return (image, lookup);
		}

		private static IEnumerator<Color> _GetColorPaletteColors()
		{
			// Create RGB length
			var rgbLength = 256;
			// Check if RGB length is not divisible by color increment
			if (rgbLength % _COLOR_INCREMENT != 0)
			{
				// Create message
				var message = $"Color increment ({_COLOR_INCREMENT}) that does not evenly divide into RGB length ({rgbLength}) is not supported.";
				// Throw error
				throw new InvalidOperationException(message);
			}
			// Create RGB indexes
			int r, g, b;
			// Run through RGBs
			for (r = 0; r <= rgbLength; r += _COLOR_INCREMENT)
			{
				for (g = 0; g <= rgbLength; g += _COLOR_INCREMENT)
				{
					for (b = 0; b <= rgbLength; b += _COLOR_INCREMENT)
					{
						// Return color
						yield return Color.FromArgb
							(
								alpha: 255,
								red: Math.Min(255, r),
								green: Math.Min(255, g),
								blue: Math.Min(255, b)
							);
					}
				}
			}
		}
	}
}
