#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>
/// A <see cref="ResourceDictionary"/> whose entries are the design-system-agnostic (semantic)
/// resources: <c>FilledButtonStyle</c>, <c>PrimaryBrush</c>, <c>Space400Thickness</c>,
/// <c>BodyMediumFontFamily</c>, and so on. Theme-prefixed keys (<c>MaterialFilledButtonStyle</c>,
/// <c>SimpleFilledButtonStyle</c>) never live in one.
/// </summary>
/// <remarks>
/// The type is the whole contract. Walking a theme, a key is semantic when any dictionary declaring
/// it is a <see cref="SemanticResources"/> or sits in the <see cref="ResourceDictionary.ThemeDictionaries"/>
/// of one (a file loaded through <see cref="ResourceDictionary.Source"/> copies its theme
/// dictionaries in as plain <see cref="ResourceDictionary"/> instances). A semantic key may also
/// appear in a plain dictionary, for example a consumer colour override; the semantic declaration
/// is what marks it.
/// </remarks>
public sealed class SemanticResources : ResourceDictionary
{
}
