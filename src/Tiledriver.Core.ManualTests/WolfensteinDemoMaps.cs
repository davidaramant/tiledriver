// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.LevelGeometry.CaveGeneration;
using Tiledriver.Core.Settings;

namespace Tiledriver.Core.ManualTests
{
    [TestFixture]
    public class WolfensteinDemoMaps
    {
        private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Wolf3D Demo Maps");

        [Test, Explicit]
        public void All()
        {
            Load(CreateWadContents(new Func<TextureQueue, MapData>[]{
                TexturesDemoMap.CreateMapAndTextures,
                TileDemoMap.CreateMapAndTextures,
                ThingDemoMap.CreateMapAndTextures,
            }));
        }

        [Test, Explicit]
        public void TexturesDemo() =>
            Load(CreateWadContents(new Func<TextureQueue, MapData>[]{
                TexturesDemoMap.CreateMapAndTextures
            }));

        [Test, Explicit]
        public void TileDemo() =>
            Load(CreateWadContents(new Func<TextureQueue, MapData>[]{
                TileDemoMap.CreateMapAndTextures
            }));

        [Test, Explicit]
        public void ThingDemo() =>
            Load(CreateWadContents(new Func<TextureQueue, MapData>[]{
                ThingDemoMap.CreateMapAndTextures
            }));

        [Test, Explicit]
        public void CaveMap()
        {
            var textureQueue = new TextureQueue();

            textureQueue.Add(
                new CompositeTexture("CRSFA0",
                    Width: 35,
                    Height: 37,
                    Namespace: TextureNamespace.Sprite,
                    XScale: 2,
                    YScale: 2,
                    Offset: new TextureOffset(17,32),
                    Patches: ImmutableArray.Create(new Patch("CRYSTAL", 0, 0))),
                new CompositeTexture("CRSCA0", 
                    Width: 35, 
                    Height: 37, 
                    Namespace: TextureNamespace.Sprite, 
                    XScale: 2,
                    YScale: 2,
                    Offset: new TextureOffset(17,133),
                    Patches: ImmutableArray.Create(new Patch("CRYSTAL", 0, 0, FlipY: true))));

            var map = WolfCaveMapGenerator.Create(seed: 13, texturePrefix: "TILE", textureQueue: textureQueue);

            Load(new List<ILump> { new Marker("P_START") }
                .AddRangeAndContinue(
                    Enumerable.Range(0, 16)
                        .Select(i => new DataLump(
                            $"TILE{i:d2}",
                            (byte[])(Resource.ResourceManager.GetObject($"tile{i:00}", CultureInfo.InvariantCulture) ??
                                     throw new ArgumentException("Somehow the name was wrong")))))
                .AddRangeAndContinue(textureQueue.RenderQueue.Select(r =>
                    DataLump.ReadFromStream(r.Item2.Name, r.Item1.RenderTo)))
                .AddRangeAndContinue(new ILump[]
                {
                    new Marker("P_END"),
                    new Marker("S_START"),
                    new DataLump("CRYSTAL", Resource.crystal),
                    new Marker("S_END"),
                    DataLump.ReadFromStream("TEXTURES",
                        stream => TexturesWriter.Write(textureQueue.Definitions, stream)),
                    new DataLump("DECORATE",
                        @"actor CeilingCrystal
{
	states
	{
		Spawn:
			CRSC A -1
			stop
	}
}
actor FloorCrystal
{
	states
	{
		Spawn:
			CRSF A -1
			stop
	}
}"),
                    new Marker("MAP01"),
                    new UwmfLump("TEXTMAP", map),
                    new Marker("ENDMAP")
                }));
        }

        void Load(IEnumerable<ILump> contents, [CallerMemberName] string? name = null)
        {
            ConfigLoader
                .Load()
                .CreateECWolfLauncher()
                .CreateAndLoadWad(contents, Path.Combine(_dirInfo.FullName, (name ?? "demo") + ".wad"));
        }

        IEnumerable<ILump> CreateWadContents(
            IEnumerable<Func<TextureQueue, MapData>> mapCreators)
        {
            var textureQueue = new TextureQueue();
            var maps = mapCreators.Select(creator => creator(textureQueue)).ToList();

            var textureLumps = new List<ILump>();
            if (textureQueue.RenderQueue.Any())
            {
                textureLumps.Add(new Marker("P_START"));
                textureLumps.AddRange(textureQueue.RenderQueue.Select(r => DataLump.ReadFromStream(r.Item2.Name, r.Item1.RenderTo)));
                textureLumps.Add(new Marker("P_END"));
            }

            var lumps = new List<ILump>
                {
                    DataLump.ReadFromStream("TEXTURES", stream => TexturesWriter.Write(textureQueue.Definitions, stream)),
                };

            
            lumps.AddRangeAndContinue(textureLumps)
            .AddRangeAndContinue(maps.SelectMany((map, index) => new ILump[]
            {
                    new Marker($"MAP{index + 1:00}"),
                    new UwmfLump("TEXTMAP", map),
                    new Marker("ENDMAP")
            }));

            return lumps;
        }
    }
}