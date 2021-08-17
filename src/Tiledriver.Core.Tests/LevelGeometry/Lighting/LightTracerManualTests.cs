// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Text;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Lighting;
using Xunit;
using Xunit.Abstractions;

namespace Tiledriver.Core.Tests.LevelGeometry.Lighting
{
    public sealed class LightTracerManualTests
    {
        private readonly ITestOutputHelper _output;

        public LightTracerManualTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //[Fact]
        public void ShouldGenerateVisualizationOfSimpleLightMap()
        {
            MapData map = TileDemoMap.Create();

            var (floorLights, ceilingLights) =
                LightTracer.Trace(
                    map,
                    new LightRange(DarkLevels: 10, LightLevels: 10),
                    new LightDefinition[]
                    {
                        new LightDefinition(new Position(1,1),Brightness:20,Radius:20),
                        new LightDefinition(new Position(map.Width-2,map.Height-2),Brightness:20,Radius:20),
                        new LightDefinition(new Position(map.Width-2,1),Brightness:20,Radius:20),
                        new LightDefinition(new Position(1,map.Height-2),Brightness:20,Radius:20),
                    });

            //StringBuilder line = new StringBuilder();
            //for (int y = 0; y < map.Height; y++)
            //{
            //    line.Clear();
            //    for (int x = 0; x < map.Width; x++)
            //    {
            //        line.Append(floorLights[x, y]);
            //    }
            //    _output.WriteLine(line.ToString());
            //}

            var image = LightMapVisualizer.Render(floorLights, scale: 20);
            image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_output.png"));
        }
    }
}