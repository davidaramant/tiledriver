using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.DemoMaps;
using Tiledriver.Core.ECWolfUtils;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Tiledriver.Core.FormatModels.Wad;

// TODO: Clean this up somehow
var textureQueue = new TextureQueue();
var map = TileDemoMap.CreateMapAndTextures(textureQueue);

var lumps = new List<ILump>
{
    DataLump.ReadFromStream("TEXTURES", stream => TexturesWriter.Write(textureQueue.Definitions, stream)),
};

lumps.AddRange(textureQueue.RenderQueue.Select(r=>DataLump.ReadFromStream(r.Item2.Name, r.Item1.RenderTo)));
lumps.Add(new Marker("MAP01"));
lumps.Add(new UwmfLump("TEXTMAP", map));
lumps.Add(new Marker("ENDMAP"));


ExeFinder
    .CreateLauncher()
    .CreateAndLoadWadInEcWolf(lumps, "demo.wad");
//    .LoadMapInEcWolf(ThingDemoMap.Create());
//    .LoadMapInEcWolf(TileDemoMap.Create());
