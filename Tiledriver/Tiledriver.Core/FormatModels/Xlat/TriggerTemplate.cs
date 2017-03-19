// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class TriggerTemplate
    {
        public bool ActivatesIn(Direction direction)
        {
            switch (direction)
            {
                case Direction.East:
                    return ActivateEast;
                case Direction.North:
                    return ActivateNorth;
                case Direction.South:
                    return ActivateSouth;
                case Direction.West:
                    return ActivateWest;
                default:
                    return false;
            }
        }

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

        public void SetAllActivations(bool enabled)
        {
            ActivateEast = ActivateNorth = ActivateWest = ActivateSouth = enabled;
        }
    }
}