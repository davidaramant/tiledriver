// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Tiledriver.Core.FormatModels.Uwmf.Writing
{
    public static partial class UwmfWriter
    {
        public static void WriteTo(this MapData map, Stream stream)
        {
            using var writer = new StreamWriter(stream, Encoding.ASCII, leaveOpen: true);

            Write(writer, map);
        }

        private static void Write(this StreamWriter writer, PlaneMap planeMap)
        {
            string Convert(TileSpace ts)
            {
                var tagPortion =
                    ts.Tag != 0 ?
                        $",{ts.Tag}" :
                        string.Empty;

                return $"\t{{{ts.Tile},{ts.Sector},{ts.Zone}{tagPortion}}}";
            }

            writer.WriteLine("planeMap");
            writer.WriteLine("{");
            writer.WriteLine(string.Join("," + Environment.NewLine, planeMap.TileSpaces.Select(Convert)));
            writer.WriteLine("}");
        }

        private static void WriteProperty(StreamWriter writer, string name, string value, string? defaultValue = null, bool indent = true)
        {
            if (value != defaultValue)
            {
                if (indent)
                {
                    writer.Write('\t');
                }
                writer.WriteLine($"{name} = \"{value}\";");
            }
        }

        private static void WriteProperty(StreamWriter writer, string name, bool value, bool? defaultValue = null, bool indent = true)
        {
            if (value != defaultValue)
            {
                if (indent)
                {
                    writer.Write('\t');
                }
                writer.WriteLine($"{name} = {value.ToString().ToLowerInvariant()};");
            }
        }

        private static void WriteProperty(StreamWriter writer, string name, int value, int? defaultValue = null, bool indent = true)
        {
            if (value != defaultValue)
            {
                if (indent)
                {
                    writer.Write('\t');
                }
            
                writer.WriteLine($"{name} = {value};");
            }
        }

        private static void WriteProperty(StreamWriter writer, string name, double value, bool indent = true)
        {
            if (indent)
            {
                writer.Write('\t');
            }

            // There are no double properties with default values, so we avoid having to compare them
            writer.WriteLine($"{name} = {value};");
        }
    }
}