using Tiledriver.DataModelGenerator.DoomGameInfo;
using Tiledriver.DataModelGenerator.MapInfo;
using Tiledriver.DataModelGenerator.Udmf;
using Tiledriver.DataModelGenerator.Uwmf;
using Tiledriver.DataModelGenerator.Xlat;

namespace Tiledriver.DataModelGenerator;

sealed class Program
{
	static void Main()
	{
		var basePath = FindSolutionPath();

		var corePath = Path.Combine(basePath, "Tiledriver");
		var formatModelsPath = Path.Combine(corePath, "FormatModels");

		var udmfPath = Path.Combine(formatModelsPath, "Udmf");
		var udmfWritingPath = Path.Combine(udmfPath, "Writing");
		var udmfReadingPath = Path.Combine(udmfPath, "Reading");

		var uwmfPath = Path.Combine(formatModelsPath, "Uwmf");
		var uwmfWritingPath = Path.Combine(uwmfPath, "Writing");
		var uwmfReadingPath = Path.Combine(uwmfPath, "Reading");

		var xlatPath = Path.Combine(formatModelsPath, "Xlat");
		var xlatReadingPath = Path.Combine(xlatPath, "Reading");

		var mapInfoPath = Path.Combine(formatModelsPath, "MapInfo");
		var mapInfoReadingPath = Path.Combine(mapInfoPath, "Reading");

		var gameInfoPath = Path.Combine(corePath, "GameInfo");
		var doomGameInfoPath = Path.Combine(gameInfoPath, "Doom");

		UwmfModelGenerator.WriteToPath(uwmfPath);
		UwmfWriterGenerator.WriteToPath(uwmfWritingPath);
		UwmfSemanticAnalyzerGenerator.WriteToPath(uwmfReadingPath);

		UdmfModelGenerator.WriteToPath(udmfPath);
		UdmfWriterGenerator.WriteToPath(udmfWritingPath);
		UdmfSemanticAnalyzerGenerator.WriteToPath(udmfReadingPath);

		XlatModelGenerator.WriteToPath(xlatPath);
		XlatParserGenerator.WriteToPath(xlatReadingPath);

		MapInfoModelGenerator.WriteToPath(mapInfoPath);
		MapInfoParserGenerator.WriteToPath(mapInfoReadingPath);

		DoomActorGenerator.WriteToPath(doomGameInfoPath);
	}

	private static string FindSolutionPath()
	{
		const string solutionFileName = "Tiledriver.slnx";

		for (
			var directory = new DirectoryInfo(AppContext.BaseDirectory);
			directory is not null;
			directory = directory.Parent
		)
		{
			if (File.Exists(Path.Combine(directory.FullName, solutionFileName)))
			{
				return directory.FullName;
			}
		}

		throw new DirectoryNotFoundException(
			$"Could not find {solutionFileName} by walking parent directories from '{AppContext.BaseDirectory}'."
		);
	}
}
