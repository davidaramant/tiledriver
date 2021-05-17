using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.DemoMaps;
using Tiledriver.Core.ECWolfUtils;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Wad;

var lumps = CreateWadContents(new Func<TextureQueue,MapData>[]{
    TileDemoMap.CreateMapAndTextures,
    TexturesDemoMap.CreateMapAndTextures,
    ThingDemoMap.CreateMapAndTextures,
});

ExeFinder
    .CreateLauncher()
    .CreateAndLoadWadInEcWolf(lumps, "demo.wad");

static IEnumerable<ILump> CreateWadContents(IEnumerable<Func<TextureQueue, MapData>> mapCreators)
{
    var textureQueue = new TextureQueue();
    var maps = mapCreators.Select(creator => creator(textureQueue)).ToList();

    return new List<ILump>
        {
            DataLump.ReadFromStream("TEXTURES", stream => TexturesWriter.Write(textureQueue.Definitions, stream)),
        }
        .AddRangeAndContinue(
            textureQueue.RenderQueue.Select(r => DataLump.ReadFromStream(r.Item2.Name, r.Item1.RenderTo)))
        .AddRangeAndContinue(maps.SelectMany((map, index) => new ILump[]
        {
            new Marker($"MAP{index + 1:00}"),
            new UwmfLump("TEXTMAP", map),
            new Marker("ENDMAP")
        }));
}