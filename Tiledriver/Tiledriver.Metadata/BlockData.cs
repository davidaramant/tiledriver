// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;

namespace Tiledriver.Metadata
{
    public sealed class BlockData : NamedItem
    {
        private readonly List<PropertyData> _properties = new List<PropertyData>();

        public IEnumerable<PropertyData> Properties => _properties;
        public bool IsSubBlock { get; }
        public bool NormalWriting { get; }
        public bool NormalReading { get; }
        public bool IsAbstract { get; }
        public IEnumerable<string> SetsPropertiesFrom { get; }
        public Maybe<string> BaseClass { get; }
        public IEnumerable<string> ImplementedInterfaces { get; }
        public bool SupportsUnknownProperties => Properties.Any(p => p.Type == PropertyType.UnknownProperties);
        public bool SupportsUnknownBlocks => Properties.Any(p => p.Type == PropertyType.UnknownBlocks);

        public IEnumerable<PropertyData> OrderedProperties()
        {
            return Properties.Where(p => p.IsRequired).Concat(Properties.Where(p => !p.IsRequired));
        }

        public BlockData(
            string formatName,
            IEnumerable<PropertyData> properties,
            string className = null,
            bool isSubBlock = true,
            bool normalWriting = true,
            bool normalReading = true,
            string inheritsFrom = null,
            bool isAbstract = false,
            IEnumerable<string> implements = null,
            IEnumerable<string> canSetPropertiesFrom = null ) :
            base(formatName, className ?? formatName)
        {
            IsSubBlock = isSubBlock;
            NormalWriting = normalWriting;
            NormalReading = normalReading;
            IsAbstract = isAbstract;
            SetsPropertiesFrom = canSetPropertiesFrom ?? Enumerable.Empty<string>();
            BaseClass = inheritsFrom.ToMaybe();
            ImplementedInterfaces = implements ?? Enumerable.Empty<string>();

            _properties.AddRange(properties);
        }
    }
}