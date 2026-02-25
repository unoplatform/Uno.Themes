#if HAS_UNO

using System;
using System.Collections.Generic;
using System.IO;

namespace Uno.Themes.AlcHost;

/// <summary>
/// Provides deduplicated content resolution for an AssemblyLoadContext.
/// Maps logical relative paths and file names to physical paths on disk.
/// </summary>
internal sealed class AlcContentPool
{
	private readonly Dictionary<string, string> _byRelativePath;
	private readonly Dictionary<string, string> _byFileName;
	private readonly List<string> _allRelativePaths;

	public string Name { get; }

	internal AlcContentPool(
		string name,
		Dictionary<string, string> byRelativePath,
		Dictionary<string, string> byFileName,
		List<string> allRelativePaths)
	{
		Name = name;
		_byRelativePath = byRelativePath;
		_byFileName = byFileName;
		_allRelativePaths = allRelativePaths;
	}

	/// <summary>
	/// Returns all original relative paths.
	/// </summary>
	public IReadOnlyList<string> ListFiles() => _allRelativePaths;

	/// <summary>
	/// Resolves an original relative path to an absolute physical path.
	/// </summary>
	public string? ResolveByRelativePath(string relativePath) =>
		_byRelativePath.TryGetValue(relativePath, out var path) ? path : null;

	/// <summary>
	/// Resolves a file name (e.g. "X.dll") to an absolute physical path (first match).
	/// </summary>
	public string? ResolveByFileName(string fileName) =>
		_byFileName.TryGetValue(fileName, out var path) ? path : null;

	/// <summary>
	/// Builds a pool by scanning a directory for all files.
	/// </summary>
	internal static AlcContentPool BuildFromDirectory(string directoryPath, string poolName)
	{
		var byRelativePath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		var byFileName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		var allRelativePaths = new List<string>();

		if (!Directory.Exists(directoryPath))
		{
			return new AlcContentPool(poolName, byRelativePath, byFileName, allRelativePaths);
		}

		foreach (var fullPath in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
		{
			var relativePath = Path.GetRelativePath(directoryPath, fullPath).Replace('\\', '/');
			byRelativePath[relativePath] = fullPath;
			allRelativePaths.Add(relativePath);

			var fileName = Path.GetFileName(relativePath);
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				byFileName.TryAdd(fileName, fullPath);
			}
		}

		return new AlcContentPool(poolName, byRelativePath, byFileName, allRelativePaths);
	}
}
#endif
