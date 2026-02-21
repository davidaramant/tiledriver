namespace Tiledriver.Core.ManualTests;

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
