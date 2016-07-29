// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.UwmfMetadata
{
    public sealed class UwmfBlock : NamedItem
    {
        private readonly List<UwmfProperty> _properties = new List<UwmfProperty>();
        private readonly List<NamedItem> _subBlocks = new List<NamedItem>();

        public IEnumerable<UwmfProperty> Properties => _properties;
        public IEnumerable<NamedItem> SubBlocks => _subBlocks;
        public bool IsSubBlock { get; private set; } = true;
        public bool NormalWriting { get; private set; } = true;
        public bool NormalReading { get; private set; } = true;
        public bool CanHaveUnknownProperties { get; private set; } = true;
        public bool CanHaveUnknownBlocks => !IsSubBlock;

        public UwmfBlock(string name) : base(name,name)
        {
        }

        public UwmfBlock HasSubBlocks(params string[] names)
        {
            _subBlocks.AddRange(names.Select(_ => new NamedItem(_,_)));
            return this;
        }

        public UwmfBlock IsTopLevel()
        {
            IsSubBlock = false;
            return this;
        }

        public UwmfBlock DisableNormalWriting()
        {
            NormalWriting = false;
            return this;
        }

        public UwmfBlock DisableNormalReading()
        {
            NormalReading = false;
            return this;
        }

        public UwmfBlock CannotHaveUnknownProperties()
        {
            CanHaveUnknownProperties = false;
            return this;
        }

        public UwmfBlock HasRequiredIntegerNumber(string name)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.IntegerNumber, defaultValue: null));
            return this;
        }

        public UwmfBlock HasRequiredFloatingPointNumber(string name)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.FloatingPointNumber, defaultValue: null));
            return this;
        }

        public UwmfBlock HasRequiredString(string name, string uwmfName = null)
        {
            // 'namespace' is currently the only name that needs special handling.
            _properties.Add(new UwmfProperty(name, uwmfName ?? name, PropertyType.String, defaultValue: null));
            return this;
        }

        public UwmfBlock HasRequiredBoolean(string name)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.Boolean, defaultValue: null));
            return this;
        }

        public UwmfBlock HasOptionalIntegerNumber(string name, int defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.IntegerNumber, defaultValue: defaultValue));
            return this;
        }

        public UwmfBlock HasOptionalFloatingPointNumber(string name, double defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.FloatingPointNumber, defaultValue: defaultValue));
            return this;
        }

        public UwmfBlock HasOptionalString(string name, string defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.String, defaultValue: defaultValue));
            return this;
        }

        public UwmfBlock HasOptionalBoolean(string name, bool defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.Boolean, defaultValue: defaultValue));
            return this;
        }
    }
}