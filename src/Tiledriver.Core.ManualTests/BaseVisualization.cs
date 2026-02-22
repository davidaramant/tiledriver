using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

public abstract class BaseVisualization(string prefix)
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory(prefix);

	protected void SaveImage(IFastImage image, string description) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

	protected void DeleteImages(string prefix)
	{
		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}
	}
}
