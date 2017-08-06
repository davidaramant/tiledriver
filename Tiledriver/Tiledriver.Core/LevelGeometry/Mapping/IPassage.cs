// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public class Passage : IEquatable<Passage>
    {
        public MapLocation Location { get; }

        public Trigger Door => Location.Triggers.Single(t=>t.Action == "Door_Open");

        public Passage(MapLocation loc)
        {
            Location = loc;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Passage);
        }

        public bool Equals(Passage other)
        {
            if (null == other)
                return false;

            return Location.Equals(other.Location);
        }
    }
}