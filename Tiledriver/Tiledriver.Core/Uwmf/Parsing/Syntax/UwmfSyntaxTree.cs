// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.Uwmf.Parsing.Syntax
{
    public sealed class UwmfSyntaxTree
    {
        public IEnumerable<Assignment> GlobalAssignments { get; }
        public IEnumerable<Block> Blocks { get; }
        public IEnumerable<ArrayBlock> ArrayBlocks { get; }

        public UwmfSyntaxTree(
            IEnumerable<Assignment> globalAssignments,
            IEnumerable<Block> blocks,
            IEnumerable<ArrayBlock> arrayBlocks)
        {
            Blocks = blocks;
            ArrayBlocks = arrayBlocks;
            GlobalAssignments = globalAssignments;
        }
    }
}