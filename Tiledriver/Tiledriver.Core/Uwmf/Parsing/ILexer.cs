﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using Tiledriver.Core.Uwmf.Metadata;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public enum TokenType
    {
        Identifier,
        Assignment,
        EndOfAssignment,
        StartBlock,
        EndBlock,
        Comma,
        EndOfFile,
        Unknown
    }

    public interface ILexer
    {
        Identifier ReadIdentifier();

        int ReadIntegerNumber();
        double ReadFloatingPointNumber();
        bool ReadBoolean();
        string ReadString();

        void AdvanceOneCharacter();

        TokenType DetermineNextToken();

        void MovePastAssignment();
    }
}