using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Tiledriver.FormatModels.Udmf;
using Tiledriver.FormatModels.Udmf.Reading;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 1, iterationCount: 20, id: "UdmfRefresh")]
public class UdmfParsingBenchmark
{
	private byte[] _udmfBytes = default!;
	private string _udmfText = default!;
	private string _thingOnlyUdmfText = default!;
	private string _lineDefOnlyUdmfText = default!;

	[GlobalSetup]
	public void ReadStream()
	{
		string projectDirectory = FindProjectDirectory();
		string udmfDirectory = Path.Combine(projectDirectory, "Udmf");
		string largestTextMapPath =
			Directory
				.EnumerateFiles(udmfDirectory, "*.txt", SearchOption.TopDirectoryOnly)
				.OrderByDescending(path => new FileInfo(path).Length)
				.FirstOrDefault()
			?? throw new InvalidOperationException($"No .txt files were found in '{udmfDirectory}'.");

		_udmfBytes = File.ReadAllBytes(largestTextMapPath);
		_udmfText = Encoding.ASCII.GetString(_udmfBytes);
		_thingOnlyUdmfText = CreateTopLevelSubset(_udmfText, "thing");
		_lineDefOnlyUdmfText = CreateTopLevelSubset(_udmfText, "linedef");
	}

	[GlobalCleanup]
	public void Cleanup()
	{
		_udmfBytes = [];
		_udmfText = string.Empty;
		_thingOnlyUdmfText = string.Empty;
		_lineDefOnlyUdmfText = string.Empty;
	}

	[Benchmark(Baseline = true)]
	public MapData ParseUdmf()
	{
		using var stream = CreateStream();
		return UdmfReader.Read(stream);
	}

	[Benchmark(OperationsPerInvoke = 4)]
	public int ParseUdmfFromPreparedText()
	{
		MapData map1 = ParseUdmfFromPreparedTextCore(_udmfText);
		MapData map2 = ParseUdmfFromPreparedTextCore(_udmfText);
		MapData map3 = ParseUdmfFromPreparedTextCore(_udmfText);
		MapData map4 = ParseUdmfFromPreparedTextCore(_udmfText);

		return GetMapItemCount(map1) + GetMapItemCount(map2) + GetMapItemCount(map3) + GetMapItemCount(map4);
	}

	[Benchmark(OperationsPerInvoke = 12)]
	public int ParseLineDefsFromPreparedText()
	{
		MapData map1 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map2 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map3 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map4 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map5 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map6 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map7 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map8 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map9 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map10 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map11 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);
		MapData map12 = ParseUdmfFromPreparedTextCore(_lineDefOnlyUdmfText);

