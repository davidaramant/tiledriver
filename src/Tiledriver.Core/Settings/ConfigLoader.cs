using System.Runtime.InteropServices;
using System.Text.Json;

namespace Tiledriver.Core.Settings;

public static class ConfigLoader
{
	public static string ConfigPath =>
		Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"Tiledriver",
			"TiledriverConfig.json"
		);

	public static TiledriverConfig Load()
	{
		var config = Deserialize<TiledriverConfig>(ConfigPath);

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			if (!config.ECWolfPath.EndsWith("ecwolf.exe", StringComparison.InvariantCultureIgnoreCase))
			{
				config = config with { ECWolfPath = Path.Combine(config.ECWolfPath, "ecwolf.exe") };
			}

			if (!config.UZDoomPath.EndsWith("uzdoom.exe", StringComparison.InvariantCultureIgnoreCase))
			{
				config = config with { UZDoomPath = Path.Combine(config.UZDoomPath, "uzdoom.exe") };
			}
		}

		if (!config.GamePaths.Complete)
		{
			var steamPaths = SteamGameSearcher.GetGamePaths();
			var newGamePaths = config.GamePaths.MergeWith(steamPaths);
			config = config with { GamePaths = newGamePaths };
		}

		System.Console.Out.WriteLine(config);

		return config;
	}

	static T Deserialize<T>(string filePath)
	{
		if (!File.Exists(filePath))
		{
			throw new Exception($"File does not exist: {filePath}");
		}

		// TODO: Is there seriously no better way to read from a file?
		var serialized = File.ReadAllText(filePath);

		return JsonSerializer.Deserialize<T>(
				serialized,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? throw new ArgumentException($"Bad format for {Path.GetFileName(filePath)}");
	}
}
