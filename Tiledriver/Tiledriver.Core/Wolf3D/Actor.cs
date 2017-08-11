// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Wolf3D
{
    public sealed partial class Actor
    {
        public string ClassName => ((this == Player1Start) ? "$" : "") + _instanceName;
    }
}
