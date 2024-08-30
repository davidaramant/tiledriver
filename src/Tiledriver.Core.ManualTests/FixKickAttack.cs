// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Udmf.Reading;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.ManualTests;

public sealed partial class FixKickAttack
{
	[GeneratedRegex(@"WallTexture ""(\w+)"", (\d+), (\d+)")]
	private static partial Regex TextureDefinitionRegex();

	[Test, Explicit]
	public async Task CreateKickTextures()
	{
		var (kickTextures, patches) = GetUsedTexturesAndPatches();

		var newTextures = kickTextures.Select(t =>
			t with
			{
				Name = "KA_" + t.Name,
				Contents = t
					.Contents.Select(c =>
					{
						foreach (var p in patches)
						{
							c = c.Replace(p, "patches/kick/KA_" + p + ".png");
						}

						return c;
					})
					.ToArray(),
			}
		);

		await using var fs = File.Open(
			@"/Users/david/RiderProjects/sep-doom-presentation/sep/TEXTURES.kick",
			FileMode.Create
		);
		await using var writer = new StreamWriter(fs);
		foreach (var t in newTextures)
		{
			await writer.WriteLineAsync($"WallTexture \"{t.Name}\", {t.Width}, {t.Height}");
			await writer.WriteLineAsync("{");
			foreach (var c in t.Contents)
			{
				await writer.WriteLineAsync($"\t{c}");
			}

			await writer.WriteLineAsync("}");
			await writer.WriteLineAsync("");
		}
	}

	[Test, Explicit]
	public void ReplaceTexturesInLevel()
	{
		var (kickTextures, patches) = GetUsedTexturesAndPatches();

		var wad = WadFile.Read(@"/Users/david/RiderProjects/sep-doom-presentation/sep/maps/KICK.wad");

		var mapData = UdmfReader.Read(new MemoryStream(wad.Single(l => l.Name == "TEXTMAP").GetData()));
	}

	private static (IReadOnlyList<WallTexture>, IReadOnlySet<string> Patches) GetUsedTexturesAndPatches()
	{
		var path = @"/Users/david/RiderProjects/sep-doom-presentation/sep/patches/kick";
		var patches = Directory
			.GetFiles(path)
			.Select(f => Path.GetFileNameWithoutExtension(f)!)
			.Select(name => name[3..])
			.ToHashSet();

		var textures = new List<WallTexture>();

		var contents = new List<string>();
		var name = string.Empty;
		var width = 0;
		var height = 0;
		foreach (
			var line in Textures.Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0 && l != "{" && l != "}")
		)
		{
			if (line.StartsWith("WallTexture"))
			{
				if (contents.Count > 0)
				{
					textures.Add(new WallTexture(name, width, height, contents.ToArray()));
				}

				contents.Clear();

				var match = TextureDefinitionRegex().Match(line);

				name = match.Groups[1].Value;
				width = int.Parse(match.Groups[2].Value);
				height = int.Parse(match.Groups[3].Value);
			}
			else
			{
				contents.Add(line);
			}
		}

		textures.Add(new WallTexture(name, width, height, contents.ToArray()));

		var kickTextures = textures.Where(t => t.Contents.Any(c => patches.Any(c.Contains))).ToList();

