// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    /// <summary>
    /// Holds a cache of descriptions for models. The same index will be returned for equivalent descriptions.
    /// </summary>
    /// <typeparam name="TDescription">A simplified description of a the model.</typeparam>
    /// <typeparam name="TModel">The full type that is being described.</typeparam>
    public sealed class ModelSequence<TDescription, TModel>
        where TDescription : notnull
    {
        private readonly Dictionary<TDescription, int> _descriptionToIndex = new();
        private readonly Func<TDescription, TModel> _transformToModel;

        public int Count => _descriptionToIndex.Count;

        /// <summary>
        /// Constructs a new instance with a method to transform a description to a model.
        /// </summary>
        /// <param name="transformToModel">How to transform a description to a model.</param>
        public ModelSequence(Func<TDescription, TModel> transformToModel) => _transformToModel = transformToModel;

        public int GetIndex(TDescription description)
        {
            if (_descriptionToIndex.TryGetValue(description, out int index))
            {
                return index;
            }

            var nextIndex = _descriptionToIndex.Count;
            _descriptionToIndex.Add(description, nextIndex);
            return nextIndex;
        }

        public IEnumerable<TModel> GetDefinitions() =>
            _descriptionToIndex.OrderBy(pair => pair.Value).Select(pair => _transformToModel(pair.Key));
    }
}
