// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public static class LightMapVisualizer
    {
        public static IFastImage Render(LightMap lightMap, int scale = 10)
        {
            var image = new FastImage(lightMap.Size.Width, lightMap.Size.Height, scale);

            var darkIncrement = 128 / lightMap.Range.DarkLevels;
            var lightIncrement = 128 / lightMap.Range.LightLevels;

            Parallel.For(0, lightMap.Size.Height, y =>
            {
                for (int x = 0; x < lightMap.Size.Width; x++)
                {
                    byte intensity = 128;
                    var light = lightMap[x, y];
                    if (light > 0)
                    {
                        intensity += (byte)(light * lightIncrement);
                    }
                    else
                    {
                        intensity += (byte)(light * darkIncrement);

                    }
                    image.SetPixel(x, y, new SKColor(intensity, intensity, intensity));
                }
            });

            return image;
        }

        public static IFastImage Render(LightMap lightMap, IEnumerable<LightDefinition> lights, int scale = 10)
        {
            var image = Render(lightMap, scale);
            foreach (var light in lights)
            {
                image.SetPixel(light.Center.X, light.Center.Y, SKColors.Red);
            }

            return image;
        }

        public static IFastImage Render(LightMap lightMap, IEnumerable<LightDefinition> lights, ConnectedArea area, int scale = 10)
        {
            var image = Render(lightMap, lights, scale);

            foreach (var position in lightMap.Size.GetAllPositions().Where(p => !area.Contains(p)))
            {
                image.SetPixel(position.X, position.Y, SKColors.DarkSlateBlue);
            }

            return image;
        }
    }
}