using System;
using System.Collections.Generic;

namespace Uno.Themes;

/// <summary>
/// The semantic resource keys the shared Uno.Themes layer declares or generates, one read-only list per family.
/// </summary>
/// <remarks>
/// <para>
/// The keys come from the shared dictionaries and generated scales that <c>SimpleTheme</c> and the
/// version 2 styles of <c>MaterialTheme</c> merge, under both the light and dark theme dictionaries.
/// Material's version 1 styles use their own palette, and Cupertino merges neither.
/// </para>
/// <para>
/// The lists hold keys only; resolve a key through the resources of the element or application to read
/// its current value. Design-system specific keys (such as <c>SimpleButtonFontFamily</c>) and semantic
/// style keys (such as <c>FilledButtonStyle</c>) are not listed. The order of keys within a list is not
/// part of the contract.
/// </para>
/// </remarks>
public static class SemanticResourceKeys
{
	/// <summary>
	/// Gets the semantic color keys, such as <c>PrimaryColor</c>, <c>OnSurfaceColor</c> and <c>ShadowColor</c>.
	/// Each resolves to a <c>Color</c>.
	/// </summary>
	public static IReadOnlyList<string> Colors { get; } = BuildColors();

	/// <summary>
	/// Gets the interaction-state opacity keys, such as <c>HoverOpacity</c> and <c>DisabledOpacity</c>.
	/// Each resolves to a <see cref="double"/>.
	/// </summary>
	public static IReadOnlyList<string> Opacities { get; } = BuildOpacities();

	/// <summary>
	/// Gets the semantic brush keys: each color role's base brush (<c>PrimaryBrush</c>) and its
	/// interaction-state brushes (<c>PrimaryHoverBrush</c>, <c>PrimaryDisabledBrush</c>, …).
	/// Each resolves to a <c>SolidColorBrush</c>.
	/// </summary>
	public static IReadOnlyList<string> Brushes { get; } = BuildBrushes();

	/// <summary>
	/// Gets the typography font family keys: the root <c>DefaultFontFamily</c> token and one
	/// <c>{Role}{Size}FontFamily</c> key per type-scale slot, such as <c>BodyMediumFontFamily</c>.
	/// Each resolves to a <c>FontFamily</c>.
	/// </summary>
	public static IReadOnlyList<string> FontFamilies { get; } = Array.AsReadOnly((string[])ThemesConstants.TypefaceScaleKeys.Clone());

	/// <summary>
	/// Gets the typography font size keys, one per type-scale slot, such as <c>BodyMediumFontSize</c>.
	/// Each resolves to a <see cref="double"/>.
	/// </summary>
	public static IReadOnlyList<string> FontSizes { get; } = WithSuffix(ThemesConstants.TypeScaleSlots, "FontSize");

	/// <summary>
	/// Gets the typography font weight keys, one per type-scale slot, such as <c>BodyMediumFontWeight</c>.
	/// Each resolves to the weight's name as a <see cref="string"/>, such as <c>SemiBold</c>.
	/// </summary>
	public static IReadOnlyList<string> FontWeights { get; } = WithSuffix(ThemesConstants.TypeScaleSlots, "FontWeight");

	/// <summary>
	/// Gets the typography character spacing keys, such as <c>BodyMediumCharacterSpacing</c>. Not every
	/// type-scale slot declares one. Each resolves to an <see cref="int"/>.
	/// </summary>
	public static IReadOnlyList<string> CharacterSpacings { get; } = WithSuffix(ThemesConstants.CharacterSpacingSlots, "CharacterSpacing");

	/// <summary>
	/// Gets the spacing keys generated from <see cref="BaseTheme.DefaultSpacing"/> scaled by
	/// <see cref="BaseTheme.DefaultDensity"/>: each <c>Space*</c> value,
	/// such as <c>Space200</c>, and its <c>Thickness</c> companions, such as <c>Space200Thickness</c> and
	/// <c>Space200HorizontalThickness</c>.
	/// </summary>
	public static IReadOnlyList<string> Spacing { get; } = KeysOf(DesignTokenScales.SpacingTokens(1));

	/// <summary>
	/// Gets the shape keys generated from <see cref="BaseTheme.DefaultCornerRadius"/>: each <c>Radius*</c>
	/// value, such as <c>Radius200</c>, and its <c>CornerRadius</c> companion, such as
	/// <c>Radius200CornerRadius</c>.
	/// </summary>
	public static IReadOnlyList<string> Shape { get; } = KeysOf(DesignTokenScales.ShapeTokens(1));

	/// <summary>
	/// Gets the fixed control size keys: control heights, icon sizes and the touch target, such as
	/// <c>ControlHeightMedium</c>. These stay constant across <see cref="BaseTheme.DefaultDensity"/> modes.
	/// Each resolves to a <see cref="double"/>.
	/// </summary>
	public static IReadOnlyList<string> ControlSizes { get; } = KeysOf(DesignTokenScales.DensityTokens());

	private static IReadOnlyList<string> BuildColors()
	{
		var keys = new List<string>(ThemesConstants.SemanticColorKeys) { ThemesConstants.ShadowColorKey };
		return keys.AsReadOnly();
	}

	private static IReadOnlyList<string> BuildOpacities()
	{
		var keys = new List<string>();

		foreach (var state in ThemesConstants.BrushStateSuffixes)
		{
			if (state.Length > 0)
			{
				keys.Add(state + ThemesConstants.OpacitySuffix);
			}
		}

		return keys.AsReadOnly();
	}

	private static IReadOnlyList<string> BuildBrushes()
	{
		var keys = new List<string>();

		foreach (var colorKey in ThemesConstants.SemanticColorKeys)
		{
			var role = colorKey.Substring(0, colorKey.Length - ThemesConstants.ColorSuffix.Length);

			if (Array.IndexOf(ThemesConstants.BaseBrushOnlyColorKeys, colorKey) >= 0)
			{
				keys.Add(role + ThemesConstants.BrushSuffix);
				continue;
			}

			foreach (var state in ThemesConstants.BrushStateSuffixes)
			{
				keys.Add(role + state + ThemesConstants.BrushSuffix);
			}
		}

		return keys.AsReadOnly();
	}

	private static IReadOnlyList<string> WithSuffix(string[] slots, string suffix)
	{
		var keys = new string[slots.Length];

		for (var i = 0; i < slots.Length; i++)
		{
			keys[i] = slots[i] + suffix;
		}

		return Array.AsReadOnly(keys);
	}

	private static IReadOnlyList<string> KeysOf(IEnumerable<(string Key, object Value)> tokens)
	{
		var keys = new List<string>();

		foreach (var (key, _) in tokens)
		{
			keys.Add(key);
		}

		return keys.AsReadOnly();
	}
}
