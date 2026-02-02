// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Runtime.CompilerServices;
using NUnit.Framework;
using Tiledriver.Core.DemoMaps.Doom;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.Settings;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class DoomDemoMaps
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Doom Demo Maps");

	[Test, Explicit]
	public void BoxDemo() => Load(CreateWadContents([tq => DemoMap.Create()]));

	[Test, Explicit]
	public void CaveMap() => Load(CreateWadContents([tq => DoomCaveMapGenerator.Create(seed: 13, tq)]));

	void Load(IEnumerable<ILump> contents, [CallerMemberName] string? name = null)
	{
		ConfigLoader
			.Load()
			.CreateUZDoomLauncher()
			.CreateAndLoadWad(contents, Path.Combine(_dirInfo.FullName, (name ?? "demo") + ".wad"));
	}

	static IEnumerable<ILump> CreateWadContents(
		IEnumerable<Func<TextureQueue, MapData>> mapCreators,
		IReadOnlyCollection<(string Name, byte[] Data)>? extraTextures = null
	)
	{
		extraTextures ??= [];
		var textureQueue = new TextureQueue();
		var maps = mapCreators.Select(creator => creator(textureQueue)).ToList();

		var textureLumps = new List<ILump>();
		if (extraTextures.Any() || textureQueue.RenderQueue.Any())
		{
			textureLumps.Add(new Marker("P_START"));
			textureLumps.AddRange(extraTextures.Select(pair => new DataLump(pair.Name, pair.Data)));
			textureLumps.AddRange(
				textureQueue.RenderQueue.Select(r => DataLump.ReadFromStream(r.Item2.Name, r.Item1.RenderTo))
			);
			textureLumps.Add(new Marker("P_END"));
		}

		var lumps = new List<ILump>();

		if (textureQueue.Definitions.Any())
		{
			lumps.Add(
				DataLump.ReadFromStream("TEXTURES", stream => TexturesWriter.Write(textureQueue.Definitions, stream))
			);
		}

		lumps
			.AddRangeAndContinue(textureLumps)
			.AddRangeAndContinue(
				maps.SelectMany(
					(map, index) =>
						new ILump[]
						{
							new Marker($"MAP{index + 1:00}"),
							new UdmfLump("TEXTMAP", map),
							new Marker("ENDMAP"),
						}
				)
			);

		return lumps;
	}
}
