// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Globalization;
using System.Linq;
using Tiledriver.Core.Extensions;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public static class Extensions
    {
        public static void AssertMetadataLength(this MapInfoBlock block, int length, string context)
        {
            if (block.Metadata.Length != length)
            {
                throw new ParsingException($"Expected {length} pieces of metadata for {context} but found {block.Metadata.Length}.");
            }
        }

        public static void AssertValuesLength(this MapInfoProperty property, int length, string context)
        {
            if (property.Values.Length != length)
            {
                throw new ParsingException($"Expected {length} values for {context} but found {property.Values.Length}.");
            }
        }

        public static MapInfoProperty AssertAsProperty(this IMapInfoElement element, string context)
        {
            if(element.IsBlock)
                throw new ParsingException($"Expecting a property for {context} but found a block.");

            return element.AsProperty();
        }

        public static MapInfoBlock AssertAsBlock(this IMapInfoElement element, string context)
        {
            if (!element.IsBlock)
                throw new ParsingException($"Expecting a block for {context} but found a property.");

            return element.AsBlock();
        }
    }
}