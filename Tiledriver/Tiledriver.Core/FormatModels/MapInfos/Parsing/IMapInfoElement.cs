// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public interface IMapInfoElement
    {
        bool IsBlock { get; }
        MapInfoBlock AsBlock();
        MapInfoProperty AsAssignment();
    }
}