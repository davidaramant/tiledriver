﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

namespace Tiledriver.Core.Uwmf.Parsing
{
    public enum ExpressionType
    {
        StartBlock,
        EndBlock,
        Identifier,
        Assignment
    }

    public interface ILexer
    {
        Identifier ReadIdentifier();

        int ReadIntAssignment();
        double ReadDoubleAssignment();
        bool ReadBoolAssignment();
        string ReadStringAssignment();

        void VerifyStartOfBlock();

        ExpressionType DetermineIfAssignmentOrStartBlock();
        ExpressionType DetermineIfIdentifierOrEndBlock();

        void MovePastAssignment();
    }
}