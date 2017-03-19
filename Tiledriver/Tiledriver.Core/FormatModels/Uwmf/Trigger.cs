// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public sealed partial class Trigger
    {
        public void SetActivation(Direction direction, bool enabled)
        {
            switch (direction)
            {
                case Direction.East:
                    ActivateEast = enabled;
                    break;
                case Direction.North:
                    ActivateNorth = enabled;
                    break;
                case Direction.South:
                    ActivateSouth = enabled;
                    break;
                case Direction.West:
                    ActivateWest = enabled;
                    break;
            }
        }
    }
}