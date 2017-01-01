// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax
{
    public sealed class Block : IEnumerable<Expression>
    {
        private readonly IEnumerable<Expression> _expressions;

        public Identifier Name { get; }

        public Block(Identifier name, IEnumerable<Expression> expressions)
        {
            Name = name;
            _expressions = expressions;
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}