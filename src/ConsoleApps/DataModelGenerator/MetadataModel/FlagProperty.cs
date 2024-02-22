// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class FlagProperty : ScalarProperty
{
	public bool Default => true;

	public FlagProperty(string name, bool isNullable = false)
		: base(name, "bool", isNullable: isNullable, defaultString: "true") { }
}
