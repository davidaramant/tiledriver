// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using System.Text;

namespace Tiledriver.Core.FormatModels.Xlat.Reading
{
    public static class XlatReader
    {
        public static MapTranslation Read(Stream xlatStream, IResourceProvider resourceProvider)
        {
            using var textReader = new StreamReader(xlatStream, Encoding.ASCII);
            var lexer = XlatLexer.Create(textReader);
            return XlatParser.Parse(lexer.Scan(), resourceProvider);
        }
    }
}
