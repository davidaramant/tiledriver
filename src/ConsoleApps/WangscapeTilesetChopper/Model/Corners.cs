namespace WangscapeTilesetChopper.Model;

[Flags]
enum Corners : byte
{
	None = 0,
	BottomLeft = 0b0001,
	BottomRight = 0b0010,
	TopRight = 0b0100,
	TopLeft = 0b1000,
}
