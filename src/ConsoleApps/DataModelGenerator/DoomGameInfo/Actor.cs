// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.DoomGameInfo;

sealed record Actor(
    string Name,
    int Id,
    string Description)
{
    public string SafeName => Name.TrimStart('$');
}
