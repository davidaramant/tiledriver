using System;
using System.CodeDom.Compiler;

#nullable enable
namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial class ThingBuilder
{
	double? X { get; set; }
	double? Y { get; set; }
	int? Angle { get; set; }
	int? Type { get; set; }
	int Id { get; set; } = 0;
	double Height { get; set; } = 0;
	bool Skill1 { get; set; } = false;
	bool Skill2 { get; set; } = false;
	bool Skill3 { get; set; } = false;
	bool Skill4 { get; set; } = false;
	bool Skill5 { get; set; } = false;
	bool Single { get; set; } = false;
	bool Coop { get; set; } = false;
	bool Dm { get; set; } = false;
	bool Ambush { get; set; } = false;
	string Comment { get; set; } = "";

	public Thing Build() =>
		new(
			X: X ?? throw new ArgumentNullException("X must have a value assigned."),
			Y: Y ?? throw new ArgumentNullException("Y must have a value assigned."),
			Angle: Angle ?? throw new ArgumentNullException("Angle must have a value assigned."),
			Type: Type ?? throw new ArgumentNullException("Type must have a value assigned."),
			Id: Id,
			Height: Height,
			Skill1: Skill1,
			Skill2: Skill2,
			Skill3: Skill3,
			Skill4: Skill4,
			Skill5: Skill5,
			Single: Single,
			Coop: Coop,
			Dm: Dm,
			Ambush: Ambush,
			Comment: Comment
		);
}
