// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SkiaSharp;
using WangscapeTilesetChopper.Model;

var wangscapeDirectory = Path.Combine(FindSolutionPath(), "..", "WangscapeTilesets", "Cave");

var inputDirectory = Path.Combine(wangscapeDirectory, "output");
var outputDirectory = Path.Combine(wangscapeDirectory, "tiles");

if (!Directory.Exists(inputDirectory))
{
	throw new DirectoryNotFoundException("Have you run Wangscape? Could not find: " + inputDirectory);
}

var tilesJsonFile = Path.Combine(inputDirectory, "tiles.json");
var tilesetsJsonFile = Path.Combine(inputDirectory, "tilesets.json");
var tileSheetFile = Directory.EnumerateFiles(inputDirectory, "*.png").Single();

var tileDefinitions = Deserialize<List<TileDefinition>>(tilesJsonFile);
var tilesetDefinition = Deserialize<List<TilesetDefinition>>(tilesetsJsonFile).Single();

if (Directory.Exists(outputDirectory))
{
	Directory.Delete(outputDirectory, recursive: true);
}
Directory.CreateDirectory(outputDirectory);

using var tileSheetStream = File.OpenRead(tileSheetFile);
using var tileSheet = SKBitmap.FromImage(SKImage.FromEncodedData(tileSheetStream));

foreach (var definition in tileDefinitions)
{
	using var tile = new SKBitmap(tilesetDefinition.Width, tilesetDefinition.Height);
	using (var tileCanvas = new SKCanvas(tile))
	{
		tileCanvas.DrawBitmap(
			tileSheet,
			SKRect.Create(definition.X, definition.Y, tilesetDefinition.Width, tilesetDefinition.Height),
			SKRect.Create(0, 0, tilesetDefinition.Width, tilesetDefinition.Height)
		);
	}

	using var tileStream = File.Open(Path.Combine(outputDirectory, definition.GetFileName()), FileMode.Create);
	tile.Encode(tileStream, SKEncodedImageFormat.Png, quality: 100);
}

static string FindSolutionPath()
{
	var path = "..";

	while (!File.Exists(Path.Combine(path, "Tiledriver.sln")))
	{
		path = Path.Combine(path, "..");
	}

	return Path.GetFullPath(path);
}

static T Deserialize<T>(string filePath)
{
	// TODO: Is there seriously no better way to read from a file?
	var serialized = File.ReadAllText(filePath);

	return JsonSerializer.Deserialize<T>(serialized, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
		?? throw new ArgumentException($"Bad format for {Path.GetFileName(filePath)}");
}
