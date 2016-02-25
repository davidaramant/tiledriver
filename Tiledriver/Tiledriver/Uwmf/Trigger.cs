using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Trigger : IUwmfEntry
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public bool ActivateNorth { get; set; } = true;
        public bool ActivateSouth { get; set; } = true;
        public bool ActivateWest { get; set; } = true;
        public bool ActivateEast { get; set; } = true;

        public int Action { get; set; }

        public int Arg0 { get; set; }
        public int Arg1 { get; set; }
        public int Arg2 { get; set; }
        public int Arg3 { get; set; }
        public int Arg4 { get; set; }

        public bool PlayerUse { get; set; }
        public bool MonsterUse { get; set; }
        public bool PlayerCross { get; set; }
        public bool Repeatable { get; set; }

        public bool Secret { get; set; }

        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("trigger").
                Line("{").
                Attribute("x", X).
                Attribute("y", Y).
                Attribute("z", Z).
                Attribute("activatenorth", ActivateNorth).
                Attribute("activatesouth", ActivateSouth).
                Attribute("activatewest", ActivateWest).
                Attribute("activateeast", ActivateEast).
                Attribute("action", Action).
                Attribute("arg0", Arg0).
                Attribute("arg1", Arg1).
                Attribute("arg2", Arg2).
                Attribute("arg3", Arg3).
                Attribute("arg4", Arg4).
                Attribute("playeruse", PlayerUse).
                Attribute("monsteruse", MonsterUse).
                Attribute("playercross", PlayerCross).
                Attribute("repeatable", Repeatable).
                Attribute("secret", Secret).
                Line("}");
        }
    }
}
