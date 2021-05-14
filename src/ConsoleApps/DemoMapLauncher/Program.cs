using System.IO;
using Tiledriver.Core.DemoMaps;
using Tiledriver.Core.ECWolfUtils;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Tiledriver.Core.FormatModels.Wad;

// TODO: Clean this up somehow
var textureQueue = new TextureQueue();
var map = TileDemoMap.CreateMapAndTextures(textureQueue);

var ms = new MemoryStream();
TexturesWriter.Write(textureQueue.Definitions, ms);

var wad = new WadFile();
wad.Append(new DataLump("TEXTURES", ms.ToArray()));

foreach (var rendered in textureQueue.RenderQueue)
{
    var tempStream = new MemoryStream();
    rendered.Item1.RenderTo(tempStream);
    wad.Append(new DataLump(rendered.Item2.Name, tempStream.ToArray()));
}

wad.Append(new Marker("MAP01"));
wad.Append(new UwmfLump("TEXTMAP", map));
wad.Append(new Marker("ENDMAP"));


ExeFinder
    .CreateLauncher()
    .LoadWadInEcWolf(wad, "demo.wad");
//    .LoadMapInEcWolf(ThingDemoMap.Create());
//    .LoadMapInEcWolf(TileDemoMap.Create());
