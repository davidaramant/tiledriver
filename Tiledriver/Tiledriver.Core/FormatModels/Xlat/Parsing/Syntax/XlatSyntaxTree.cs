// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax
{
    public sealed class XlatSyntaxTree
    {
        public IEnumerable<IExpression> GlobalExpressions { get; }
        public IEnumerable<Block> Blocks { get; }

        public XlatSyntaxTree(IEnumerable<IExpression> globalExpressions, IEnumerable<Block> blocks)
        {
            GlobalExpressions = globalExpressions;
            Blocks = blocks;
        }
    }
}