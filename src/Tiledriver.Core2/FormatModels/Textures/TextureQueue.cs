// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Textures
{
    public sealed class TextureQueue
    {
        private readonly List<CompositeTexture> _definitions = new();
        private readonly List<(RenderedTexture, Texture)> _renderQueue = new();
        private readonly Dictionary<RenderedTexture, Texture> _renderNameLookup = new();

        public IReadOnlyList<CompositeTexture> Definitions => _definitions;
        public IReadOnlyList<(RenderedTexture, Texture)> RenderQueue => _renderQueue;

        public void Add(params CompositeTexture[] definitions) => _definitions.AddRange(definitions);

        public Texture Add(RenderedTexture renderedTexture)
        {
            if (_renderNameLookup.TryGetValue(renderedTexture, out var existing))
            {
                return existing;
            }

            var number = _renderQueue.Count;
            var tex = new Texture($"RNDR{number:D3}");
            _renderQueue.Add((renderedTexture, tex));

            if (renderedTexture.HasText)
            {
                Texture compositeName = "TEXT" + number;
                _definitions.Add(
                    new CompositeTexture(
                        compositeName.Name,
                        256,
                        256,
                        XScale: 4,
                        YScale: 4,
                        Patches: ImmutableList.Create(new Patch(tex.Name, 0, 0))));
                _renderNameLookup.Add(renderedTexture, compositeName);
                return compositeName;
            }
            else
            {
                _renderNameLookup.Add(renderedTexture, tex);
                return tex;

            }
        }
    }
}