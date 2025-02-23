using NUnit.Framework;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Udmf.Reading;
using Tiledriver.Core.FormatModels.Wad;

// ReSharper disable LocalizableElement

namespace Tiledriver.Core.ManualTests;

public sealed class ChexHacks
{
	private const string ProjectPath = @"C:\src\Personal\sep-doom-presentation\sep";
	private const string ChexProjectPath = @"C:\Users\dbaramant\Downloads\chex3gzd-rc5\chex3gzd-rc5";

	private static readonly IReadOnlyDictionary<int, string> ThingLookup = new Dictionary<int, string>
	{
		{7, "Flembomination"},
		{16, "Snotfolus"},
		{18, "PropModelRocket"},
		{19, "PropRadarDish"},
		{26, "PropCaptive2"},
		{27, "PropSlimeyMeteor"},
		{29, "PropMonitor"},
		{36, "PropOxygenTank"},
		{38, "RedFlemKey"},
		{39, "YellowFlemKey"},
		{40, "BlueFlemKey"},
		{42, "LabCoil"},
		{46, "PropRedTorch"},
		{49, "PropStool"},
		{50, "PropHydroponicPlant"},
		{51, "PropBigBowl"},
		{52, "PropCaptive3"},
		{53, "PropBazoikCart"},
		{58, "FlemoidusCycloptisCommonusV3"},
		{59, "PropHangingPlant1"},
		{60, "PropCeilingSlime"},
		{61, "PropHangingPlant2"},
		{62, "PropHangingPots"},
		{63, "PropCaveBat"},
		{69, "FlembraneV3"},
		{70, "PropCaptive1"},
		{73, "PropPillar"},
		{74, "PropStalagmite"},
		{75, "PropStalagtite2"},
		{76, "PropDinosaur1"},
		{77, "PropDinosaur2"},
		{78, "PropFlower1"},
		{79, "PropFlower2"},
		{80, "PropBeaker"},
		{81, "PropSmallBush"},
		{85, "MapPointLight"},
		{86, "PropSlimeyUrn"},
		{2035, "FlemoidPowerStrand"},
		{2045, "UltraGoggles"},
		{3005, "SuperCycloptis"},
		{5400, "BoomWindow"},
		{6052, "ChexTeleGlitterGenerator"},
		{9050, "Larva"},
		{9051, "StatueDavid"},
		{9052, "StatueThinker"},
		{9053, "StatueRamses"},
		{9054, "StatueKingTut"},
		{9055, "StatueChexWarrior"},
		{9056, "StatueSpoon"},
		{9057, "Quadrumpus"},
		{9058, "TreeBanana"},
		{9059, "TreeBeech"},
		{9060, "TreeApple"},
		{9061, "TreeOrange"},
	};

	[Test, Explicit]
	public void GetLevelStats()
	{
		var wad = WadFile.Read(Path.Combine(ProjectPath, "maps", "CHEX.wad"));
		var mapData = UdmfReader.Read(new MemoryStream(wad.Single(l => l.Name == "TEXTMAP").GetData()));
		var usedTextures = mapData
			.SideDefs.SelectMany(sd => new[] {sd.TextureMiddle, sd.TextureBottom, sd.TextureTop})
			.Where(tex => tex != Texture.None)
			.Select(tex => tex.Name)
			.ToHashSet();

		var usedThings = mapData.Things.Select(t => t.Type).ToHashSet();

		Console.Out.WriteLine($"{usedTextures.Count} textures");

		foreach (var tex in usedTextures.Order())
		{
			Console.WriteLine($" * {tex}");
		}

		Console.Out.WriteLine();
		Console.Out.WriteLine($"{usedThings.Count} things");
		foreach (var thingId in usedThings.Order())
		{
			var description = ThingLookup.GetValueOrDefault(thingId, "Doom thing");
			Console.WriteLine($" * {thingId}: {description}");
		}

		// TODOs for conversion:
		// - Get list of things required in level (not sure what is doing on with the TIDs...)
		//	- Decorations
		//  - Enemies
		//  - Weapons
		// - For each texture used:
		//  - Is it a Doom replacement? UGH - need renames like KICK did
		//  - If it's brand new:
		//		- Get patches needed
		//		- Texture definition
		// - Sky definition

	}
}
