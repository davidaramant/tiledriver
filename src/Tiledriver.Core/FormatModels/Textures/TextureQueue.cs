// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Textures;

public sealed class TextureQueue
{
	private readonly List<CompositeTexture> _definitions = [];
	private readonly HashSet<string> _compositeNames = [];
	private readonly List<(RenderedTexture, Texture)> _renderQueue = [];
	private readonly Dictionary<RenderedTexture, Texture> _renderNameLookup = new();

	public IReadOnlyList<CompositeTexture> Definitions => _definitions;
	public IReadOnlyList<(RenderedTexture, Texture)> RenderQueue => _renderQueue;

	public void Add(params CompositeTexture[] definitions)
	{
		foreach (var def in definitions)
		{
			if (_compositeNames.Add(def.Name))
			{
				_definitions.Add(def);
			}
		}
	}

	public Texture Add(RenderedTexture renderedTexture)
	{
		if (_renderNameLookup.TryGetValue(renderedTexture, out var existing))
		{
			return existing;
		}

		var number = _renderQueue.Count;
		var tex = new Texture(
			string.IsNullOrWhiteSpace(renderedTexture.Name) ? $"RNDR{number:D3}" : renderedTexture.Name
		);
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
					Patches: [new Patch(tex.Name, 0, 0, Rotate: renderedTexture.Rotation)]
				)
			);
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
