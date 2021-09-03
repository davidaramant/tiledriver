// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using System.Linq;
using Tiledriver.Core.LevelGeometry.CaveGeneration;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;
using NUnit.Framework;
using Tiledriver.Core.Utils.CellularAutomata;

namespace Tiledriver.Core.ManualTests
{
    [TestFixture]
    public class LightTracerVisualization
    {
        private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Light Tracer");
        private const int Seed = 13;

        [Test, Explicit]
        public void ShowFloorVsCeilingLighting()
        {
            void SaveImage(IFastImage image, string description) =>
                image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

            var random = new Random(Seed);
            var board =
                new CellBoard(new(128, 128))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 3)
                    .GenerateStandardCave();

            var (largestComponent, dimensions) =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                    .OrderByDescending(component => component.Area)
                    .First()
                    .TrimExcess(border: 1);

            // Place some lights
            var lightRange = new LightRange(DarkLevels: 15, LightLevels: 5);
            var lights = CaveThingPlacement.RandomlyPlaceLights(
                    largestComponent,
                    random,
                    lightRange,
                    percentAreaToCover: 0.01,
                    varyHeight: true)
                .ToArray();

            var (floorLighting, ceilingLight) =
                LightTracer.Trace(dimensions, p => board[p] == CellType.Alive, lightRange, lights);

            using var floorImg = LightMapVisualizer.Render(floorLighting, lights, largestComponent, scale:5);
            using var ceilingImg = LightMapVisualizer.Render(ceilingLight, lights, largestComponent, scale:5);

            SaveImage(floorImg, "Floor");
            SaveImage(ceilingImg, "Ceiling");
        }
    }
}