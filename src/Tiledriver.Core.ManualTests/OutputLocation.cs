// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;

namespace Tiledriver.Core.ManualTests
{
    public static class OutputLocation
    {
        public static DirectoryInfo CreateDirectory(string folderName) =>
            Directory.CreateDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Tiledriver Visualizations",
                    folderName
                )
            );
    }
}
