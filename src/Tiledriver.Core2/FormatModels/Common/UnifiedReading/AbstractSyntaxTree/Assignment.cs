﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Diagnostics;

namespace Tiledriver.Core.FormatModels.Common.UnifiedReading.AbstractSyntaxTree
{
    [DebuggerDisplay("{ToString()}")]
    public sealed record Assignment(IdentifierToken Name, Token Value) : IExpression
    {
        public string ValueAsString() => Value switch
        {
            IntegerToken i => i.Value.ToString(),
            FloatToken f => ToStringWithDecimal(f.Value),
            BooleanToken b => b.Value.ToString().ToLowerInvariant(),
            StringToken s => '"' + s.Value + '"',
            _ => Value.ToString()
        };

        public override string ToString() => $"{Name.Id}: {ValueAsString()} ({Value.GetType()})";

        private static string ToStringWithDecimal(double source) => (source % 1) == 0 ? source.ToString("f1") : source.ToString();
    }
}