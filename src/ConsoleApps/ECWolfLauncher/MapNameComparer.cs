using System;
using System.Collections.Generic;
using System.IO;

namespace TestRunner
{
    sealed class MapNameComparer : Comparer<string>
    {
        [Flags]
        public enum Type
        {
            Wolf3D,
            Mirrored = 1 << 0,
            Rotated90 = 1 << 1,
            Rotated180 = 1 << 2,
            Rotated270 = 1 << 3,
            Custom = 1 << 4,
        }

        public static Type DetermineType(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            var type = fileName.Contains("Wolf3D") ? Type.Wolf3D : Type.Custom;

            if (fileName.EndsWith(" m"))
            {
                type |= Type.Mirrored;
            }
            else if (fileName.EndsWith(" r1"))
            {
                type |= Type.Rotated90;
            }
            else if (fileName.EndsWith(" r1m"))
            {
                type |= Type.Mirrored;
                type |= Type.Rotated90;
            }
            else if (fileName.EndsWith(" r2"))
            {
                type |= Type.Rotated180;
            }
            else if (fileName.EndsWith(" r2m"))
            {
                type |= Type.Mirrored;
                type |= Type.Rotated180;
            }
            else if (fileName.EndsWith(" r3"))
            {
                type |= Type.Rotated270;
            }
            else if (fileName.EndsWith(" r3m"))
            {
                type |= Type.Mirrored;
                type |= Type.Rotated270;
            }

            return type;
        }

        public override int Compare(string x, string y)
        {
            var xType = DetermineType(x);
            var yType = DetermineType(y);

            return xType.CompareTo(yType);
        }
    }
}