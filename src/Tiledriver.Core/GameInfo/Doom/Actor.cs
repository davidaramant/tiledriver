using Tiledriver.Core.FormatModels.Udmf;

namespace Tiledriver.Core.GameInfo.Doom;

public sealed partial record Actor(
	int Id,
	string Description,
	int Width,
	int Height,
	string ClassName,
	ActorCategory Category
)
{
	public Thing MakeThing(double x, double y, int angle) =>
		new(
			X: x,
			Y: y,
			Type: Id,
			Angle: angle,
			Skill1: true,
			Skill2: true,
			Skill3: true,
			Skill4: true,
			Skill5: true,
			Ambush: false,
			Single: true,
			Dm: true,
			Coop: true
		);
}
