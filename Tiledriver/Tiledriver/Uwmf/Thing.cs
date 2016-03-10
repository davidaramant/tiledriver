﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Thing : IUwmfEntry
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        // 0 - 359; 90 is South
        public int Angle { get; set; }

        public int Type { get; set; }

        public bool Ambush { get; set; }
        public bool Patrol { get; set; }

        public bool Skill1 { get; set; }
        public bool Skill2 { get; set; }
        public bool Skill3 { get; set; }
        public bool Skill4 { get; set; }

        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("thing").
                Line("{").
                Attribute("x", X).
                Attribute("y", Y).
                Attribute("z", Z).
                Attribute("angle", Angle).
                Attribute("type", Type).
                Attribute("ambush", Ambush).
                Attribute("patrol", Patrol).
                Attribute("skill1", Skill1).
                Attribute("skill2", Skill2).
                Attribute("skill3", Skill3).
                Attribute("skill4", Skill4).
                Line("}");
        }
    }
}
