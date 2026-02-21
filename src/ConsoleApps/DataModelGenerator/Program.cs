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

		var corePath = Path.Combine(basePath, "Tiledriver.Core");
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
		var path = "..";

		while (!File.Exists(Path.Combine(path, "Tiledriver.slnx")))
		{
			path = Path.Combine(path, "..");
		}

		return Path.GetFullPath(path);
	}
}
