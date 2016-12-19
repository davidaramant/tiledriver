// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Metadata
{
    public sealed class BlockData : NamedItem
    {
        private readonly List<PropertyData> _properties = new List<PropertyData>();

        public IEnumerable<PropertyData> Properties => _properties;
        public bool IsSubBlock { get; private set; } = true;
        public bool NormalWriting { get; private set; } = true;
        public bool NormalReading { get; private set; } = true;
        public bool SupportsUnknownProperties { get; private set; } = false;
        public bool SupportsUnknownBlocks { get; private set; } = false;

        public IEnumerable<PropertyData> OrderedProperties()
        {
            return Properties.Where(p => p.IsRequired).Concat(Properties.Where(p => !p.IsRequired));
        }

        public BlockData(string name) : base(name,name)
        {
        }

        public BlockData(string xlatName, string className) : base(xlatName, className)
        {
        }

        public BlockData HasSubBlockLists(params string[] names)
        {
            _properties.AddRange(names.Select(name=>new PropertyData(name, name, PropertyType.BlockList, defaultValue: null)));
            return this;
        }

        public BlockData HasRequiredUshortSet(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.UshortSet, defaultValue: null));
            return this;
        }

        public BlockData HasRequiredStringList(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.StringList, defaultValue: null));
            return this;
        }

        public BlockData HasMappedSubBlocks(params string[] names)
        {
            _properties.AddRange(names.Select(name => new PropertyData(name, name, PropertyType.MappedBlockList, defaultValue: null)));
            return this;
        }

        public BlockData HasSubBlock(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Block, defaultValue: null));
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

        public BlockData CanHaveUnknownProperties()
        {
            SupportsUnknownProperties = true;
            _properties.Add(new PropertyData("unknownProperties", "unknownProperties", PropertyType.UnknownProperties, defaultValue: "null"));
            return this;
        }

        public BlockData CanHaveUnknownBlocks()
        {
            SupportsUnknownBlocks = true;
            _properties.Add(new PropertyData("unknownBlocks", "unknownBlocks", PropertyType.UnknownBlocks, defaultValue: "null"));
            return this;
        }

        public BlockData HasRequiredInteger(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Integer, defaultValue: null));
            return this;
        }

        public BlockData HasRequiredUshort(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Ushort, defaultValue: null));
            return this;
        }

        public BlockData HasRequiredDouble(string name)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Double, defaultValue: null));
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

        public BlockData HasOptionalInteger(string name, int defaultValue)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Integer, defaultValue: defaultValue));
            return this;
        }

        public BlockData HasOptionalDouble(string name, double defaultValue)
        {
            _properties.Add(new PropertyData(name, name, PropertyType.Double, defaultValue: defaultValue));
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