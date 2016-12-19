// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed class InvalidXlatException : Exception
    {
        public InvalidXlatException(string message) : base(message)
        {
            
        }
    }
}