		return map1.LineDefs.Length
			+ map2.LineDefs.Length
			+ map3.LineDefs.Length
			+ map4.LineDefs.Length
			+ map5.LineDefs.Length
			+ map6.LineDefs.Length
			+ map7.LineDefs.Length
			+ map8.LineDefs.Length
			+ map9.LineDefs.Length
			+ map10.LineDefs.Length
			+ map11.LineDefs.Length
			+ map12.LineDefs.Length;
	}

	[Benchmark(OperationsPerInvoke = 32)]
	public int ParseThingsFromPreparedText()
	{
		int totalThingCount = 0;
		for (int index = 0; index < 32; index++)
		{
			totalThingCount += ParseUdmfFromPreparedTextCore(_thingOnlyUdmfText).Things.Length;
		}

		return totalThingCount;
	}

	private static MapData ParseUdmfFromPreparedTextCore(string text) =>
		new UdmfParser(new DirectLexer(new StringReader(text))).Parse();

	private static int GetMapItemCount(MapData map) =>
		map.Things.Length + map.Vertices.Length + map.LineDefs.Length + map.SideDefs.Length + map.Sectors.Length;

	private static string CreateTopLevelSubset(string text, string targetBlockName)
	{
		var subset = new StringBuilder(text.Length / 4);
		bool copiedNamespace = false;
		int index = 0;

		while (true)
		{
			SkipWhitespaceAndComments(text, ref index);
			if (index >= text.Length)
			{
				break;
			}

			int start = index;
			string identifier = ReadIdentifier(text, ref index);

			SkipWhitespaceAndComments(text, ref index);
			if (index >= text.Length)
			{
				throw new InvalidOperationException("Unexpected end of UDMF text while reading top-level item.");
			}

			if (text[index] == '=')
			{
				index++;
				SkipTopLevelAssignment(text, ref index);
				bool shouldCopy =
					!copiedNamespace && string.Equals(identifier, "namespace", StringComparison.OrdinalIgnoreCase);
				if (shouldCopy)
				{
					AppendTopLevelItem(subset, text.AsSpan(start, index - start));
				}

				copiedNamespace |= shouldCopy;
				continue;
			}

			if (text[index] == '{')
			{
				index++;
				SkipTopLevelBlock(text, ref index);
				if (string.Equals(identifier, targetBlockName, StringComparison.OrdinalIgnoreCase))
				{
					AppendTopLevelItem(subset, text.AsSpan(start, index - start));
				}

				continue;
			}

			throw new InvalidOperationException("Unexpected top-level UDMF text sequence.");
		}

		if (!copiedNamespace)
		{
			throw new InvalidOperationException("Could not find required namespace assignment in UDMF text.");
		}

		return subset.ToString();
	}

	private static void AppendTopLevelItem(StringBuilder builder, ReadOnlySpan<char> item)
	{
		if (builder.Length > 0)
		{
			builder.AppendLine();
		}

		builder.Append(item);
	}

	private static void SkipWhitespaceAndComments(string text, ref int index)
	{
		while (index < text.Length)
		{
			char current = text[index];
			switch (current)
			{
				case ' ':
				case '\t':
				case '\r':
				case '\f':
				case '\v':
				case '\n':
					index++;
					continue;
				case '/':
					SkipComment(text, ref index);
					continue;
				default:
					return;
			}
		}
	}

	private static void SkipTopLevelAssignment(string text, ref int index)
	{
		while (index < text.Length)
		{
			if (text[index] == '"')
			{
				SkipString(text, ref index);
				continue;
			}

			if (StartsWith(text, index, "//") || StartsWith(text, index, "/*"))
			{
				SkipComment(text, ref index);
				continue;
			}

			if (text[index] == ';')
			{
				index++;
				return;
			}

			index++;
		}

		throw new InvalidOperationException("Unexpected end of UDMF text while reading assignment.");
	}

	private static void SkipTopLevelBlock(string text, ref int index)
	{
		int depth = 1;
		while (index < text.Length)
		{
			if (text[index] == '"')
			{
				SkipString(text, ref index);
				continue;
			}

			if (StartsWith(text, index, "//") || StartsWith(text, index, "/*"))
			{
				SkipComment(text, ref index);
				continue;
			}

			if (text[index] == '{')
			{
				depth++;
				index++;
				continue;
			}

			if (text[index] == '}')
			{
				depth--;
				index++;
				if (depth == 0)
				{
					return;
				}

				continue;
			}

			index++;
		}

		throw new InvalidOperationException("Unexpected end of UDMF text while reading block.");
	}

	private static string ReadIdentifier(string text, ref int index)
	{
		if (index >= text.Length || !IsIdentifierStart(text[index]))
		{
			throw new InvalidOperationException("Expected identifier in UDMF text.");
		}

		int start = index;
		index++;
		while (index < text.Length && IsIdentifierPart(text[index]))
		{
			index++;
		}

		return text[start..index];
	}

	private static void SkipString(string text, ref int index)
	{
		index++;
		while (index < text.Length && text[index] != '"')
		{
			index++;
		}

		if (index >= text.Length)
		{
			throw new InvalidOperationException("Unterminated string in UDMF text.");
		}

		index++;
	}

	private static void SkipComment(string text, ref int index)
	{
		if (StartsWith(text, index, "//"))
		{
			index += 2;
			while (index < text.Length && text[index] != '\n')
			{
				index++;
			}

			return;
		}

		if (StartsWith(text, index, "/*"))
		{
			index += 2;
			while (index + 1 < text.Length && !StartsWith(text, index, "*/"))
			{
				index++;
			}

			if (index + 1 >= text.Length)
			{
				throw new InvalidOperationException("Unterminated block comment in UDMF text.");
			}

			index += 2;
			return;
		}

		throw new InvalidOperationException("Malformed comment in UDMF text.");
	}

	private static bool StartsWith(string text, int index, string value) =>
		index + value.Length <= text.Length && text.AsSpan(index, value.Length).SequenceEqual(value);

	private static bool IsIdentifierStart(char value) => char.IsAsciiLetter(value) || value == '_';

	private static bool IsIdentifierPart(char value) => IsIdentifierStart(value) || char.IsAsciiDigit(value);

	private MemoryStream CreateStream() => new(_udmfBytes, writable: false);

	private static string FindProjectDirectory()
	{
		for (
			DirectoryInfo? directory = new(AppContext.BaseDirectory);
			directory is not null;
			directory = directory.Parent
		)
		{
			if (File.Exists(Path.Combine(directory.FullName, "Benchmarks.csproj")))
			{
				return directory.FullName;
			}
		}

		throw new InvalidOperationException("Could not locate the Benchmarks project directory.");
	}
}