		return (kickTextures, patches);
	}

	sealed record WallTexture(string Name, int Width, int Height, IReadOnlyList<string> Contents);

	private const string Textures = """
		WallTexture "AASHITTY", 64, 64
		{
			NullTexture
			Patch "BODIES", 0, 0
		}

		WallTexture "ASHWALL2", 64, 128
		{
			Patch "RW22_1", 0, 0
		}

		WallTexture "ASHWALL3", 64, 128
		{
			Patch "RW22_2", 0, 0
		}

		WallTexture "ASHWALL4", 64, 128
		{
			Patch "RW22_3", 0, 0
		}

		WallTexture "ASHWALL6", 64, 128
		{
			Patch "RW27_2", 0, 0
		}

		WallTexture "ASHWALL7", 64, 128
		{
			Patch "RW27_3", 0, 0
		}

		WallTexture "BFALL1", 64, 128
		{
			Patch "BFALL1", 0, 0
		}

		WallTexture "BFALL2", 64, 128
		{
			Patch "BFALL2", 0, 0
		}

		WallTexture "BFALL3", 64, 128
		{
			Patch "BFALL3", 0, 0
		}

		WallTexture "BFALL4", 64, 128
		{
			Patch "BFALL4", 0, 0
		}

		WallTexture "BIGBRIK1", 64, 128
		{
			Patch "RW23_3", 0, 0
		}

		WallTexture "BIGBRIK2", 64, 128
		{
			Patch "RW23_4", 0, 0
		}

		WallTexture "BIGBRIK3", 64, 128
		{
			Patch "RW38_4", 0, 0
		}

		WallTexture "BIGDOOR1", 128, 96
		{
			Patch "W13_1", 0, 0
			Patch "W13_1", 0, 24
			Patch "DOOR2_1", 17, 0
			Patch "W13_1", 113, 0
			Patch "W13_1", 113, 25
		}

		WallTexture "BIGDOOR2", 128, 128
		{
			Patch "DOOR2_4", 0, 0
		}

		WallTexture "BIGDOOR3", 128, 128
		{
			Patch "DOOR9_2", 0, 0
		}

		WallTexture "BIGDOOR4", 128, 128
		{
			Patch "DOOR9_1", 0, 0
		}

		WallTexture "BIGDOOR5", 128, 128
		{
			Patch "WALL40_1", 0, 0
			Patch "WALL42_3", 51, 0
			Patch "WALL42_3", 0, 0
			Patch "WALL42_3", 104, 0
		}

		WallTexture "BIGDOOR6", 128, 112
		{
			Patch "DOOR11_1", 4, 0
			Patch "DOOR11_1", 124, 0
			Patch "DOOR11_1", -116, 0
		}

		WallTexture "BIGDOOR7", 128, 128
		{
			Patch "W105_1", -5, 0
			Patch "W105_1", 123, 0
		}

		WallTexture "BLAKWAL1", 64, 128
		{
			Patch "RW34_1", 0, 0
		}

		WallTexture "BLAKWAL2", 64, 128
		{
			Patch "RW34_2", 0, 0
		}

		WallTexture "BLODRIP1", 32, 128
		{
			Patch "RP2_1", 0, 0
		}

		WallTexture "BLODRIP2", 32, 128
		{
			Patch "RP2_2", 0, 0
		}

		WallTexture "BLODRIP3", 32, 128
		{
			Patch "RP2_3", 0, 0
		}

		WallTexture "BLODRIP4", 32, 128
		{
			Patch "RP2_4", 0, 0
		}

		WallTexture "BRICK1", 64, 128
		{
			Patch "RW1_3", 0, 0
		}

		WallTexture "BRICK10", 64, 128
		{
			Patch "RW41_1", 0, 0
		}

		WallTexture "BRICK11", 64, 128
		{
			Patch "RW41_3", 0, 0
		}

		WallTexture "BRICK12", 64, 128
		{
			Patch "RW41_4", 0, 0
		}

		WallTexture "BRICK2", 64, 128
		{
			Patch "RW1_4", 0, 0
		}

		WallTexture "BRICK3", 64, 128
		{
			Patch "RW5_1", 0, 0
		}

		WallTexture "BRICK4", 64, 128
		{
			Patch "RW5_2", 0, 0
		}

		WallTexture "BRICK5", 128, 128
		{
			Patch "RW5_3", 0, 0
			Patch "RW5_4", 64, 0
		}

		WallTexture "BRICK6", 64, 128
		{
			Patch "RW24_1", 0, 0
		}

		WallTexture "BRICK7", 64, 128
		{
			Patch "RW24_2", 0, 0
		}

		WallTexture "BRICK8", 64, 128
		{
			Patch "RW24_3", 0, 0
		}

		WallTexture "BRICK9", 64, 128
		{
			Patch "RW24_4", 0, 0
		}

		WallTexture "BRICKLIT", 64, 128
		{
			Patch "RW12_3", 0, 0
		}

		WallTexture "BRNPOIS", 128, 128
		{
			Patch "WALL62_2", 0, 0
			Patch "PS20A0", 1, 60
			Patch "WALL62_2", 64, 0
		}

		WallTexture "BRNSMAL1", 64, 64
		{
			Patch "W111_2", 0, 0
		}

		WallTexture "BRNSMAL2", 64, 64
		{
			Patch "W111_3", 0, 0
		}

		WallTexture "BRNSMALC", 64, 64
		{
			Patch "W112_1", 0, 0
		}

		WallTexture "BRNSMALL", 32, 64
		{
			Patch "W112_3", 0, 0
		}

		WallTexture "BRNSMALR", 32, 64
		{
			Patch "W112_2", 0, 0
		}

		WallTexture "BRONZE1", 64, 128
		{
			Patch "RW10_1", 0, 0
		}

		WallTexture "BRONZE2", 64, 128
		{
			Patch "RW10_2", 0, 0
		}

		WallTexture "BRONZE3", 64, 128
		{
			Patch "RW10_3", 0, 0
		}

		WallTexture "BRONZE4", 64, 128
		{
			Patch "RW38_3", 0, 0
		}

		WallTexture "BROVINE2", 256, 128
		{
			Patch "WALL62_2", 0, 0
			Patch "WALL62_2", 64, 0
			Patch "WALL62_2", 128, 0
			Patch "WALL62_2", 192, 0
			Patch "W106_1", 0, 0
		}

		WallTexture "BROWN1", 128, 128
		{
			Patch "WALL02_2", 0, 56
			Patch "WALL02_2", 0, 0
			Patch "WALL02_3", 64, 56
			Patch "WALL02_1", 88, 56
			Patch "WALL02_3", 64, 0
			Patch "WALL02_1", 88, 0
		}

		WallTexture "BROWN144", 128, 128
		{
			Patch "WALL00_5", 0, 0
			Patch "WALL00_6", 16, 0
			Patch "WALL00_7", 32, 0
			Patch "WALL00_8", 48, 0
			Patch "WALL00_6", 64, 0
			Patch "WALL00_6", 112, -16
			Patch "WALL00_7", 96, -1
			Patch "WALL00_6", 80, 0
		}

		WallTexture "BROWN96", 128, 128
		{
			Patch "WALL62_1", 0, 0
		}

		WallTexture "BROWNGRN", 64, 128
		{
			Patch "WALL62_2", 0, 0
		}

		WallTexture "BROWNHUG", 64, 128
		{
			Patch "WALL03_4", 0, 0
			Patch "WALL03_4", 0, 72
		}

		WallTexture "BROWNPIP", 128, 128
		{
			Patch "TP2_1", 0, 0
			Patch "STEP07", 0, 64
			Patch "STEP07", 32, 64
			Patch "STEP07", 64, 64
			Patch "STEP07", 96, 64
			Patch "WALL05_2", 0, 72
			Patch "WALL05_2", 64, 72
			Patch "STEP07", 0, 120
			Patch "STEP07", 32, 120
			Patch "STEP07", 64, 120
			Patch "STEP07", 96, 120
		}

		WallTexture "BRWINDOW", 64, 128
		{
			Patch "RW6_1", 0, 0
		}

		WallTexture "BSTONE1", 64, 128
		{
			Patch "RW1_1", 0, 0
		}

		WallTexture "BSTONE2", 64, 128
		{
			Patch "RW1_2", 0, 0
		}

		WallTexture "BSTONE3", 64, 128
		{
			Patch "RW12_2", 0, 0
		}

		WallTexture "CEMENT1", 128, 128
		{
			Patch "WALL52_1", 0, 0
		}

		WallTexture "CEMENT2", 128, 128
		{
			Patch "WALL53_1", 0, 0
		}

		WallTexture "CEMENT3", 128, 128
		{
			Patch "WALL54_1", 0, 0
		}

		WallTexture "CEMENT4", 128, 128
		{
			Patch "WALL55_1", 0, 0
		}

		WallTexture "CEMENT5", 128, 128
		{
			Patch "WALL52_2", 0, 0
		}

		WallTexture "CEMENT6", 128, 128
		{
			Patch "WALL54_2", 0, 0
		}

		WallTexture "CEMENT7", 64, 128
		{
			Patch "RW7_1", 0, 0
		}

		WallTexture "CEMENT8", 64, 128
		{
			Patch "RW11_3", 0, 0
		}

		WallTexture "CEMENT9", 64, 128
		{
			Patch "RW28_1", 0, 0
		}

		WallTexture "COMPBLUE", 64, 128
		{
			Patch "COMP03_1", 0, 0
			Patch "COMP03_2", 0, 64
		}

		WallTexture "COMPSPAN", 32, 128
		{
			Patch "COMP03_4", 0, 0
			Patch "COMP03_4", 0, 64
		}

		WallTexture "COMPSTA1", 128, 128
		{
			Patch "TOMW2_1", 0, 0
			Patch "AG128_2", 0, 72
			Patch "AG128_2", 64, 72
		}

		WallTexture "COMPSTA2", 128, 128
		{
			Patch "TOMW2_2", 0, 0
			Patch "AG128_2", 0, 72
			Patch "AG128_2", 64, 72
		}

		WallTexture "COMPTALL", 256, 128
		{
			Patch "COMP04_5", 32, 0
			Patch "COMP04_8", 96, 0
			Patch "COMP03_8", 160, 0
			Patch "COMP04_6", 0, 64
			Patch "COMP04_7", 64, 64
			Patch "COMP04_2", 128, 64
			Patch "COMP03_5", 192, 0
			Patch "COMP04_1", 192, 64
			Patch "COMP03_8", 0, 0
		}

		WallTexture "COMPWERD", 64, 128
		{
			Patch "COMP04_6", 0, 0
			Patch "COMP04_6", 0, 64
		}

		WallTexture "CRACKLE2", 64, 128
		{
			Patch "RW44_2", 0, 0
		}

		WallTexture "CRACKLE4", 64, 128
		{
			Patch "RW44_4", 0, 0
		}

		WallTexture "CRATE1", 64, 128
		{
			Patch "BCRATEL1", 0, 0
			Patch "BCRATER1", 32, 0
			Patch "BCRATEL1", 0, 64
			Patch "BCRATER1", 32, 64
		}

		WallTexture "CRATE2", 64, 128
		{
			Patch "GCRATEL1", 0, 0
			Patch "GCRATER1", 32, 0
			Patch "GCRATEL1", 0, 64
			Patch "GCRATER1", 32, 64
		}

		WallTexture "CRATE3", 64, 128
		{
			Patch "GCRATEL1", 0, 0
			Patch "GCRATER1", 32, 0
			Patch "BCRATEL1", 0, 64
			Patch "BCRATER1", 32, 64
		}

		WallTexture "CRATELIT", 64, 128
		{
			Patch "SGCRATE2", 0, 0
			Patch "SGCRATE2", 32, 0
			Patch "BCRATEL1", 0, 64
			Patch "BCRATER1", 32, 64
		}

		WallTexture "CRATINY", 64, 16
		{
			Patch "VGCRATE1", 0, 0
			Patch "VGCRATE1", 16, 0
			Patch "VGCRATE1", 32, 0
			Patch "VGCRATE1", 48, 0
		}

		WallTexture "CRATWIDE", 128, 128
		{
			Patch "BCRATEL1", 0, 64
			Patch "BCRATEM1", 40, 64
			Patch "BCRATEM1", 32, 64
			Patch "BCRATEM1", 48, 64
			Patch "BCRATEM1", 56, 64
			Patch "BCRATER1", 96, 64
			Patch "BCRATEM1", 88, 64
			Patch "BCRATEM1", 80, 64
			Patch "BCRATEM1", 72, 64
			Patch "BCRATEM1", 64, 64
			Patch "GCRATEL1", 0, 0
			Patch "GCRATER1", 96, 0
			Patch "GCRATEM1", 48, 0
			Patch "GCRATEM1", 40, 0
			Patch "GCRATEM1", 32, 0
			Patch "GCRATEM1", 72, 0
			Patch "GCRATEM1", 64, 0
			Patch "GCRATEM1", 56, 0
			Patch "GCRATEM1", 80, 0
			Patch "GCRATEM1", 88, 0
		}

		WallTexture "DBRAIN1", 64, 32
		{
			Patch "RWDM11A", 0, 0
		}

		WallTexture "DBRAIN2", 64, 32
		{
			Patch "RWDM11B", 0, 0
		}

		WallTexture "DBRAIN3", 64, 32
		{
			Patch "RWDM11C", 0, 0
		}

		WallTexture "DBRAIN4", 64, 32
		{
			Patch "RWDM11D", 0, 0
		}

		WallTexture "DOOR1", 64, 72
		{
			Patch "WALL03_1", 0, 0
		}

		WallTexture "DOOR3", 64, 72
		{
			Patch "DOOR2_5", 0, 0
		}

		WallTexture "DOORBLU", 8, 128
		{
			Patch "W46_37", 0, 112
			Patch "W46_37", 0, 0
			Patch "W46_37", 0, 96
			Patch "W46_37", 0, 80
			Patch "W46_37", 0, 16
			Patch "W46_37", 0, 32
			Patch "W46_37", 0, 48
			Patch "W46_37", 0, 64
		}

		WallTexture "DOORBLU2", 16, 128
		{
			Patch "W108_2", 0, 0
			Patch "W108_2", 0, 24
			Patch "W108_2", 0, 48
			Patch "W108_2", 0, 72
			Patch "W108_2", 0, 96
			Patch "STEP07", 0, 120
		}

		WallTexture "DOORRED", 8, 128
		{
			Patch "W46_38", 0, 0
			Patch "W46_38", 0, 16
			Patch "W46_38", 0, 32
			Patch "W46_38", 0, 48
			Patch "W46_38", 0, 64
			Patch "W46_38", 0, 80
			Patch "W46_38", 0, 96
			Patch "W46_38", 0, 112
		}

		WallTexture "DOORRED2", 16, 128
		{
			Patch "W108_3", 0, 0
			Patch "W108_3", 0, 24
			Patch "W108_3", 0, 48
			Patch "W108_3", 0, 72
			Patch "W108_3", 0, 96
			Patch "STEP07", 0, 120
		}

		WallTexture "DOORSTOP", 8, 128
		{
			Patch "TTALL1_2", 0, 0
		}

		WallTexture "DOORTRAK", 8, 128
		{
			Patch "DOORTRAK", 0, 0
		}

		WallTexture "DOORYEL", 8, 128
		{
			Patch "W46_39", 0, 112
			Patch "W46_39", 0, 80
			Patch "W46_39", 0, 64
			Patch "W46_39", 0, 0
			Patch "W46_39", 0, 16
			Patch "W46_39", 0, 48
			Patch "W46_39", 0, 32
			Patch "W46_39", 0, 96
		}

		WallTexture "DOORYEL2", 16, 128
		{
			Patch "W108_4", 0, 0
			Patch "W108_4", 0, 24
			Patch "W108_4", 0, 48
			Patch "W108_4", 0, 72
			Patch "W108_4", 0, 96
			Patch "STEP07", 0, 120
		}

		WallTexture "EXITDOOR", 128, 72
		{
			Patch "DOOR3_6", 0, 0
			Patch "DOOR3_4", 64, 0
			Patch "DOOR3_5", 88, 0
			Patch "T14_5", 112, 0
		}

		WallTexture "EXITSIGN", 64, 16
		{
			Patch "EXIT1", 0, 0
			Patch "EXIT2", 32, 0
			Patch "EXIT2", 40, 0
			Patch "EXIT2", 48, 0
			Patch "EXIT2", 56, 0
		}

		WallTexture "EXITSTON", 64, 128
		{
			Patch "W28_8", 0, 64
			Patch "W28_8", 0, 0
			Patch "EXIT1", 17, 22
		}

		WallTexture "FIREBLU1", 128, 128
		{
			Patch "W65B_1", 0, 0
		}

		WallTexture "FIREBLU2", 128, 128
		{
			Patch "W65B_2", 0, 0
		}

		WallTexture "FIRELAV2", 128, 128
		{
			Patch "W73A_2", 0, 0
		}

		WallTexture "FIRELAV3", 128, 128
		{
			Patch "W73B_1", 0, 0
		}

		WallTexture "FIRELAVA", 128, 128
		{
			Patch "W73A_1", 0, 0
		}

		WallTexture "FIREMAG1", 128, 128
		{
			Patch "W74A_1", 0, 0
		}

		WallTexture "FIREMAG2", 128, 128
		{
			Patch "W74A_2", 0, 0
		}

		WallTexture "FIREMAG3", 128, 128
		{
			Patch "W74B_1", 0, 0
		}

		WallTexture "FIREWALA", 128, 112
		{
			Patch "WALL23_1", 0, 0
		}

		WallTexture "FIREWALB", 128, 112
		{
			Patch "WALL23_2", 0, 0
		}

		WallTexture "FIREWALL", 128, 112
		{
			Patch "WALL22_1", 0, 0
		}

		WallTexture "GRAY1", 64, 128
		{
			Patch "W31_1", 0, 64
			Patch "W31_1", 0, 0
		}

		WallTexture "GRAY2", 64, 72
		{
			Patch "W33_7", 0, -8
			Patch "W31_1", 0, 56
			Patch "DUCT1", 20, 16
		}

		WallTexture "GRAY4", 64, 128
		{
			Patch "W33_5", 0, 0
			Patch "W33_5", 0, 64
		}

		WallTexture "GRAY5", 64, 128
		{
			Patch "W33_7", 0, 72
			Patch "W33_7", 0, 0
			Patch "W33_8", 0, 64
		}

		WallTexture "GRAY7", 256, 128
		{
			Patch "W32_4", 0, 0
			Patch "W32_1", 64, 0
			Patch "W32_4", 128, 0
			Patch "W33_8", 0, 120
			Patch "W33_8", 64, 120
			Patch "W33_8", 128, 120
			Patch "W32_4", 192, 0
			Patch "W33_8", 192, 120
			Patch "W32_4", 64, 56
			Patch "W32_1", 128, 56
			Patch "W32_4", 192, 56
			Patch "W32_4", 0, 56
		}

		WallTexture "GRAYBIG", 128, 128
		{
			Patch "WALL00_1", 0, 0
			Patch "WALL00_1", 64, 0
			Patch "WALL00_2", 83, 0
		}

		WallTexture "GRAYPOIS", 64, 72
		{
			Patch "WALL04_9", 0, 0
			Patch "WALL04_A", 16, 0
			Patch "WALL04_B", 32, 0
			Patch "WALL04_C", 48, 0
			Patch "PS18A0", 1, 19
		}

		WallTexture "GRAYTALL", 128, 128
		{
			Patch "WALL00_1", 16, 0
			Patch "WALL00_3", 80, 0
			Patch "WALL00_2", 0, 0
			Patch "WALL00_1", 96, 0
		}

		WallTexture "GRAYVINE", 256, 128
		{
			Patch "WALL00_1", 0, 0
			Patch "WALL00_1", 64, 0
			Patch "WALL00_1", 128, -16
			Patch "WALL00_1", 192, -16
			Patch "W106_1", 0, 0
		}

		WallTexture "GSTFONT1", 64, 128
		{
			Patch "WALL58_1", 0, 0
		}

		WallTexture "GSTFONT2", 64, 128
		{
			Patch "WALL58_2", 0, 0
		}

		WallTexture "GSTFONT3", 64, 128
		{
			Patch "WALL58_3", 0, 0
		}

		WallTexture "GSTGARG", 64, 128
		{
			Patch "WALL30_2", 0, 0
		}

		WallTexture "GSTLION", 64, 128
		{
			Patch "WALL30_4", 0, 0
		}

		WallTexture "GSTONE1", 256, 128
		{
			Patch "WALL48_1", 0, 0
			Patch "WALL48_2", 64, 0
			Patch "WALL48_3", 128, 0
			Patch "WALL48_4", 192, 0
		}

		WallTexture "GSTONE2", 256, 128
		{
			Patch "WALL59_1", 0, 0
			Patch "WALL59_2", 64, 0
			Patch "WALL59_3", 128, 0
			Patch "WALL59_4", 192, 0
		}

		WallTexture "GSTSATYR", 64, 128
		{
			Patch "WALL30_3", 0, 0
		}

		WallTexture "GSTVINE1", 256, 128
		{
			Patch "WALL48_1", 0, 0
			Patch "WALL48_2", 64, 0
			Patch "WALL48_3", 128, 0
			Patch "WALL48_4", 192, 0
			Patch "W106_1", 0, 0
		}

		WallTexture "GSTVINE2", 256, 128
		{
			Patch "WALL48_1", 0, 0
			Patch "WALL48_2", 64, 0
			Patch "WALL48_3", 128, 0
			Patch "WALL48_4", 192, 0
			Patch "W107_1", 0, 0
		}

		WallTexture "ICKWALL1", 64, 128
		{
			Patch "WALL69_4", 0, 0
			Patch "W32_4", 0, 64
		}

		WallTexture "ICKWALL2", 64, 128
		{
			Patch "WALL71_5", 0, 0
			Patch "W33_5", 0, 64
		}

		WallTexture "ICKWALL3", 64, 128
		{
			Patch "WALL72_7", 0, 0
			Patch "WALL69_9", 0, 64
			Patch "W67_2", 32, 64
		}

		WallTexture "ICKWALL4", 64, 128
		{
			Patch "WALL72_5", 0, 0
			Patch "WALL72_3", 0, 64
		}

		WallTexture "ICKWALL5", 64, 128
		{
			Patch "WALL70_4", 0, 0
			Patch "W32_4", 0, 64
		}

		WallTexture "ICKWALL7", 64, 128
		{
			Patch "W67_1", 0, 0
			Patch "W67_2", 63, 64
			Patch "WALL70_9", 28, 64
			Patch "W67_2", -1, 64
		}

		WallTexture "LITE3", 32, 128
		{
			Patch "WLITA0", 0, 0
			Patch "WLITB0", 8, 0
			Patch "WLITB0", 16, 0
			Patch "WLITC0", 24, 0
			Patch "WLITA0", 0, 8
			Patch "WLITB0", 8, 8
			Patch "WLITB0", 16, 8
			Patch "WLITC0", 24, 8
			Patch "WLITA0", 0, 16
			Patch "WLITB0", 8, 16
			Patch "WLITB0", 16, 16
			Patch "WLITC0", 24, 16
			Patch "WLITA0", 0, 24
			Patch "WLITB0", 8, 24
			Patch "WLITB0", 16, 24
			Patch "WLITC0", 24, 24
			Patch "WLITA0", 0, 32
			Patch "WLITB0", 8, 32
			Patch "WLITB0", 16, 32
			Patch "WLITC0", 24, 32
			Patch "WLITA0", 0, 40
			Patch "WLITB0", 8, 40
			Patch "WLITB0", 16, 40
			Patch "WLITC0", 24, 40
			Patch "WLITA0", 0, 48
			Patch "WLITB0", 8, 48
			Patch "WLITB0", 16, 48
			Patch "WLITC0", 24, 48
			Patch "WLITA0", 0, 56
			Patch "WLITB0", 8, 56
			Patch "WLITB0", 16, 56
			Patch "WLITC0", 24, 56
			Patch "WLITA0", 0, 64
			Patch "WLITB0", 8, 64
			Patch "WLITB0", 16, 64
			Patch "WLITC0", 24, 64
			Patch "WLITA0", 0, 72
			Patch "WLITB0", 8, 72
			Patch "WLITB0", 16, 72
			Patch "WLITC0", 24, 72
			Patch "WLITA0", 0, 80
			Patch "WLITB0", 8, 80
			Patch "WLITB0", 16, 80
			Patch "WLITC0", 24, 80
			Patch "WLITA0", 0, 88
			Patch "WLITB0", 8, 88
			Patch "WLITB0", 16, 88
			Patch "WLITC0", 24, 88
			Patch "WLITA0", 0, 96
			Patch "WLITB0", 8, 96
			Patch "WLITB0", 16, 96
			Patch "WLITC0", 24, 96
			Patch "WLITA0", 0, 104
			Patch "WLITB0", 8, 104
			Patch "WLITB0", 16, 104
			Patch "WLITC0", 24, 104
			Patch "WLITA0", 0, 112
			Patch "WLITB0", 8, 112
			Patch "WLITB0", 16, 112
			Patch "WLITC0", 24, 112
			Patch "WLITA0", 0, 120
			Patch "WLITB0", 8, 120
			Patch "WLITB0", 16, 120
			Patch "WLITC0", 24, 120
		}

		WallTexture "LITE5", 16, 128
		{
			Patch "WLITA0", 0, 0
			Patch "WLITC0", 8, 0
			Patch "WLITA0", 0, 8
			Patch "WLITC0", 8, 8
			Patch "WLITA0", 0, 16
			Patch "WLITC0", 8, 16
			Patch "WLITA0", 0, 24
			Patch "WLITC0", 8, 24
			Patch "WLITA0", 0, 32
			Patch "WLITC0", 8, 32
			Patch "WLITA0", 0, 40
			Patch "WLITC0", 8, 40
			Patch "WLITA0", 0, 48
			Patch "WLITC0", 8, 48
			Patch "WLITA0", 0, 56
			Patch "WLITC0", 8, 56
			Patch "WLITA0", 0, 64
			Patch "WLITC0", 8, 64
			Patch "WLITA0", 0, 72
			Patch "WLITC0", 8, 72
			Patch "WLITA0", 0, 80
			Patch "WLITC0", 8, 80
			Patch "WLITA0", 0, 88
			Patch "WLITC0", 8, 88
			Patch "WLITA0", 0, 96
			Patch "WLITC0", 8, 96
			Patch "WLITA0", 0, 104
			Patch "WLITC0", 8, 104
			Patch "WLITA0", 0, 112
			Patch "WLITC0", 8, 112
			Patch "WLITA0", 0, 120
			Patch "WLITC0", 8, 120
		}

		WallTexture "LITEBLU1", 8, 128
		{
			Patch "AGB128_1", 0, 0
		}

		WallTexture "LITEBLU4", 16, 128
		{
			Patch "BLITA0", 0, 0
			Patch "BLITB0", 8, 0
			Patch "BLITC0", 8, 0
			Patch "BLITA0", 0, 8
			Patch "BLITC0", 8, 8
			Patch "BLITA0", 0, 16
			Patch "BLITC0", 8, 16
			Patch "BLITA0", 0, 24
			Patch "BLITC0", 8, 24
			Patch "BLITA0", 0, 32
			Patch "BLITC0", 8, 32
			Patch "BLITA0", 0, 40
			Patch "BLITC0", 8, 40
			Patch "BLITA0", 0, 48
			Patch "BLITC0", 8, 48
			Patch "BLITA0", 0, 56
			Patch "BLITC0", 8, 56
			Patch "BLITA0", 0, 64
			Patch "BLITC0", 8, 64
			Patch "BLITA0", 0, 72
			Patch "BLITC0", 8, 72
			Patch "BLITA0", 0, 80
			Patch "BLITC0", 8, 80
			Patch "BLITA0", 0, 88
			Patch "BLITC0", 8, 88
			Patch "BLITA0", 0, 96
			Patch "BLITC0", 8, 96
			Patch "BLITA0", 0, 104
			Patch "BLITC0", 8, 104
			Patch "BLITA0", 0, 112
			Patch "BLITC0", 8, 112
			Patch "BLITA0", 0, 120
			Patch "BLITC0", 8, 120
		}

		WallTexture "MARBFAC2", 128, 128
		{
			Patch "MWALL4_2", 0, 0
		}

		WallTexture "MARBFAC3", 128, 128
		{
			Patch "MWALL5_1", 0, 0
		}

		WallTexture "MARBFAC4", 64, 128
		{
			Patch "RW7_3", 0, 0
		}

		WallTexture "MARBFACE", 128, 128
		{
			Patch "MWALL4_1", 0, 0
		}

		WallTexture "MARBGRAY", 64, 128
		{
			Patch "RW7_2", 0, 0
		}

		WallTexture "MARBLE1", 128, 128
		{
			Patch "MWALL1_1", 0, 0
		}

		WallTexture "MARBLE2", 128, 128
		{
			Patch "MWALL2_1", 0, 0
		}

		WallTexture "MARBLE3", 128, 128
		{
			Patch "MWALL3_1", 0, 0
		}

		WallTexture "MARBLOD1", 128, 128
		{
			Patch "MWALL1_2", 0, 0
		}

		WallTexture "METAL", 64, 128
		{
			Patch "WALL47_1", 0, 0
		}

		WallTexture "METAL1", 64, 128
		{
			Patch "WALL03_7", 0, 0
			Patch "WALL03_7", 0, 64
		}

		WallTexture "METAL2", 64, 128
		{
			Patch "RW33_1", 0, 0
		}

		WallTexture "METAL3", 64, 128
		{
			Patch "RW33_2", 0, 0
		}

		WallTexture "METAL4", 64, 128
		{
			Patch "RW33_3", 0, 0
		}

		WallTexture "METAL5", 64, 128
		{
			Patch "RW33_4", 0, 0
		}

		WallTexture "METAL6", 64, 128
		{
			Patch "RW38_1", 0, 0
		}

		WallTexture "METAL7", 64, 128
		{
			Patch "RW38_2", 0, 0
		}

		WallTexture "MIDBARS1", 64, 128
		{
			Patch "RW43_1", 0, 0
		}

		WallTexture "MIDBARS3", 64, 72
		{
			Patch "RW45_1", 0, 0
		}

		WallTexture "MIDBRN1", 64, 112
		{
			Patch "DOOR12_1", 0, 0
		}

		WallTexture "MIDBRONZ", 64, 128
		{
			Patch "RW10_4", 0, 0
		}

		WallTexture "MIDGRATE", 128, 128
		{
			Patch "M1_1", 0, 0
		}

		WallTexture "MIDSPACE", 64, 128
		{
			Patch "RW47_1", 0, 0
		}

		WallTexture "MODWALL1", 64, 128
		{
			Patch "RW31_1", 0, 0
		}

		WallTexture "MODWALL2", 64, 128
		{
			Patch "RW31_2", 0, 0
		}

		WallTexture "MODWALL3", 64, 128
		{
			Patch "RW31_3", 0, 0
		}

		WallTexture "MODWALL4", 64, 128
		{
			Patch "RW31_4", 0, 0
		}

		WallTexture "NUKE24", 64, 24
		{
			Patch "NUKEDGE", 0, 0
		}

		WallTexture "NUKEDGE1", 128, 128
		{
			Patch "WALL04_3", 0, 0
			Patch "WALL04_4", 16, 0
			Patch "WALL04_5", 32, 0
			Patch "WALL04_2", 48, 0
			Patch "WALL04_2", 64, 0
			Patch "WALL04_5", 96, 32
			Patch "WALL04_3", 80, 0
			Patch "WALL04_5", 112, 0
			Patch "WALL04_7", 96, 0
			Patch "NUKEDGE", 0, 104
			Patch "NUKEDGE", 64, 104
			Patch "WALL04_5", 112, 32
			Patch "WALL04_3", 0, 33
			Patch "WALL04_4", 16, 33
			Patch "WALL04_5", 32, 33
			Patch "WALL04_2", 48, 33
			Patch "WALL04_2", 64, 33
			Patch "WALL04_3", 80, 33
		}

		WallTexture "NUKEPOIS", 128, 128
		{
			Patch "WALL04_3", 0, 0
			Patch "WALL04_4", 16, 0
			Patch "WALL04_5", 32, 0
			Patch "WALL04_2", 48, 0
			Patch "NUKEDGE", 0, 104
			Patch "WALL04_3", 0, 33
			Patch "WALL04_4", 16, 33
			Patch "WALL04_5", 32, 33
			Patch "WALL04_2", 48, 33
			Patch "PS20A0", 0, 69
			Patch "WALL04_2", 64, 0
			Patch "WALL04_3", 80, 0
			Patch "WALL04_4", 112, 0
			Patch "WALL04_7", 96, 0
			Patch "WALL04_2", 112, 72
			Patch "WALL04_3", 96, 72
			Patch "WALL04_4", 80, 72
			Patch "WALL04_5", 64, 64
			Patch "NUKEDGE", 64, 105
		}

		WallTexture "PANBLACK", 64, 128
		{
			Patch "RW16_1", 0, 0
		}

		WallTexture "PANBLUE", 64, 128
		{
			Patch "RW16_3", 0, 0
		}

		WallTexture "PANBOOK", 64, 128
		{
			Patch "RW21_5", 0, 0
		}

		WallTexture "PANBORD1", 32, 128
		{
			Patch "RW21_1", 0, 0
		}

		WallTexture "PANBORD2", 16, 128
		{
			Patch "RW21_2", 0, 0
		}

		WallTexture "PANCASE1", 64, 128
		{
			Patch "RW21_3", 0, 0
		}

		WallTexture "PANCASE2", 64, 128
		{
			Patch "RW21_4", 0, 0
		}

		WallTexture "PANEL1", 64, 128
		{
			Patch "RW15_1", 0, 0
		}

		WallTexture "PANEL2", 64, 128
		{
			Patch "RW15_2", 0, 0
		}

		WallTexture "PANEL3", 64, 128
		{
			Patch "RW15_3", 0, 0
		}

		WallTexture "PANEL4", 64, 128
		{
			Patch "RW15_4", 0, 0
		}

		WallTexture "PANEL5", 64, 128
		{
			Patch "RW16_4", 0, 0
		}

		WallTexture "PANEL6", 64, 128
		{
			Patch "RW19_1", 0, 0
		}

		WallTexture "PANEL7", 64, 128
		{
			Patch "RW19_2", 0, 0
		}

		WallTexture "PANEL8", 64, 128
		{
			Patch "RW19_3", 0, 0
		}

		WallTexture "PANEL9", 64, 128
		{
			Patch "RW19_4", 0, 0
		}

		WallTexture "PANRED", 64, 128
		{
			Patch "RW16_2", 0, 0
		}

		WallTexture "PIPE1", 256, 128
		{
			Patch "RP1_1", 0, 0
			Patch "RP1_2", 128, 0
		}

		WallTexture "PIPE2", 256, 128
		{
			Patch "TP2_1", 0, 0
			Patch "TP2_2", 128, 0
		}

		WallTexture "PIPE4", 256, 128
		{
			Patch "TP7_1", 0, 0
			Patch "TP7_2", 128, 0
		}

		WallTexture "PIPE6", 256, 128
		{
			Patch "TP3_1", 0, 0
			Patch "TP3_2", 128, 0
		}

		WallTexture "PIPES", 64, 128
		{
			Patch "RW28_4", 0, 0
		}

		WallTexture "PIPEWAL1", 64, 128
		{
			Patch "RW28_2", 0, 0
		}

		WallTexture "PIPEWAL2", 64, 128
		{
			Patch "RW36_2", 0, 0
		}

		WallTexture "PLAT1", 128, 128
		{
			Patch "PLAT2_1", 0, 0
		}

		WallTexture "REDWALL", 128, 128
		{
			Patch "WALL78_1", 0, 0
		}

		WallTexture "ROCK1", 64, 128
		{
			Patch "RW30_1", 0, 0
		}

		WallTexture "ROCK2", 64, 128
		{
			Patch "RW30_2", 0, 0
		}

		WallTexture "ROCK3", 64, 128
		{
			Patch "RW30_3", 0, 0
		}

		WallTexture "ROCK4", 128, 128
		{
			Patch "RW35_1", 0, 0
		}

		WallTexture "ROCK5", 128, 128
		{
			Patch "RW35_2", 0, 0
		}

		WallTexture "ROCKRED1", 128, 128
		{
			Patch "WALL64_2", 0, 0
		}

		WallTexture "ROCKRED2", 128, 128
		{
			Patch "W64B_1", 0, 0
		}

		WallTexture "ROCKRED3", 128, 128
		{
			Patch "W64B_2", 0, 0
		}

		WallTexture "SFALL1", 64, 128
		{
			Patch "SFALL1", 0, 0
		}

		WallTexture "SFALL2", 64, 128
		{
			Patch "SFALL2", 0, 0
		}

		WallTexture "SFALL3", 64, 128
		{
			Patch "SFALL3", 0, 0
		}

		WallTexture "SFALL4", 64, 128
		{
			Patch "SFALL4", 0, 0
		}

		WallTexture "SHAWN1", 128, 128
		{
			Patch "W13_1", 64, 56
			Patch "W13_1", 64, 0
			Patch "W13_1", 0, 56
			Patch "CYL1_1", 0, 0
		}

		WallTexture "SHAWN2", 64, 128
		{
			Patch "AG128_1", 0, 0
		}

		WallTexture "SHAWN3", 64, 72
		{
			Patch "T14_3", 0, 0
		}

		WallTexture "SILVER1", 64, 128
		{
			Patch "RW32_1", 0, 0
		}

		WallTexture "SILVER2", 64, 128
		{
			Patch "RW32_2", 0, 0
		}

		WallTexture "SILVER3", 64, 128
		{
			Patch "RW32_3", 0, 0
		}

		WallTexture "SK_LEFT", 64, 128
		{
			Patch "RW48_1", 0, 0
		}

		WallTexture "SK_RIGHT", 64, 128
		{
			Patch "RW48_3", 0, 0
		}

		WallTexture "SKIN2", 128, 128
		{
			Patch "HELL8_2", 64, 0
			Patch "HELL8_4", 0, 0
		}

		WallTexture "SKINCUT", 256, 128
		{
			Patch "W102_1", 0, 0
			Patch "W102_2", 128, 0
		}

		WallTexture "SKINEDGE", 128, 128
		{
			Patch "HELL6_2", 0, 0
			Patch "HELL8_1", 64, 0
		}

		WallTexture "SKINFACE", 256, 128
		{
			Patch "HELL5_1", 0, 0
			Patch "HELL5_2", 128, 0
		}

		WallTexture "SKINLOW", 256, 104
		{
			Patch "W92_1", 0, 0
			Patch "W92_2", 128, 0
		}

		WallTexture "SKINMET1", 256, 128
		{
			Patch "W98_1", 0, 0
			Patch "W98_2", 128, 0
		}

		WallTexture "SKINMET2", 256, 128
		{
			Patch "W99_1", 0, 0
			Patch "W99_2", 128, 0
		}

		WallTexture "SKINSCAB", 256, 128
		{
			Patch "W101_1", 0, 0
			Patch "W101_2", 128, 0
		}

		WallTexture "SKINSYMB", 256, 128
		{
			Patch "W103_1", 0, 0
			Patch "W103_2", 128, 0
		}

		WallTexture "SKSNAKE1", 64, 128
		{
			Patch "SNAK7_1", 0, 0
		}

		WallTexture "SKSNAKE2", 64, 128
		{
			Patch "SNAK8_1", 0, 0
		}

		WallTexture "SKSPINE1", 128, 128
		{
			Patch "SPINE4_1", 0, 0
		}

		WallTexture "SKSPINE2", 256, 96
		{
			Patch "SPINE3_1", 0, 0
			Patch "SPINE3_2", 128, 0
		}

		WallTexture "SKY1", 256, 128
		{
			Patch "RSKY1", 0, 0
		}

		WallTexture "SKY2", 256, 128
		{
			Patch "RSKY2", 0, 0
		}

		WallTexture "SKY3", 256, 128
		{
			Patch "RSKY3", 0, 0
		}

		WallTexture "SLADPOIS", 64, 128
		{
			Patch "WLA128_1", 0, 0
			Patch "PS20A0", 1, 49
		}

		WallTexture "SLADSKUL", 64, 128
		{
			Patch "WLA128_1", 0, 0
			Patch "SW2_2", 21, 65
		}

		WallTexture "SLADWALL", 64, 128
		{
			Patch "WLA128_1", 0, 0
		}

		WallTexture "SLOPPY1", 64, 128
		{
			Patch "RW47_3", 0, 0
		}

		WallTexture "SLOPPY2", 64, 128
		{
			Patch "RW47_4", 0, 0
		}

		WallTexture "SP_DUDE1", 128, 128
		{
			Patch "WALL50_1", 0, 0
		}

		WallTexture "SP_DUDE2", 128, 128
		{
			Patch "WALL50_2", 0, 0
		}

		WallTexture "SP_DUDE4", 64, 128
		{
			Patch "WALL51_2", 0, 0
		}

		WallTexture "SP_DUDE5", 64, 128
		{
			Patch "WALL51_3", 0, 0
		}

		WallTexture "SP_DUDE7", 128, 128
		{
			Patch "BODY_1", 0, 0
		}

		WallTexture "SP_DUDE8", 128, 128
		{
			Patch "BODY_2", 0, 0
		}

		WallTexture "SP_FACE1", 128, 96
		{
			Patch "WALL25_1", 0, 0
		}

		WallTexture "SP_FACE2", 64, 128
		{
			Patch "BODIES", 0, 0
		}

		WallTexture "SP_HOT1", 256, 128
		{
			Patch "WALL49_1", 0, 0
			Patch "WALL49_2", 64, 0
			Patch "WALL49_3", 128, 0
			Patch "WALL49_4", 192, 0
		}

		WallTexture "SP_ROCK1", 128, 128
		{
			Patch "WALL63_1", 0, 0
		}

		WallTexture "SPACEW2", 64, 128
		{
			Patch "RW46_2", 0, 0
		}

		WallTexture "SPACEW3", 64, 128
		{
			Patch "RW46_3", 0, 0
		}

		WallTexture "SPACEW4", 64, 128
		{
			Patch "RW46_4", 0, 0
		}

		WallTexture "SPCDOOR1", 64, 128
		{
			Patch "DOOR15_1", 0, 0
		}

		WallTexture "SPCDOOR2", 64, 128
		{
			Patch "DOOR15_2", 0, 0
		}

		WallTexture "SPCDOOR3", 64, 128
		{
			Patch "DOOR15_3", 0, 0
		}

		WallTexture "SPCDOOR4", 64, 128
		{
			Patch "DOOR15_4", 0, 0
		}

		WallTexture "STARBR2", 128, 128
		{
			Patch "SW15_4", 0, 0
			Patch "SW16_4", 32, 0
			Patch "SW15_6", 64, 0
			Patch "SW16_6", 96, 0
		}

		WallTexture "STARG1", 64, 128
		{
			Patch "SW12_1", 0, 0
			Patch "SW12_2", 32, 0
		}

		WallTexture "STARG2", 128, 128
		{
			Patch "SW17_1", 0, 0
			Patch "SW17_2", 32, 0
			Patch "SW17_3", 64, 0
			Patch "SW18_5", 96, 0
		}

		WallTexture "STARG3", 128, 128
		{
			Patch "SW19_3", 64, 0
			Patch "SW19_4", 0, 0
		}

		WallTexture "STARGR1", 64, 128
		{
			Patch "SW11_1", 0, 0
			Patch "SW11_2", 32, 0
		}

		WallTexture "STARGR2", 128, 128
		{
			Patch "SW15_1", 0, 0
			Patch "SW15_3", 64, 0
			Patch "SW16_1", 96, 0
			Patch "SW16_2", 32, 0
		}

		WallTexture "STARTAN2", 128, 128
		{
			Patch "SW17_4", 0, 0
			Patch "SW17_5", 32, 0
			Patch "SW17_6", 64, 0
			Patch "SW18_7", 96, 0
		}

		WallTexture "STARTAN3", 128, 128
		{
			Patch "SW19_1", 64, 0
			Patch "SW19_2", 0, 0
		}

		WallTexture "STEP1", 32, 16
		{
			Patch "STEP05", 0, 8
			Patch "STEP05", 0, 5
			Patch "STEP04", 0, 0
		}

		WallTexture "STEP2", 32, 16
		{
			Patch "SW11_4", 0, -112
			Patch "STEP03", 0, 0
		}

		WallTexture "STEP3", 32, 16
		{
			Patch "STEP05", 0, 0
			Patch "STEP05", 0, 8
		}

		WallTexture "STEP4", 32, 16
		{
			Patch "STEP06", 0, 0
			Patch "STEP06", 0, 8
		}

		WallTexture "STEP5", 32, 16
		{
			Patch "STEP09", 0, 0
			Patch "STEP08", 0, 8
		}

		WallTexture "STEP6", 32, 16
		{
			Patch "STEP10", 0, 8
			Patch "STEP07", 0, 0
		}

		WallTexture "STEPLAD1", 64, 16
		{
			Patch "LADDER16", 0, 0
		}

		WallTexture "STEPTOP", 128, 16
		{
			Patch "RIPW15", 0, 0
		}

		WallTexture "STONE", 256, 128
		{
			Patch "WALL01_1", 0, 0
			Patch "WALL01_2", 16, 0
			Patch "WALL01_3", 32, 0
			Patch "WALL01_4", 48, 0
			Patch "WALL01_5", 64, 0
			Patch "WALL01_6", 80, 0
			Patch "WALL01_7", 96, 0
			Patch "WALL01_8", 112, 0
			Patch "WALL01_9", 128, 0
			Patch "WALL01_A", 144, 0
			Patch "WALL01_B", 160, 0
			Patch "WALL01_C", 176, 0
			Patch "WALL01_3", 192, 0
			Patch "WALL01_1", 208, 0
			Patch "WALL01_6", 224, 0
			Patch "WALL01_A", 240, 0
			Patch "WALL01_1", 0, 72
			Patch "WALL01_2", 16, 72
			Patch "WALL01_3", 32, 72
			Patch "WALL01_4", 48, 72
			Patch "WALL01_5", 64, 72
			Patch "WALL01_6", 80, 72
			Patch "WALL01_7", 96, 72
			Patch "WALL01_8", 112, 72
			Patch "WALL01_9", 128, 72
			Patch "WALL01_A", 144, 72
			Patch "WALL01_B", 160, 72
			Patch "WALL01_C", 176, 72
			Patch "WALL01_3", 192, 72
			Patch "WALL01_1", 208, 72
			Patch "WALL01_6", 224, 72
			Patch "WALL01_A", 240, 72
			Patch "W33_8", 0, 64
			Patch "W33_8", 64, 64
			Patch "W33_8", 128, 64
			Patch "W33_8", 192, 64
			Patch "W33_8", 0, 120
			Patch "W33_8", 64, 120
			Patch "W33_8", 128, 120
			Patch "W33_8", 192, 120
		}

		WallTexture "STONE2", 128, 128
		{
			Patch "W28_8", 0, 64
			Patch "W28_8", 64, 0
			Patch "W28_5", 0, 0
			Patch "W28_5", 64, 64
		}

		WallTexture "STONE3", 128, 128
		{
			Patch "W28_7", 0, 0
			Patch "W28_6", 0, 64
			Patch "W28_7", 64, 64
			Patch "W28_6", 64, 0
		}

		WallTexture "STONE4", 64, 128
		{
			Patch "RW18_1", 0, 0
		}

		WallTexture "STONE5", 64, 128
		{
			Patch "RW18_2", 0, 0
		}

		WallTexture "STONE6", 64, 128
		{
			Patch "RW18_3", 0, 0
		}

		WallTexture "STONE7", 64, 128
		{
			Patch "RW18_4", 0, 0
		}

		WallTexture "STUCCO", 64, 128
		{
			Patch "RW8_1", 0, 0
		}

		WallTexture "STUCCO1", 64, 128
		{
			Patch "RW8_2", 0, 0
		}

		WallTexture "STUCCO2", 64, 128
		{
			Patch "RW8_3", 0, 0
		}

		WallTexture "STUCCO3", 64, 128
		{
			Patch "RW8_4", 0, 0
		}

		WallTexture "SUPPORT2", 64, 128
		{
			Patch "SUPPORT2", 19, 72
			Patch "SUPPORT2", 19, 0
			Patch "SUPPORT2", 0, 0
			Patch "SUPPORT2", 0, 72
			Patch "SUPPORT2", 40, 0
			Patch "SUPPORT2", 40, 72
		}

		WallTexture "SUPPORT3", 64, 128
		{
			Patch "WALL42_3", 20, 0
			Patch "WALL42_3", 0, 0
			Patch "WALL42_3", 40, 0
		}

		WallTexture "SW1BLUE", 64, 128
		{
			Patch "COMP03_1", 0, 0
			Patch "COMP03_2", 0, 64
			Patch "SW2_7", 14, 66
		}

		WallTexture "SW1BRCOM", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S0", 48, 72
		}

		WallTexture "SW1BRIK", 64, 128
		{
			Patch "RW23_4", 0, 0
			Patch "SW1S0", 16, 72
		}

		WallTexture "SW1BRN1", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S0", 48, 72
		}

		WallTexture "SW1BRN2", 64, 128
		{
			Patch "WALL02_2", 0, 56
			Patch "WALL02_2", 0, 0
			Patch "SW4S0", 20, 79
		}

		WallTexture "SW1BRNGN", 64, 128
		{
			Patch "WALL62_2", 0, 0
			Patch "SW4S0", 20, 80
		}

		WallTexture "SW1BROWN", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW3S1", 48, 72
		}

		WallTexture "SW1CMT", 64, 128
		{
			Patch "WALL54_1", -42, 0
			Patch "SW3S1", 16, 72
		}

		WallTexture "SW1COMM", 64, 72
		{
			Patch "W13_1", 0, 0
			Patch "SW1S0", 15, 18
		}

		WallTexture "SW1COMP", 64, 128
		{
			Patch "COMP03_4", 0, 64
			Patch "COMP04_5", 0, 0
			Patch "COMP03_4", 32, 64
			Patch "SW2S0", 16, 72
		}

		WallTexture "SW1DIRT", 64, 128
		{
			Patch "WALL00_7", 32, 0
			Patch "WALL00_6", 16, 0
			Patch "SW1S0", 16, 20
			Patch "WALL00_6", 0, -16
			Patch "WALL00_7", 48, 0
		}

		WallTexture "SW1EXIT", 32, 72
		{
			Patch "W32_4", 0, 0
			Patch "SW2S0", 0, 16
			Patch "W33_8", 0, 64
		}

		WallTexture "SW1GARG", 64, 128
		{
			Patch "WALL47_2", 0, 0
			Patch "WALL42_6", 12, 62
		}

		WallTexture "SW1GRAY", 64, 128
		{
			Patch "W31_1", 0, 0
			Patch "W31_1", 0, 64
			Patch "SW2S0", 16, 70
		}

		WallTexture "SW1GRAY1", 64, 128
		{
			Patch "W31_1", 0, 64
			Patch "W31_1", 0, 0
			Patch "SW4S0", 19, 79
		}

		WallTexture "SW1GSTON", 64, 128
		{
			Patch "WALL48_2", 0, 0
			Patch "SW2_7", 13, 67
		}

		WallTexture "SW1HOT", 64, 128
		{
			Patch "WALL49_1", 0, 0
			Patch "SW2_7", 12, 66
		}

		WallTexture "SW1LION", 64, 128
		{
			Patch "WALL47_2", 0, 0
			Patch "WALL42_5", 11, 62
		}

		WallTexture "SW1MARB", 64, 128
		{
			Patch "MWALL1_1", 0, 0
			Patch "SW2_7", 13, 55
		}

		WallTexture "SW1MET2", 64, 128
		{
			Patch "RW33_1", 0, 0
			Patch "SW1S0", 16, 20
		}

		WallTexture "SW1METAL", 64, 128
		{
			Patch "WALL03_7", 0, 0
			Patch "WALL03_7", 0, 64
			Patch "SW4S0", 20, 68
		}

		WallTexture "SW1MOD1", 64, 128
		{
			Patch "RW31_1", 0, 0
			Patch "SW4S0", 20, 80
		}

		WallTexture "SW1PANEL", 64, 128
		{
			Patch "RW21_4", 0, 0
			Patch "SW2_7", 14, 64
		}

		WallTexture "SW1PIPE", 128, 128
		{
			Patch "TP2_2", 0, 0
			Patch "SW3S0", 48, 76
		}

		WallTexture "SW1ROCK", 64, 128
		{
			Patch "RW30_1", 0, 0
			Patch "SW2_7", 14, 66
		}

		WallTexture "SW1SATYR", 64, 128
		{
			Patch "WALL47_2", 0, 0
			Patch "WALL42_1", 12, 62
		}

		WallTexture "SW1SKIN", 64, 128
		{
			Patch "HELL6_3", 0, 0
			Patch "SW2_5", 0, 59
		}

		WallTexture "SW1SKULL", 64, 128
		{
			Patch "RW48_4", 0, 0
		}

		WallTexture "SW1SLAD", 64, 128
		{
			Patch "WLA128_1", 0, 0
			Patch "WARNB0", 24, 73
		}

		WallTexture "SW1STARG", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S0", 48, 72
		}

		WallTexture "SW1STON1", 64, 128
		{
			Patch "W28_8", 0, 64
			Patch "W28_8", 0, 0
			Patch "SW1S0", 16, 78
		}

		WallTexture "SW1STON2", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S0", 48, 72
		}

		WallTexture "SW1STON6", 64, 128
		{
			Patch "RW18_3", 0, 0
			Patch "SW2S0", 16, 72
		}

		WallTexture "SW1STONE", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S0", 48, 72
		}

		WallTexture "SW1STRTN", 64, 128
		{
			Patch "SW12_4", 0, 0
			Patch "SW12_5", 32, 0
			Patch "SW1S0", 16, 72
		}

		WallTexture "SW1TEK", 64, 128
		{
			Patch "RW37_2", 0, 0
			Patch "SW4S0", 20, 79
		}

		WallTexture "SW1VINE", 64, 128
		{
			Patch "WALL00_1", 0, -16
			Patch "W106_1", 0, 0
			Patch "SW4S0", 20, 84
		}

		WallTexture "SW1WDMET", 64, 128
		{
			Patch "RW26_1", 0, 0
			Patch "EXIT2", 24, 48
			Patch "EXIT2", 32, 64
			Patch "EXIT2", 32, 48
			Patch "EXIT2", 24, 64
			Patch "W108_2", 24, 52
			Patch "EXIT2", 40, 48
			Patch "EXIT2", 40, 64
			Patch "EXIT2", 16, 48
			Patch "EXIT2", 16, 64
		}

		WallTexture "SW1WOOD", 64, 128
		{
			Patch "WALL40_2", -64, 0
			Patch "SW2_7", 14, 66
		}

		WallTexture "SW1ZIM", 64, 128
		{
			Patch "RW20_1", 0, 0
			Patch "SW2_7", 16, 66
		}

		WallTexture "SW2BLUE", 64, 128
		{
			Patch "COMP03_1", 0, 0
			Patch "COMP03_2", 0, 64
			Patch "SW2_8", 14, 66
		}

		WallTexture "SW2BRCOM", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S1", 48, 72
		}

		WallTexture "SW2BRIK", 64, 128
		{
			Patch "RW23_4", 0, 0
			Patch "SW1S1", 16, 72
		}

		WallTexture "SW2BRN1", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S1", 48, 72
		}

		WallTexture "SW2BRN2", 64, 128
		{
			Patch "WALL02_2", 0, 56
			Patch "WALL02_2", 0, 0
			Patch "SW4S1", 20, 79
		}

		WallTexture "SW2BRNGN", 64, 128
		{
			Patch "WALL62_2", 0, 0
			Patch "SW4S1", 20, 80
		}

		WallTexture "SW2BROWN", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW3S0", 48, 72
		}

		WallTexture "SW2CMT", 64, 128
		{
			Patch "WALL54_1", -42, 0
			Patch "SW3S0", 16, 72
		}

		WallTexture "SW2COMM", 64, 72
		{
			Patch "W13_1", 0, 0
			Patch "SW1S1", 15, 18
		}

		WallTexture "SW2COMP", 64, 128
		{
			Patch "COMP03_4", 0, 64
			Patch "COMP04_5", 0, 0
			Patch "COMP03_4", 32, 64
			Patch "SW2S1", 16, 72
		}

		WallTexture "SW2DIRT", 64, 128
		{
			Patch "WALL00_8", 48, 0
			Patch "WALL00_7", 32, 0
			Patch "WALL00_6", 16, 0
			Patch "WALL00_5", 0, -1
			Patch "SW1S1", 16, 20
		}

		WallTexture "SW2EXIT", 32, 72
		{
			Patch "W32_4", 0, 0
			Patch "SW2S1", 0, 16
			Patch "W33_8", 0, 64
		}

		WallTexture "SW2GARG", 64, 128
		{
			Patch "WALL47_2", 0, 0
			Patch "WALL47_5", 12, 62
		}

		WallTexture "SW2GRAY", 64, 128
		{
			Patch "W31_1", 0, 0
			Patch "W31_1", 0, 64
			Patch "SW2S1", 16, 70
		}

		WallTexture "SW2GRAY1", 64, 128
		{
			Patch "W31_1", 0, 64
			Patch "W31_1", 0, 0
			Patch "SW4S1", 19, 79
		}

		WallTexture "SW2GSTON", 64, 128
		{
			Patch "WALL48_2", 0, 0
			Patch "SW2_8", 13, 67
		}

		WallTexture "SW2HOT", 64, 128
		{
			Patch "WALL49_1", 0, 0
			Patch "SW2_8", 12, 66
		}

		WallTexture "SW2LION", 64, 128
		{
			Patch "WALL47_2", 0, 0
			Patch "WALL47_4", 11, 62
		}

		WallTexture "SW2MARB", 64, 128
		{
			Patch "MWALL1_1", 0, 0
			Patch "SW2_8", 13, 55
		}

		WallTexture "SW2MET2", 64, 128
		{
			Patch "RW33_1", 0, 0
			Patch "SW1S1", 16, 20
		}

		WallTexture "SW2METAL", 64, 128
		{
			Patch "WALL03_7", 0, 0
			Patch "WALL03_7", 0, 64
			Patch "SW4S1", 20, 68
		}

		WallTexture "SW2MOD1", 64, 128
		{
			Patch "RW31_1", 0, 0
			Patch "SW4S1", 20, 80
		}

		WallTexture "SW2PANEL", 64, 128
		{
			Patch "RW21_4", 0, 0
			Patch "SW2_8", 14, 64
		}

		WallTexture "SW2PIPE", 128, 128
		{
			Patch "TP2_2", 0, 0
			Patch "SW3S1", 48, 76
		}

		WallTexture "SW2ROCK", 64, 128
		{
			Patch "RW30_1", 0, 0
			Patch "SW2_8", 14, 66
		}

		WallTexture "SW2SATYR", 64, 128
		{
			Patch "WALL47_2", 0, 0
			Patch "WALL47_3", 12, 62
		}

		WallTexture "SW2SKIN", 64, 128
		{
			Patch "HELL6_3", 0, 0
			Patch "SW2_6", 0, 59
		}

		WallTexture "SW2SKULL", 64, 128
		{
			Patch "RW48_2", 0, 0
		}

		WallTexture "SW2SLAD", 64, 128
		{
			Patch "WLA128_1", 0, 0
			Patch "WARNA0", 24, 73
		}

		WallTexture "SW2STARG", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S1", 48, 72
		}

		WallTexture "SW2STON1", 64, 128
		{
			Patch "W28_8", 0, 64
			Patch "W28_8", 0, 0
			Patch "SW1S1", 16, 78
		}

		WallTexture "SW2STON2", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S1", 48, 72
		}

		WallTexture "SW2STON6", 64, 128
		{
			Patch "RW18_3", 0, 0
			Patch "SW2S1", 16, 72
		}

		WallTexture "SW2STONE", 128, 128
		{
			Patch "WALL62_1", 0, 0
			Patch "SW1S1", 48, 72
		}

		WallTexture "SW2STRTN", 64, 128
		{
			Patch "SW12_4", 0, 0
			Patch "SW12_5", 32, 0
			Patch "SW1S1", 16, 72
		}

		WallTexture "SW2TEK", 64, 128
		{
			Patch "RW37_2", 0, 0
			Patch "SW4S1", 20, 79
		}

		WallTexture "SW2VINE", 64, 128
		{
			Patch "WALL00_1", 0, -16
			Patch "W106_1", 0, 0
			Patch "SW4S1", 20, 84
		}

		WallTexture "SW2WDMET", 64, 128
		{
			Patch "RW26_1", 0, 0
			Patch "EXIT2", 24, 48
			Patch "EXIT2", 32, 64
			Patch "EXIT2", 32, 48
			Patch "EXIT2", 24, 64
			Patch "EXIT2", 40, 48
			Patch "EXIT2", 40, 64
			Patch "EXIT2", 16, 48
			Patch "EXIT2", 16, 64
			Patch "W108_3", 24, 52
		}

		WallTexture "SW2WOOD", 64, 128
		{
			Patch "WALL40_2", -64, 0
			Patch "SW2_8", 14, 66
		}

		WallTexture "SW2ZIM", 64, 128
		{
			Patch "RW20_1", 0, 0
			Patch "SW2_8", 16, 66
		}

		WallTexture "TANROCK2", 64, 128
		{
			Patch "RW11_2", 0, 0
		}

		WallTexture "TANROCK3", 64, 128
		{
			Patch "RW12_1", 0, 0
		}

		WallTexture "TANROCK4", 64, 128
		{
			Patch "RW12_4", 0, 0
		}

		WallTexture "TANROCK5", 64, 128
		{
			Patch "RW14_1", 0, 0
		}

		WallTexture "TANROCK7", 64, 128
		{
			Patch "RW23_2", 0, 0
		}

		WallTexture "TANROCK8", 64, 128
		{
			Patch "RW28_3", 0, 0
		}

		WallTexture "TEKBRON1", 128, 128
		{
			Patch "RW36_1", 0, 0
			Patch "RW36_3", 64, 0
		}

		WallTexture "TEKBRON2", 64, 128
		{
			Patch "RW39_2", 0, 0
		}

		WallTexture "TEKGREN1", 64, 128
		{
			Patch "RW37_1", 0, 0
		}

		WallTexture "TEKGREN2", 64, 128
		{
			Patch "RW37_2", 0, 0
		}

		WallTexture "TEKGREN3", 64, 128
		{
			Patch "RW37_3", 0, 0
		}

		WallTexture "TEKGREN4", 64, 128
		{
			Patch "RW37_4", 0, 0
		}

		WallTexture "TEKGREN5", 64, 128
		{
			Patch "RW39_1", 0, 0
		}

		WallTexture "TEKLITE", 64, 128
		{
			Patch "RW43_3", 0, 0
		}

		WallTexture "TEKLITE2", 64, 128
		{
			Patch "RW43_4", 0, 0
		}

		WallTexture "TEKWALL1", 128, 128
		{
			Patch "W17_1", 0, -27
			Patch "W17_1", 0, 16
		}

		WallTexture "TEKWALL4", 128, 128
		{
			Patch "W94_1", 0, 0
		}

		WallTexture "TEKWALL6", 256, 128
		{
			Patch "RW25_1", 0, 0
			Patch "RW25_2", 128, 0
			Patch "RW25_3", 64, 0
			Patch "RW25_4", 192, 0
		}

		WallTexture "WOOD1", 256, 128
		{
			Patch "WALL40_1", 128, 0
			Patch "WALL40_2", 0, 0
		}

		WallTexture "WOOD10", 128, 128
		{
			Patch "RW13_1", 0, 0
		}

		WallTexture "WOOD12", 64, 128
		{
			Patch "RW41_2", 0, 0
		}

		WallTexture "WOOD3", 256, 128
		{
			Patch "WALL97_1", -4, 0
			Patch "WALL97_2", 60, 0
			Patch "WALL97_3", 124, 0
			Patch "WALL97_2", 188, 0
			Patch "WALL97_1", 252, 0
		}

		WallTexture "WOOD4", 64, 128
		{
			Patch "WALL97_2", -4, 0
			Patch "SW2_2", 22, 18
			Patch "SW2_1", 22, 76
			Patch "WALL97_2", 60, 0
		}

		WallTexture "WOOD5", 256, 128
		{
			Patch "W96_1", -4, 0
			Patch "W96_2", 124, 0
			Patch "W96_1", 252, 0
		}

		WallTexture "WOOD6", 64, 128
		{
			Patch "RW9_1", 0, 0
		}

		WallTexture "WOOD7", 64, 128
		{
			Patch "RW9_2", 0, 0
		}

		WallTexture "WOOD8", 64, 128
		{
			Patch "RW9_3", 0, 0
		}

		WallTexture "WOOD9", 64, 128
		{
			Patch "RW9_4", 0, 0
		}

		WallTexture "WOODGARG", 64, 128
		{
			Patch "WALL40_1", 0, 0
			Patch "SW2_4", 6, 63
		}

		WallTexture "WOODMET1", 64, 128
		{
			Patch "RW26_1", 0, 0
		}

		WallTexture "WOODMET2", 64, 128
		{
			Patch "RW26_2", 0, 0
		}

		WallTexture "WOODMET3", 64, 128
		{
			Patch "RW26_3", 0, 0
		}

		WallTexture "WOODMET4", 64, 128
		{
			Patch "RW26_4", 0, 0
		}

		WallTexture "WOODVERT", 64, 128
		{
			Patch "RW23_1", 0, 0
		}

		WallTexture "ZDOORB1", 128, 128
		{
			Patch "WOLF18", 0, 0
		}

		WallTexture "ZDOORF1", 128, 128
		{
			Patch "WOLF10", 0, 0
		}

		WallTexture "ZELDOOR", 128, 128
		{
			Patch "WOLF11", 0, 0
		}

		WallTexture "ZIMMER1", 64, 128
		{
			Patch "RW20_1", 0, 0
		}

		WallTexture "ZIMMER2", 64, 128
		{
			Patch "RW20_2", 0, 0
		}

		WallTexture "ZIMMER3", 64, 128
		{
			Patch "RW20_3", 0, 0
		}

		WallTexture "ZIMMER4", 64, 128
		{
			Patch "RW20_4", 0, 0
		}

		WallTexture "ZIMMER5", 64, 128
		{
			Patch "RW42_1", 0, 0
		}

		WallTexture "ZIMMER7", 64, 128
		{
			Patch "RW42_3", 0, 0
		}

		WallTexture "ZIMMER8", 64, 128
		{
			Patch "RW42_4", 0, 0
		}

		WallTexture "ZZWOLF1", 128, 128
		{
			Patch "WOLF1", 0, 0
		}

		WallTexture "ZZWOLF10", 128, 128
		{
			Patch "WOLF12", 0, 0
		}

		WallTexture "ZZWOLF11", 128, 128
		{
			Patch "WOLF13", 0, 0
		}

		WallTexture "ZZWOLF12", 128, 128
		{
			Patch "WOLF14", 0, 0
		}

		WallTexture "ZZWOLF13", 128, 128
		{
			Patch "WOLF17", 0, 0
		}

		WallTexture "ZZWOLF2", 128, 128
		{
			Patch "WOLF2", 0, 0
		}

		WallTexture "ZZWOLF3", 128, 128
		{
			Patch "WOLF3", 0, 0
		}

		WallTexture "ZZWOLF4", 128, 128
		{
			Patch "WOLF4", 0, 0
		}

		WallTexture "ZZWOLF5", 128, 128
		{
			Patch "WOLF5", 0, 0
		}

		WallTexture "ZZWOLF6", 128, 128
		{
			Patch "WOLF6", 0, 0
		}

		WallTexture "ZZWOLF7", 128, 128
		{
			Patch "WOLF7", 0, 0
		}

		WallTexture "ZZWOLF9", 128, 128
		{
			Patch "WOLF9", 0, 0
		}

		WallTexture "ZZZFACE1", 256, 128
		{
			Patch "RWDMON4", 0, 0
		}

		WallTexture "ZZZFACE2", 256, 128
		{
			Patch "RWDMON5", 0, 0
		}

		WallTexture "ZZZFACE3", 256, 128
		{
			Patch "RWDMON3", 0, 0
		}

		WallTexture "ZZZFACE4", 256, 128
		{
			Patch "RWDMON2", 0, 0
		}

		WallTexture "ZZZFACE5", 256, 128
		{
			Patch "RWDMON1", 0, 0
		}

		WallTexture "ZZZFACE6", 256, 128
		{
			Patch "RWDMON7", 0, 0
		}

		WallTexture "ZZZFACE7", 256, 128
		{
			Patch "RWDMON8", 0, 0
		}

		WallTexture "ZZZFACE8", 256, 128
		{
			Patch "RWDMON9", 0, 0
		}

		WallTexture "ZZZFACE9", 256, 128
		{
			Patch "RWDMON10", 0, 0
		}
		""";
}
