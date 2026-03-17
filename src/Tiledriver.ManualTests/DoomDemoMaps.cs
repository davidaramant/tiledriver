using System.Runtime.CompilerServices;
using Tiledriver.DemoMaps.Doom;
using Tiledriver.Extensions.Collections;
using Tiledriver.FormatModels.Textures;
using Tiledriver.FormatModels.Textures.Writing;
using Tiledriver.FormatModels.Udmf;
using Tiledriver.FormatModels.Wad;
using Tiledriver.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Settings;

namespace Tiledriver.ManualTests;

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
