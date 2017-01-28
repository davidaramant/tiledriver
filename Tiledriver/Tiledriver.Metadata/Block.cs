// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Functional.Maybe;

namespace Tiledriver.Metadata
{
    public enum Parsing
    {
        Normal,
        Manual,
        InOrder
    }

    [DebuggerDisplay("{" + nameof(ClassName) + "}")]
    public sealed class Block : NamedItem
    {
        private readonly List<Property> _properties = new List<Property>();

        public IEnumerable<Property> Properties => _properties;
        public bool IsSubBlock { get; }
        public Parsing ParsingMode { get; }
        public bool NormalWriting { get; }
        public bool NormalParsing => ParsingMode == Parsing.Normal;
        public bool IsAbstract { get; }
        public IEnumerable<string> SetsPropertiesFrom { get; }
        public IEnumerable<string> PropertyFallbacksFrom { get; }
        public Maybe<string> BaseClass { get; }
        public IEnumerable<string> ImplementedInterfaces { get; }
        public bool SupportsUnknownProperties => Properties.Any(p => p.Type == PropertyType.UnknownProperties);
        public bool SupportsUnknownBlocks => Properties.Any(p => p.Type == PropertyType.UnknownBlocks);

        public IEnumerable<Property> OrderedProperties()
        {
            return Properties.Where(p => p.IsRequired).Concat(Properties.Where(p => !p.IsRequired));
        }

        public Block(
            string formatName,
            IEnumerable<Property> properties,
            string className = null,
            bool isSubBlock = true,
            bool normalWriting = true,
            Parsing parsing = Parsing.Normal,
            string inheritsFrom = null,
            bool isAbstract = false,
            IEnumerable<string> implements = null,
            IEnumerable<string> canSetPropertiesFrom = null,
            IEnumerable<string> propertyFallbacksFrom = null ) :
            base(formatName, className ?? formatName)
        {
            IsSubBlock = isSubBlock;
            NormalWriting = normalWriting;
            ParsingMode = parsing;
            IsAbstract = isAbstract;
            SetsPropertiesFrom = canSetPropertiesFrom ?? Enumerable.Empty<string>();
            PropertyFallbacksFrom = propertyFallbacksFrom ?? Enumerable.Empty<string>();
            BaseClass = inheritsFrom.ToMaybe();
            ImplementedInterfaces = implements ?? Enumerable.Empty<string>();

            _properties.AddRange(properties);
        }
    }
}