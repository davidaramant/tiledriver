using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class NoiseBenchmarks
{
	private const int Points = 1000;

	[Benchmark(Baseline = true)]
	public double OpenSimplex2S()
	{
		double sum = 0;

		for (int i = 0; i < Points; i++)
		{
			sum += Tiledriver.Core.Utils.Noise.OpenSimplex2S.Noise2(0, i, i);
		}

		return sum;
	}

	[Benchmark]
	public double OpenSimplex2()
	{
		double sum = 0;

		for (int i = 0; i < Points; i++)
		{
			sum += Tiledriver.Core.Utils.Noise.OpenSimplex2.Noise2(0, i, i);
		}

		return sum;
	}
}
