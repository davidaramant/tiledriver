namespace Tiledriver.Core.FormatModels.Udmf;

public sealed partial record MapData
{
	public double MinX => Vertices.Min(p => p.X);
	public double MaxX => Vertices.Max(p => p.X);
	public double MinY => Vertices.Min(p => p.Y);
	public double MaxY => Vertices.Max(p => p.Y);
	public double Width => MaxX - MinX;
	public double Height => MaxY - MinY;
}
