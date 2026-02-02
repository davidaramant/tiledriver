// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading;

public static class UwmfParser
{
	public static IEnumerable<IExpression> Parse(IEnumerable<Token> tokens)
	{
		using var tokenStream = tokens.GetEnumerator();
		while (tokenStream.MoveNext())
		{
			if (tokenStream.Current is IdentifierToken i)
			{
				switch (tokenStream.GetNext())
				{
					case OpenBraceToken _:
						yield return ParseBlockOrTupleList(i, tokenStream);
						break;
					case EqualsToken _:
						yield return tokenStream.ParseAssignment(i);
						break;
					default:
						throw ParsingException.CreateError(tokenStream.Current, "open brace or equals");
				}
			}
			else
			{
				throw ParsingException.CreateError<IdentifierToken>(tokenStream.Current);
			}
		}
	}

	private static IExpression ParseBlockOrTupleList(IdentifierToken name, IEnumerator<Token> tokenStream)
	{
		var token = tokenStream.GetNext();
		return token switch
		{
			OpenBraceToken ob => ParseIntTupleList(name, ob.Location, tokenStream),
			CloseBraceToken _ => new Block(name, ImmutableArray<Assignment>.Empty),
			IdentifierToken i => ParseBlock(name, i, tokenStream),
			_ => throw ParsingException.CreateError(token, "identifier, end of block, or start of tuple"),
		};
	}

	private static Block ParseBlock(IdentifierToken name, IdentifierToken fieldName, IEnumerator<Token> tokenStream)
	{
		var assignments = new List<Assignment>();

		tokenStream.ExpectNext<EqualsToken>();
		assignments.Add(tokenStream.ParseAssignment(fieldName));

		while (true)
		{
			var token = tokenStream.GetNext();
			switch (token)
			{
				case IdentifierToken i:
					tokenStream.ExpectNext<EqualsToken>();
					assignments.Add(tokenStream.ParseAssignment(i));
					break;
				case CloseBraceToken:
					return new Block(name, [.. assignments]);
				default:
					throw ParsingException.CreateError(token, "identifier or end of block");
			}
		}
	}

	private static IntTupleBlock ParseIntTupleList(
		IdentifierToken name,
		FilePosition startLocation,
		IEnumerator<Token> tokenStream
	)
	{
		var tuples = ImmutableArray.CreateBuilder<IntTuple>();
		tuples.Add(ParseIntTuple(startLocation, tokenStream));

		while (true)
		{
			var token = tokenStream.GetNext();
			switch (token)
			{
				case CommaToken _:
					var openBrace = tokenStream.ExpectNext<OpenBraceToken>();
					tuples.Add(ParseIntTuple(openBrace.Location, tokenStream));
					break;

				case CloseBraceToken _:
					return new IntTupleBlock(name, tuples.ToImmutable());

				default:
					throw ParsingException.CreateError(token, "comma or end of block");
			}
		}
	}

	private static IntTuple ParseIntTuple(FilePosition startLocation, IEnumerator<Token> tokenStream)
	{
		var ints = ImmutableArray.CreateBuilder<IntegerToken>();

		var token = tokenStream.GetNext();
		switch (token)
		{
			case CloseBraceToken _:
				return new IntTuple(startLocation, ints.ToImmutable());
			case IntegerToken i:
				ints.Add(i);
				break;
			default:
				throw ParsingException.CreateError(token, "integer or end of tuple");
		}

		while (true)
		{
			token = tokenStream.GetNext();
			switch (token)
			{
				case CloseBraceToken _:
					return new IntTuple(startLocation, ints.ToImmutable());

				case CommaToken _:
					token = tokenStream.GetNext();
					if (token is IntegerToken i)
					{
						ints.Add(i);
					}
					else
					{
						throw ParsingException.CreateError(token, "integer");
					}

					break;

				default:
					throw ParsingException.CreateError(token, "Comma or end of tuple");
			}
		}
	}
}
