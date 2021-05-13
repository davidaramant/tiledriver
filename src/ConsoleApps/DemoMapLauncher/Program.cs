using System.IO;
using Tiledriver.Core.DemoMaps;
using Tiledriver.Core.ECWolfUtils;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Tiledriver.Core.FormatModels.Wad;

// TODO: Clean this up somehow
var (map, textures) = TexturesDemoMap.CreateMapAndTextures();

var ms = new MemoryStream();
TexturesWriter.Write(textures,ms);

var wad = new WadFile();
wad.Append(new DataLump("TEXTURES", ms.ToArray()));
wad.Append(new Marker("MAP01"));
wad.Append(new UwmfLump("TEXTMAP", map));
wad.Append(new Marker("ENDMAP"));


ExeFinder
    .CreateLauncher()
    .LoadWadInEcWolf(wad, "demo.wad");
//    .LoadMapInEcWolf(ThingDemoMap.Create());
//    .LoadMapInEcWolf(TileDemoMap.Create());
