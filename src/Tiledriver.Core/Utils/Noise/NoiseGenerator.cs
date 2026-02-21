namespace Tiledriver.Core.Utils.Noise;

public interface INoiseGenerator
{
	float[,] Generate(int width, int height, float scale, int? seed = null);
}

public sealed class NoiseGenerator : INoiseGenerator
{
	public float[,] Generate(int width, int height, float scale, int? seed = null)
	{
		if (seed is not null)
			SimplexNoise.Noise.Seed = seed.Value;

		return SimplexNoise.Noise.Calc2D(width, height, scale);
	}
}
