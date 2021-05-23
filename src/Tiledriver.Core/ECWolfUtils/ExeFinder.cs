// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using System;
using System.IO;
using System.Linq;

namespace Tiledriver.Core.ECWolfUtils
{
    public static class ExeFinder
    {
        private const string ECWolfPathConfigurationFile = "ECWolfPath.txt";
        
        private static readonly Lazy<string> ExePath = new(() =>
        {
            var configPath = ECWolfPathConfigurationFile;
            if (!File.Exists(ECWolfPathConfigurationFile))
            {
                configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    ECWolfPathConfigurationFile);
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException($"Could not find {ECWolfPathConfigurationFile}");
                }
            }

            var ecWolfPath =
                File.ReadAllLines(configPath)
                    .SingleOrDefault(l => !string.IsNullOrWhiteSpace(l))
                ?? throw new ArgumentException("Wrong format for file contents");

            if (!ecWolfPath.EndsWith("ecwolf.exe", StringComparison.InvariantCultureIgnoreCase))
            {
                ecWolfPath = Path.Combine(ecWolfPath, "ecwolf.exe");
            }

            return ecWolfPath;
        }
        );

        public static Launcher CreateLauncher() => new(GetECWolfExePath());

        public static string GetECWolfExePath() => ExePath.Value;
    }
}