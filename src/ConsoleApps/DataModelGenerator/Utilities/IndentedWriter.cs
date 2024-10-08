// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.Utilities;

public sealed class IndentedWriter : IDisposable
{
	private readonly StreamWriter _writer;

	public IndentedWriter(StreamWriter writer) => _writer = writer;

	public int IndentionLevel { get; private set; }
	public string CurrentIndent => new('\t', IndentionLevel);

	public IndentedWriter WriteHeader(
		string nameSpace,
		IEnumerable<string> usingNamespaces,
		bool enableNullables = false
	)
	{
		Line(
			$@"// Copyright (c) {DateTime.Today.Year}, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE."
		);
		Line();

		foreach (var usingNamespace in usingNamespaces.OrderBy(text => text))
		{
			Line($"using {usingNamespace};");
		}

		Line();

		if (enableNullables)
		{
			Line("#nullable enable");
		}

		Line($"namespace {nameSpace};");

		return this;
	}

	public IndentedWriter IncreaseIndent()
	{
		IndentionLevel++;
		return this;
	}

	public IndentedWriter DecreaseIndent()
	{
		if (IndentionLevel == 0)
			throw new InvalidOperationException();
		IndentionLevel--;
		return this;
	}

	public IndentedWriter OpenParen() => Line("{").IncreaseIndent();

	public IndentedWriter CloseParen(bool withSemicolon = false) =>
		DecreaseIndent().Line("}" + (withSemicolon ? ";" : string.Empty));

	public IndentedWriter Line(string line)
	{
		_writer.WriteLine(CurrentIndent + line);
		return this;
	}

	public IndentedWriter Line()
	{
		_writer.WriteLine();
		return this;
	}

	public IndentedWriter Lines(IEnumerable<string> lines)
	{
		foreach (var line in lines)
		{
			Line(line);
		}

		return this;
	}

	public IndentedWriter JoinLines(string linePostfix, IEnumerable<string> lines)
	{
		using var enumerator = lines.GetEnumerator();

		string? actualLine = null;

		if (enumerator.MoveNext())
		{
			actualLine = enumerator.Current;
		}

		while (enumerator.MoveNext())
		{
			Line(actualLine + linePostfix);
			actualLine = enumerator.Current;
		}

		if (actualLine != null)
		{
			Line(actualLine);
		}

		return this;
	}

	public void Dispose()
	{
		if (IndentionLevel != 0)
		{
			throw new InvalidOperationException("Indention level is screwed up.");
		}
	}
}
