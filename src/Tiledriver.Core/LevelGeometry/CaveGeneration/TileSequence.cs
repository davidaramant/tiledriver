// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    public sealed class TileSequence<TDescription> where TDescription : notnull
    {
        private readonly Dictionary<TDescription, int> _descriptionToIndex = new();
        private readonly Func<TDescription, Tile> _transformToTile;

        public TileSequence(Func<TDescription, Tile> transformToTile)
        {
            _transformToTile = transformToTile;
        }

        public int GetTileIndex(TDescription description)
        {
            if (_descriptionToIndex.TryGetValue(description, out int index))
            {
                return index;
            }

            var nextIndex = _descriptionToIndex.Count;
            _descriptionToIndex.Add(description, nextIndex);
            return nextIndex;
        }

        public IEnumerable<Tile> GetTileDefinitions() =>
            _descriptionToIndex
                .OrderBy(pair => pair.Value)
                .Select(pair => _transformToTile(pair.Key));
    }
}