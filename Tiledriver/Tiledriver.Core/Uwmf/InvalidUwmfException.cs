// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Uwmf
{
    public sealed class InvalidUwmfException : Exception
    {
        public InvalidUwmfException(string message) : base(message)
        {
            
        }
    }
}