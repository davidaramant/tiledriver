// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Common;

public sealed class ParsingException : Exception
{
	public ParsingException(string message)
		: base(message) { }

	public static ParsingException CreateError(Token? token, string expected)
	{
		if (token == null)
		{
			return new ParsingException("Unexpected end of file");
		}
		return new ParsingException(
			$"Unexpected token {token.GetType().Name} (expected {expected}) on {token.Location}"
		);
	}

	public static ParsingException CreateError<TExpected>(Token? token) => CreateError(token, typeof(TExpected).Name);
}
