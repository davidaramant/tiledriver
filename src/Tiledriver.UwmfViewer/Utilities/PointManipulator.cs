using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Tiledriver.UwmfViewer.Utilities
{
    class PointManipulator
    {
        public static int[] CreatePath(int[] input, double width, double height, double left, double top, double degrees)
        {
            var rads = degrees * PI / 180;

            var doubles = ToDoubles(input);

            var scaled = Scale(doubles, width, height);
            var rotated = scaled;
            if (rads != 0)
            {
                rotated = Rotate(scaled, rads);
            }
            var translated = Translate(rotated, top, left);
            return RoundToInts(translated);
        }

        private static double[] Scale(double[] input, double width, double height)
        {
            var max = input.Max();
            var scaleWidth = width / max;
            var scaleHeight = height / max;

            return ByPairs(input,
                x => x * scaleWidth,
                y => y * scaleHeight);
        }

        private static double[] Rotate(double[] input, double rads)
        {
            var midX = ReduceX(input, Max) / 2;
            var midY = ReduceY(input, Max) / 2;

            var sin = Sin(rads);
            var cos = Cos(rads);

            var centered = Translate(input, -midX, -midY);
            var rotated = ByPairs(centered,
                (x, y) => x * cos - y * sin,
                (x, y) => x * sin + y * cos);
            var movedBack = Translate(rotated, midX, midY);

            return movedBack;
        }

        private static double[] Translate(double[] input, double top, double left)
        {
            return ByPairs(input,
                x => x + left,
                y => y + top);
        }

        private static double[] ByPairs(double[] input, Func<double,double> onXs, Func<double,double> onYs)
        {
            var output = new double[input.Length];
            for (int i = 0; i < input.Length; i += 2)
            {
                output[i] = onXs(input[i]);
                output[i + 1] = onYs(input[i + 1]);
            }
            return output;
        }

        private static double[] ByPairs(double[] input, Func<double, double, double> onXs, Func<double, double, double> onYs)
        {
            var output = new double[input.Length];
            for (int i = 0; i < input.Length; i += 2)
            {
                output[i] = onXs(input[i], input[i + 1]);
                output[i + 1] = onYs(input[i], input[i + 1]);
            }
            return output;
        }

        private static double ReduceX(double[] input, Func<double, double, double> func)
        {
            return Reduce(input, func, 0);
        }

        private static double ReduceY(double[] input, Func<double, double, double> func)
        {
            return Reduce(input, func, 1);
        }

        private static double Reduce(double[] input, Func<double, double, double> func, int start)
        {
            var output = 0.0;
            for (int i = start; i < input.Length; i += 2)
            {
                output = func(input[i], output);
            }
            return output;
        }

        private static double[] ToDoubles(int[] ints)
        {
            var doubles = new double[ints.Length];
            for (int i = 0; i < ints.Length; i++)
            {
                doubles[i] = ints[i];
            }
            return doubles;
        }

        private static int[] RoundToInts(double[] doubles)
        {
            var ints = new int[doubles.Length];
            for (int i = 0; i < doubles.Length; i++)
            {
                ints[i] = (int)(doubles[i] + 0.5);
            }
            return ints;
        }
    }
}
