using System.Drawing;
using SkiaSharp;
using Tiledriver.Extensions.Colors;
using static System.Math;

namespace Tiledriver.Rendering;

public enum LineMode
{
	Exact,
	Smooth,
}

public static class PixelBufferExtensions
{
	public static void SetColor(this IPixelBuffer buffer, SKPointI p, SKColor c) => buffer.SetColor(p.X, p.Y, c);

	public static void AddColor(this IPixelBuffer buffer, SKPointI p, SKColor c) => buffer.AddColor(p.X, p.Y, c);

	public static void DrawLine(this IPixelBuffer buffer, SKPointI p0, SKPointI p1, SKColor color, LineMode mode) =>
		DrawLine(buffer, p0.X, p0.Y, p1.X, p1.Y, color, mode);

	public static void DrawLine(this IPixelBuffer buffer, int x0, int y0, int x1, int y1, SKColor color, LineMode mode)
	{
		switch (mode)
		{
			case LineMode.Exact:
				DrawLine(buffer, x0, y0, x1, y1, color);
				break;
			case LineMode.Smooth:
				DrawLineSmooth(buffer, x0, y0, x1, y1, color);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
		}
	}

	#region Bresenham's Line Algorithm

	public static void DrawLine(this IPixelBuffer buffer, SKPointI p0, SKPointI p1, SKColor color) =>
		DrawLine(buffer, p0.X, p0.Y, p1.X, p1.Y, color);

	public static void DrawLine(this IPixelBuffer buffer, int x0, int y0, int x1, int y1, SKColor color)
	{
		if (Abs(y1 - y0) < Abs(x1 - x0))
		{
			if (x0 > x1)
				DrawLineLow(buffer, x1, y1, x0, y0, color);
			else
				DrawLineLow(buffer, x0, y0, x1, y1, color);
		}
		else
		{
			if (y0 > y1)
				DrawLineHigh(buffer, x1, y1, x0, y0, color);
			else
				DrawLineHigh(buffer, x0, y0, x1, y1, color);
		}
	}

	private static void DrawLineLow(IPixelBuffer buffer, int x0, int y0, int x1, int y1, SKColor color)
	{
		int dx = x1 - x0;
		int dy = y1 - y0;
		int yi = 1;
		if (dy < 0)
		{
			yi = -1;
			dy = -dy;
		}

		int D = 2 * dy - dx;
		int y = y0;

		for (int x = x0; x <= x1; x++)
		{
			buffer.SetColor(x, y, color);

			if (D > 0)
			{
				y = y + yi;
				D = D - 2 * dx;
			}
			D = D + 2 * dy;
		}
	}

	private static void DrawLineHigh(IPixelBuffer buffer, int x0, int y0, int x1, int y1, SKColor color)
	{
		int dx = x1 - x0;
		int dy = y1 - y0;
		int xi = 1;
		if (dx < 0)
		{
			xi = -1;
			dx = -dx;
		}

		int D = 2 * dx - dy;
		int x = x0;

		for (int y = y0; y <= y1; y++)
		{
			buffer.SetColor(x, y, color);

			if (D > 0)
			{
				x = x + xi;
				D = D - 2 * dy;
			}
			D = D + 2 * dx;
		}
	}
	#endregion

	#region Bresenham's Circle Algorithm
	public static void DrawCircle(this IPixelBuffer buffer, SKPointI center, int radius, SKColor color) =>
		DrawCircle(buffer, center.X, center.Y, radius, color);

	public static void DrawCircle(this IPixelBuffer buffer, int xCenter, int yCenter, int radius, SKColor color)
	{
		int x = 0,
			y = radius;
		int d = 3 - 2 * radius;
		DrawCircleSegments(buffer, xCenter, yCenter, x, y, color);
		while (y >= x)
		{
			x++;

			if (d > 0)
			{
				y--;
				d = d + 4 * (x - y) + 10;
			}
			else
			{
				d = d + 4 * x + 6;
			}
			DrawCircleSegments(buffer, xCenter, yCenter, x, y, color);
		}
	}

