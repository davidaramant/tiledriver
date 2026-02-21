namespace Tiledriver.DataModelGenerator.DoomGameInfo;

sealed record Actor(string Name, string CategoryName, int Id, string Description, int Radius, int Height)
{
	public int Width => Radius * 2;
	public string SafeName => Name.TrimStart('$');
}
