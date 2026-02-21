namespace Tiledriver.Core.FormatModels;

public sealed class CompoundResourceProvider : IResourceProvider
{
	private readonly Stack<IResourceProvider> _providers = new();

	public void AddProvider(IResourceProvider provider) => _providers.Push(provider);

	public Stream Lookup(string path) => TryLookup(path) ?? throw new EntryNotFoundException(path);

	public Stream? TryLookup(string path)
	{
		foreach (var provider in _providers)
		{
			var result = provider.TryLookup(path);
			if (result != null)
			{
				return result;
			}
		}
		return null;
	}

	public void Dispose()
	{
		foreach (var provider in _providers)
		{
			provider.Dispose();
		}
	}
}
