// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Reflection;

namespace Tiledriver.DataModelGenerator.Utilities
{
	static class CurrentLibraryInfo
	{
		private static readonly Lazy<(string Name, Version Version)> _info =
			new(() =>
			{
				var currentAssembly =
					Assembly.GetAssembly(typeof(CurrentLibraryInfo))
					?? throw new Exception("Can't find this assembly?!");

				var assemblyName = currentAssembly.GetName();

				return (
					Name: assemblyName.Name ?? throw new Exception("Could not look up name of current assembly"),
					Version: assemblyName.Version
						?? throw new Exception("Could not look up version of current assembly")
				);
			});

		public static string Name => _info.Value.Name;
		public static Version Version => _info.Value.Version;
	}
}
