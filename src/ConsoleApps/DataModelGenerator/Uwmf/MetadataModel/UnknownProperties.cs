// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    sealed class UnknownProperties : Property
    {
        public override string CodeName => "UnknownProperties";
        public override string CodeType => "ImmutableList<UnknownProperty>";

        public UnknownProperties() : base("UnknownProperties")
        {
        }
    }
}