	private static void DrawCircleSegments(IPixelBuffer buffer, int xc, int yc, int x, int y, SKColor color)
	{
		buffer.SetColor(xc + x, yc + y, color);
		buffer.SetColor(xc - x, yc + y, color);
		buffer.SetColor(xc + x, yc - y, color);
		buffer.SetColor(xc - x, yc - y, color);
		buffer.SetColor(xc + y, yc + x, color);
		buffer.SetColor(xc - y, yc + x, color);
		buffer.SetColor(xc + y, yc - x, color);
		buffer.SetColor(xc - y, yc - x, color);
	}
	#endregion

	#region Adapted version of Mike Abrash's Wu line drawer

	// HACK!!!! Keep this as global mutable state for convenience
	public static float GammaExponent = 2.5f;

	// Based on the version from his Graphics Programming Black Book http://www.jagregory.com/abrash-black-book/
	// The integer error stuff was removed in favor of floats.
	// It might have worked if I used 'unchecked' but I got frustrated debugging it.  FPUs are considerably faster these days anyway.
	public static void DrawLineSmooth(this IPixelBuffer buffer, Point p1, Point p2, SKColor baseColor) =>
		DrawLineSmooth(buffer, p1.X, p1.Y, p2.X, p2.Y, baseColor);

	public static void DrawLineSmooth(this IPixelBuffer buffer, int x0, int y0, int x1, int y1, SKColor baseColor)
	{
		float FractionalPart(float f) => f - (int)f;
		float ReciprocalOfFractionalPart(float f) => 1 - FractionalPart(f);

		float Gamma(float x, float exp) => (float)Pow(x, 1.0f / exp);
		void DrawPixel(int x, int y) => buffer.SetColor(x, y, baseColor);
		void DrawPixelScale(int x, int y, float intensity) =>
			buffer.AddColor(x, y, baseColor.Multiply(Gamma(intensity, GammaExponent)));

		// Make sure the line runs top to bottom
		if (y0 > y1)
		{
			(x0, x1) = (x1, x0);
			(y0, y1) = (y1, y0);
		}
		// Draw the initial pixel, which is always exactly intersected by the line and so needs no weighting
		DrawPixel(x0, y0);

		var deltaX = x1 - x0;
		var xDir = 1;

		if (deltaX < 0)
		{
			xDir = -1;
			deltaX = -deltaX; // make DeltaX positive
		}

		var deltaY = y1 - y0; // Guaranteed to be positive since we made sure it goes from top to bottom

		// Special cases for horizontal, vertical, and diagonal lines
		if (deltaY == 0)
		{
			while (deltaX-- != 0)
			{
				x0 += xDir;
				DrawPixel(x0, y0);
			}
			return;
		}
		if (deltaX == 0)
		{
			do
			{
				y0++;
				DrawPixel(x0, y0);
			} while (--deltaY != 0);
			return;
		}
		if (deltaX == deltaY)
		{
			do
			{
				x0 += xDir;
				y0++;
				DrawPixel(x0, y0);
			} while (--deltaY != 0);
			return;
		}

		// line is not horizontal, diagonal, or vertical
		float gradient = 0;
		float accumulatedError = 0;

		bool isYMajorLine = deltaY > deltaX;
		if (isYMajorLine)
		{
			gradient = (float)deltaX / deltaY;
			while (--deltaY != 0)
			{
				accumulatedError += gradient;
				y0++;

				DrawPixelScale(x0 + xDir * (int)accumulatedError, y0, ReciprocalOfFractionalPart(accumulatedError));
				DrawPixelScale(x0 + xDir * (int)accumulatedError + xDir, y0, FractionalPart(accumulatedError));
			}
		}
		else
		{
			gradient = (float)deltaY / deltaX;
			while (--deltaX != 0)
			{
				accumulatedError += gradient;
				x0 += xDir;

				DrawPixelScale(x0, y0 + (int)accumulatedError, ReciprocalOfFractionalPart(accumulatedError));
				DrawPixelScale(x0, y0 + (int)accumulatedError + 1, FractionalPart(accumulatedError));
			}
		}
		// Draw the final pixel, which is always exactly intersected by the line
		// and so needs no weighting
		DrawPixel(x1, y1);
	}

	#endregion
}
