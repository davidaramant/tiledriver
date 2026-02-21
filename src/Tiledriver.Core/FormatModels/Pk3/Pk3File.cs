using System.IO.Compression;

namespace Tiledriver.Core.FormatModels.Pk3;

public sealed class Pk3File : IResourceProvider
{
	private readonly ZipArchive _archive;

	private Pk3File(string fileName) =>
		_archive = new ZipArchive(File.OpenRead(fileName), ZipArchiveMode.Read, leaveOpen: false);

	public static Pk3File Open(string fileName) => new(fileName);

	public Stream Lookup(string path) => TryLookup(path) ?? throw new EntryNotFoundException(path);

	public Stream? TryLookup(string path) =>
		_archive.Entries.SingleOrDefault(e => StringComparer.InvariantCultureIgnoreCase.Equals(path, e.Name))?.Open();

	public IEnumerable<string> GetAllEntryNames() => _archive.Entries.Select(e => e.Name);

	public void Dispose() => _archive.Dispose();
}
