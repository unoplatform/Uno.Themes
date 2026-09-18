using System;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Cupertino;

/// <summary>
/// Legacy entry point: a <see cref="CupertinoTheme"/> that picks up the overrides recorded by a
/// <see cref="CupertinoColors"/> / <see cref="CupertinoFonts"/> declared before it.
/// </summary>
#pragma warning disable CS0618 // The obsolete recorders are this shim's only input.
[Obsolete("Use CupertinoTheme instead. This type will be removed in a future version.")]
public sealed class CupertinoResources : CupertinoTheme
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoResources"/> class from the recorded overrides.
	/// </summary>
	public CupertinoResources()
		: base(Load(CupertinoColors.RecordedOverrideSource), Load(CupertinoFonts.RecordedOverrideSource))
	{
		// A legacy font file redefines CupertinoFontFamily, which under the theme is an alias of the root and
		// reaches nothing when overridden. Translate it into the root so the whole type scale follows.
		if (FontOverrideDictionary is { } fonts
			&& !fonts.ContainsKey(DefaultFontFamilyKey)
			&& fonts.TryGetValue(CupertinoConstants.LegacyFontFamilyKey, out var value)
			&& value is FontFamily family)
		{
			DefaultFontFamily = family;
		}
	}

	private const string DefaultFontFamilyKey = "DefaultFontFamily";

	/// <summary>
	/// No longer has any effect: the implicit styles are part of <see cref="CupertinoTheme"/>.
	/// </summary>
	public bool WithImplicitStyles { set { } }

	private static ResourceDictionary Load(string source) =>
		string.IsNullOrWhiteSpace(source) ? null : new ResourceDictionary { Source = new Uri(source) };
}
#pragma warning restore CS0618
