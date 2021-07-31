// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Lighting;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.Lighting
{
    public sealed class LightTracerManualTests
    {
        //[Fact]
        public void ShouldGenerateVisualizationOfSimpleLightMap()
        {
            MapData map = DemoMaps.TileDemoMap.Create();

            var (floorLights, ceilingLights) =
                LightTracer.Trace(
                    map,
                    new LightRange(DarkLevels: 10, LightLevels: 10),
                    new LightDefinition[]
                    {
                        new LightDefinition(new Position(1,1),Brightness:1,Radius:1)
                    });

            var image = LightMapVisualizer.Render(floorLights);
            image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output.png"));
        }
    }
}