// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Metadata
{
    public sealed class BlockData : NamedItem
    {
        private readonly List<PropertyData> _properties = new List<PropertyData>();
        private readonly List<NamedItem> _subBlocks = new List<NamedItem>();

        public IEnumerable<PropertyData> Properties => _properties;
        public IEnumerable<NamedItem> SubBlocks => _subBlocks;
        public bool IsSubBlock { get; private set; } = true;
        public bool NormalWriting { get; private set; } = true;
        public bool NormalReading { get; private set; } = true;
        public bool CanHaveUnknownProperties { get; private set; } = true;
        public bool CanHaveUnknownBlocks => !IsSubBlock;

        public BlockData(string name) : base(name,name)
        {
        }

        public BlockData HasSubBlocks(params string[] names)
        {
            _subBlocks.AddRange(names.Select(_ => new NamedItem(_,_)));
            return this;
        }

        public BlockData IsTopLevel()
        {
            IsSubBlock = false;
            return this;
        }

        public BlockData DisableNormalWriting()
        {
            NormalWriting = false;
            return this;
        }

        public BlockData DisableNormalReading()
        {
            NormalReading = false;
            return this;
        }

        public BlockData CannotHaveUnknownProperties()
        {
            CanHaveUnknownProperties = false;
            return this;
        }

        public BlockData HasRequiredIntegerNumber(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.IntegerNumber, defaultValue: null));
            return this;
        }

        public BlockData HasRequiredFloatingPointNumber(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.FloatingPointNumber, defaultValue: null));
            return this;
        }

        public BlockData HasRequiredString(string name, string uwmfName = null)
        {
            // 'namespace' is currently the only name that needs special handling.
            _properties.Add(new PropertyData(name, uwmfName ?? name, PropertyType.String, defaultValue: null));
            return this;
        }

        public BlockData HasRequiredBoolean(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Boolean, defaultValue: null));
            return this;
        }

        public BlockData HasOptionalIntegerNumber(string name, int defaultValue)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.IntegerNumber, defaultValue: defaultValue));
            return this;
        }

        public BlockData HasOptionalFloatingPointNumber(string name, double defaultValue)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.FloatingPointNumber, defaultValue: defaultValue));
            return this;
        }

        public BlockData HasOptionalString(string name, string defaultValue)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.String, defaultValue: defaultValue));
            return this;
        }

        public BlockData HasOptionalBoolean(string name, bool defaultValue)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Boolean, defaultValue: defaultValue));
            return this;
        }
    }
}