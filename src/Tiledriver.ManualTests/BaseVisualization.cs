using Tiledriver.Rendering;

namespace Tiledriver.ManualTests;

public abstract class BaseVisualization(string prefix)
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory(prefix);

	protected void SaveImage(IPixelBuffer image, string description, int scale = 1) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"), scale: scale);

	protected void DeleteImages(string prefix)
	{
		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}
	}
}
