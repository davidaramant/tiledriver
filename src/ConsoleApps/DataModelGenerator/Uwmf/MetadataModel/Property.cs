// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    abstract class Property
    {
        public string Name { get; }
        public virtual string FormatName => Name;
        public abstract string CodeName { get; }
        public abstract string CodeType { get; }
        public virtual string? DefaultString => null;
        protected Property(string name) => Name = name;
    }
}