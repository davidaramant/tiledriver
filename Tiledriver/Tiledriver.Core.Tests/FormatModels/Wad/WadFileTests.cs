// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.FormatModels.Uwmf.Parsing.Syntax;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
    [TestFixture]
    public sealed class WadFileTests
    {
        [Test]
        public void ShouldCreateWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UwmfLump("TEXTMAP", DemoMap.Create()));
                wad.Append(new Marker("ENDMAP"));
                wad.SaveTo(fileInfo.FullName);
            }
            finally
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }

        [Test]
        public void ShouldReadCreatedWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var map = DemoMap.Create();

                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UwmfLump("TEXTMAP", map));
                wad.Append(new Marker("ENDMAP"));
                wad.SaveTo(fileInfo.FullName);

                wad = WadFile.Read(fileInfo.FullName);
                Assert.That(wad.Count, Is.EqualTo(3), "Did not return correct count.");
                Assert.That(
                    wad.Select(l => l.Name).ToArray(),
                    Is.EquivalentTo(new[] { new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP"), }),
                    "Did not return correct lump names.");

                var mapBytes = wad[1].GetData();
                using (var ms = new MemoryStream(mapBytes))
                using (var textReader = new StreamReader(ms, Encoding.ASCII))
                {
                    var sa = new SyntaxAnalyzer();
                    var roundTripped = Parser.Parse(sa.Analyze(new UwmfLexer(textReader)));

                    UwmfComparison.AssertEqual(roundTripped, map);
                }
            }
            finally
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }
    }
}
