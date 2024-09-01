// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf.Writing;

public static partial class UdmfWriter
{
	public static void WriteTo(this MapData map, Stream stream)
	{
		using var writer = new StreamWriter(stream, Encoding.ASCII, leaveOpen: true) { NewLine = "\r\n" };

		Write(writer, map);
	}

	private static void WriteProperty(StreamWriter writer, string name, Texture value, Texture? defaultValue = null) =>
		WriteProperty(writer, name, value.Name, defaultValue: defaultValue?.Name);

	private static void WriteProperty(StreamWriter writer, string name, string value, string? defaultValue = null)
	{
		if (value != defaultValue)
		{
			writer.WriteLine($"{name} = \"{value}\";");
		}
	}

	private static void WriteProperty(StreamWriter writer, string name, bool value, bool? defaultValue = null)
	{
		if (value != defaultValue)
		{
			writer.WriteLine($"{name} = {value.ToString().ToLowerInvariant()};");
		}
	}

	private static void WriteProperty(StreamWriter writer, string name, int value, int? defaultValue = null)
	{
		if (value != defaultValue)
		{
			writer.WriteLine($"{name} = {value};");
		}
	}

	private static void WriteProperty(StreamWriter writer, string name, double value, double? defaultValue = null)
	{
		var diff = Math.Abs(value - defaultValue ?? 0);
		if (defaultValue is null || diff != 0)
		{
			writer.WriteLine($"{name} = {value:F1};");
		}
	}
}
