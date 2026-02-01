// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CommandLine;
using System.CommandLine.Parsing;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.MapMetadata.Writing;

namespace mm;

/// <summary>
/// Turns a metamap into an image
/// </summary>
class Program
{
	record Options(string InputMapPath, ImagePalette Theme, string? ImageName, int Scale);

	public enum ImagePalette
	{
		CarveOutRooms,
		HighlightWalls,
		Full,
	}

	static int Main(string[] args)
	{
		Option<string> inputMapPathOption = new("-i", "Input path of metamap");
		Option<ImagePalette> themeOption = new("-t", "Theme to use for the image.");
		Option<string?> imageNameOption = new("-n", "Name of the image file (by default, use the map name)")
		{
			Required = false,
		};
		Option<int> scaleOption = new("-s", "How much to scale the output image.")
		{
			Required = false,
			DefaultValueFactory = _ => 1,
		};

		RootCommand root = new("Turns a metamap into an image");
		root.Options.Add(inputMapPathOption);
		root.Options.Add(themeOption);
		root.Options.Add(imageNameOption);
		root.Options.Add(scaleOption);

		ParseResult parseResult = root.Parse(args);
		if (parseResult.Errors.Count == 0)
		{
			RunOptions(
				new Options(
					InputMapPath: parseResult.GetRequiredValue(inputMapPathOption),
					Theme: parseResult.GetRequiredValue(themeOption),
					ImageName: parseResult.GetValue(imageNameOption),
					Scale: parseResult.GetRequiredValue(scaleOption)
				)
			);
			return 0;
		}
		foreach (ParseError error in parseResult.Errors)
		{
			Console.Error.WriteLine(error.Message);
		}

		return 1;
	}

	private static void RunOptions(Options opts)
	{
		var map = MetaMap.Load(opts.InputMapPath);

		var imageName = opts.ImageName;
		if (!string.IsNullOrEmpty(imageName))
		{
			if (!imageName.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
			{
				imageName += ".png";
			}
		}
		else
		{
			imageName = Path.GetFileNameWithoutExtension(opts.InputMapPath) + ".png";
		}

		static MapPalette PickPalette(ImagePalette palette) =>
			palette switch
			{
				ImagePalette.HighlightWalls => MapPalette.HighlightWalls,
				ImagePalette.Full => MapPalette.Full,
				_ => MapPalette.CarveOutRooms,
			};

		var imagePath = Path.Combine(Path.GetDirectoryName(opts.InputMapPath)!, imageName);
		MetaMapImageExporter.Export(map, PickPalette(opts.Theme), imagePath, scale: opts.Scale);

		Console.WriteLine($"Wrote {imagePath} with color theme {opts.Theme} at scale {opts.Scale}x.");
	}
}
