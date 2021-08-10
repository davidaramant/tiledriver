// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Utils
{
    public static class Interpolate
    {
        /// <summary>
        /// Linearly interpolate between <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <param name="p">Percentage: 0 - 1</param>
        public static double Linear(double a, double b, double p) => (1 - p) * a + p * b;

        /// <summary>
        /// Linearly interpolate between <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <param name="p">Percentage: 0 - 1</param>
        public static float Linear(float a, float b, float p) => (1 - p) * a + p * b;
    }
}