﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public sealed class ParsingException : Exception
    {
        public ParsingException(string message) : base(message)
        {            
        }

        public ParsingException(CharPosition position, string message) : base($"{position}: {message}")
        {            
        }
    }
}