using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Common.Reading;

public static class TokenStreamExtensions
{
	extension(IEnumerator<Token> enumerator)
	{
		public Token? GetNext() => enumerator.MoveNext() ? enumerator.Current : null;

		public Token? GetNextSkippingNewlines()
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is not NewLineToken)
				{
					return enumerator.Current;
				}
			}

			return null;
		}

		public TExpected ExpectNext<TExpected>()
			where TExpected : Token
		{
			var nextToken = enumerator.GetNext();
			return nextToken as TExpected ?? throw ParsingException.CreateError<TExpected>(nextToken);
		}

		public TExpected ExpectNextSkippingNewlines<TExpected>()
			where TExpected : Token
		{
			var nextToken = enumerator.GetNextSkippingNewlines();
			return nextToken as TExpected ?? throw ParsingException.CreateError<TExpected>(nextToken);
		}

		public Block ParseBlock(IdentifierToken name)
		{
			var assignments = new List<Assignment>();

			while (true)
			{
				var token = enumerator.GetNext();
				switch (token)
				{
					case IdentifierToken i:
						enumerator.ExpectNext<EqualsToken>();
						assignments.Add(enumerator.ParseAssignment(i));
						break;
					case CloseBraceToken:
						return new Block(name, [.. assignments]);
					default:
						throw ParsingException.CreateError(token, "identifier or end of block");
				}
			}
		}

		public Assignment ParseAssignment(IdentifierToken id)
		{
			var valueToken = enumerator.GetNext();
			switch (valueToken)
			{
				case IntegerToken:
					break;
				case FloatToken:
					break;
				case BooleanToken:
					break;
				case StringToken:
					break;
				default:
					throw ParsingException.CreateError(valueToken, "value");
			}

			enumerator.ExpectNext<SemicolonToken>();

			return new Assignment(id, valueToken);
		}
	}
}
