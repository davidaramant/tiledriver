using System.Reflection;

namespace Tiledriver.DataModelGenerator.Utilities;

static class CurrentLibraryInfo
{
	private static readonly Lazy<(string Name, Version Version)> Info = new(() =>
	{
		var currentAssembly =
			Assembly.GetAssembly(typeof(CurrentLibraryInfo)) ?? throw new Exception("Can't find this assembly?!");

		var assemblyName = currentAssembly.GetName();

		return (
			Name: assemblyName.Name ?? throw new Exception("Could not look up name of current assembly"),
			Version: assemblyName.Version ?? throw new Exception("Could not look up version of current assembly")
		);
	});

	public static string Name => Info.Value.Name;
	public static Version Version => Info.Value.Version;
}
