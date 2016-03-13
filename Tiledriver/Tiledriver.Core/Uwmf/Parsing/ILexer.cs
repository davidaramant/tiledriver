// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Uwmf.Parsing
{
    public enum BlockExpression
    {
        EndBlock,
        Identifier
    }

    public interface ILexer
    {
        string ReadIdentifier();

        int ReadIntAssignment();
        double ReadDoubleAssignment();
        bool ReadBoolAssignment();
        string ReadStringAssignment();

        void VerifyStartOfBlock();

        Tuple<BlockExpression, string> ReadEndBlockOrIdentifier();

        void SkipAssignment();
    }
}