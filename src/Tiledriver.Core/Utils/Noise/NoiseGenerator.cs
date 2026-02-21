namespace Tiledriver.Core.Utils.Noise;

public static class NoiseGenerator
{
	public static float Generate2D(double x, double y, long seed = 0) => OpenSimplex2.Noise2(seed: seed, x: x, y: y);
}
