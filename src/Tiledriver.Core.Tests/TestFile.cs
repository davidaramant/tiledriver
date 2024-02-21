// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Tiledriver.Core.Tests
{
    /// <summary>
    /// Accessor for embedded test files.
    /// </summary>
    /// <remarks>
    /// As an experiment, lets see how well having all test files be embedded into the tests work out. There seem to be
    /// issues with copying files to the test output directory which should be solved with this approach.
    /// </remarks>
    public static class TestFile
    {
        // The property names intentionally mirror the file names
#pragma warning disable IDE1006 // Naming Styles
        public static class MapInfo
        {
            public static Stream spear => Open(nameof(MapInfo));
            public static Stream wolf3d => Open(nameof(MapInfo));
            public static Stream wolfcommon => Open(nameof(MapInfo));
        }

        public static class Uwmf
        {
            public static Stream TEXTMAP => Open(nameof(Uwmf));
        }

        public static class Xlat
        {
            public static Stream spear => Open(nameof(Xlat));
            public static Stream wolf3d => Open(nameof(Xlat));
        }
#pragma warning restore IDE1006 // Naming Styles

        private static Stream Open(string area, [CallerMemberName] string? fileName = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            string resourcePath = $"Tiledriver.Core.Tests.TestFiles.{area}.{fileName}.txt";

            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath)
                ?? throw new Exception("Could not find test file");
        }
    }
}
