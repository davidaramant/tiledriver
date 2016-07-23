// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

namespace Tiledriver.Core.Wolf3D
{
    public sealed partial class Actor
    {
        public string ClassName => Prefix + _instanceName;
    }
}
