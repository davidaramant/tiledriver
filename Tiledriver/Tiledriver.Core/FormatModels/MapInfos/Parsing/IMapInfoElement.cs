// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public interface IMapInfoElement
    {
        Identifier Name { get; }
        bool IsBlock { get; }
        MapInfoBlock AsBlock();
        MapInfoProperty AsProperty();
    }
}