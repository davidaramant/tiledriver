// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Uwmf.Parsing
{
    public enum TokenTypeOld
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

    public interface ILexerOld
    {
        Identifier ReadIdentifier();

        int ReadIntegerNumber();
        double ReadFloatingPointNumber();
        bool ReadBoolean();
        string ReadString();

        void AdvanceOneCharacter();

        TokenTypeOld DetermineNextToken();

        void MovePastAssignment();
    }
}