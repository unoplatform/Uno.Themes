#nullable enable
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

#if HAS_UNO
using Microsoft.UI.Composition;
using Uno.WinUI.Graphics2DSK;
#endif

namespace Uno.Cupertino;

/// <summary>
/// A Liquid Glass surface: it blurs, saturates and refracts whatever is drawn beneath it. Place it as the
/// bottom layer of a bar, a floating control, a popover or a dialog, and put the content on top.
/// </summary>
/// <remarks>
/// <para>
/// Where the blurred backdrop cannot be drawn — WinAppSDK, native renderers, the software renderer, or when
/// <see cref="CupertinoTheme.GlassRenderingMode"/> is <see cref="GlassRenderingMode.Solid"/> — the panel
/// renders its <see cref="Control.Background"/> opaquely instead. <see cref="ActualRenderingMode"/> tells which.
/// </para>
/// <para>
/// A panel drawing the blurred backdrop repaints on every frame while it is loaded, so the app's render loop
/// does not idle while one is on screen. Use glass for a few large or transient surfaces, never inside
/// item templates.
/// </para>
/// </remarks>
[TemplatePart(Name = RootPartName, Type = typeof(Grid))]
[TemplatePart(Name = FillPartName, Type = typeof(Border))]
public partial class GlassPanel : Control
{
	private const string RootPartName = "PART_Root";
	private const string FillPartName = "PART_Fill";

	private Grid? _root;
	private Border? _fill;

#if HAS_UNO
	private SkiaGlassBackplate? _backplate;

	internal SkiaGlassBackplate? Backplate => _backplate;
#endif

	/// <summary>
	/// Initializes a new instance of the <see cref="GlassPanel"/> class.
	/// </summary>
	public GlassPanel()
	{
		Loaded += (_, _) => UpdateTier();
		RegisterPropertyChangedCallback(CornerRadiusProperty, (_, _) => UpdateTier());
	}

	#region Material (DP)
	/// <summary>
	/// Gets or sets the glass variant. Default is <see cref="GlassMaterial.Regular"/>.
	/// </summary>
	public GlassMaterial Material
	{
		get => (GlassMaterial)GetValue(MaterialProperty);
		set => SetValue(MaterialProperty, value);
	}

	/// <summary>Identifies the <see cref="Material"/> dependency property.</summary>
	public static DependencyProperty MaterialProperty { get; } =
		DependencyProperty.Register(
			nameof(Material),
			typeof(GlassMaterial),
			typeof(GlassPanel),
			new PropertyMetadata(GlassMaterial.Regular, OnTierPropertyChanged));
	#endregion

	#region RenderingMode (DP)
	/// <summary>
	/// Gets or sets how this panel is drawn. Default is <see cref="GlassRenderingMode.Auto"/>.
	/// <see cref="GlassRenderingMode.Solid"/> on <see cref="CupertinoTheme.GlassRenderingMode"/> wins over
	/// <see cref="GlassRenderingMode.Liquid"/> here.
	/// </summary>
	public GlassRenderingMode RenderingMode
	{
		get => (GlassRenderingMode)GetValue(RenderingModeProperty);
		set => SetValue(RenderingModeProperty, value);
	}

	/// <summary>Identifies the <see cref="RenderingMode"/> dependency property.</summary>
	public static DependencyProperty RenderingModeProperty { get; } =
		DependencyProperty.Register(
			nameof(RenderingMode),
			typeof(GlassRenderingMode),
			typeof(GlassPanel),
			new PropertyMetadata(GlassRenderingMode.Auto, OnTierPropertyChanged));
	#endregion

	#region Tint (DP)
	/// <summary>
	/// Gets or sets a brush laid over the glass, in both rendering modes. Its opacity is the tint strength.
	/// Default is <see langword="null"/>: no tint.
	/// </summary>
	public Brush? Tint
	{
		get => (Brush?)GetValue(TintProperty);
		set => SetValue(TintProperty, value);
	}

	/// <summary>Identifies the <see cref="Tint"/> dependency property.</summary>
	public static DependencyProperty TintProperty { get; } =
		DependencyProperty.Register(
			nameof(Tint),
			typeof(Brush),
			typeof(GlassPanel),
			new PropertyMetadata(null));
	#endregion

	/// <summary>
	/// Gets how the panel is drawn right now: <see cref="GlassRenderingMode.Liquid"/> or
	/// <see cref="GlassRenderingMode.Solid"/>, never <see cref="GlassRenderingMode.Auto"/>.
	/// It is resolved when the panel loads.
	/// </summary>
	public GlassRenderingMode ActualRenderingMode { get; private set; } = GlassRenderingMode.Solid;

	/// <inheritdoc />
	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

#if HAS_UNO
		// A new template means a new root: the backplate of the previous one goes with it.
		_root?.Children.Remove(_backplate);
		_backplate = null;
#endif
		_root = GetTemplateChild(RootPartName) as Grid;
		_fill = GetTemplateChild(FillPartName) as Border;

		UpdateTier();
	}

	private static void OnTierPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		=> (d as GlassPanel)?.UpdateTier();

	private void UpdateTier()
	{
		if (_root is null || _fill is null || !IsLoaded)
		{
			return;
		}

		ActualRenderingMode = ResolveTier();

#if HAS_UNO
		if (ActualRenderingMode == GlassRenderingMode.Liquid)
		{
			var preset = GlassPreset.For(Material);
			if (_backplate is null)
			{
				_backplate = new SkiaGlassBackplate();
				_root.Children.Insert(0, _backplate);
			}

			_backplate.Preset = preset;
			// ponytail: one radius for all four corners; per-corner radii need a second normal map.
			_backplate.Radius = (float)CornerRadius.TopLeft;
			_fill.Opacity = preset.Veil;
			return;
		}

		_root.Children.Remove(_backplate);
		_backplate = null;
#endif
		_fill.Opacity = 1;
	}

	private GlassRenderingMode ResolveTier()
	{
#if HAS_UNO
		var themeMode = GetThemeRenderingMode();
		if (RenderingMode == GlassRenderingMode.Solid
			|| themeMode == GlassRenderingMode.Solid
			|| !SKCanvasElement.IsSupportedOnCurrentPlatform())
		{
			return GlassRenderingMode.Solid;
		}

		if (RenderingMode == GlassRenderingMode.Liquid
			|| themeMode == GlassRenderingMode.Liquid
			|| CompositionCapabilities.GetForCurrentView().AreEffectsFast())
		{
			return GlassRenderingMode.Liquid;
		}
#endif
		return GlassRenderingMode.Solid;
	}

#if HAS_UNO
	// The nearest theme wins, so a scoped CupertinoTheme can differ from the application's.
	private GlassRenderingMode GetThemeRenderingMode()
	{
		for (DependencyObject? d = this; d is not null; d = VisualTreeHelper.GetParent(d))
		{
			if (d is FrameworkElement element && TryRead(element.Resources, out var scoped))
			{
				return scoped;
			}
		}

		return TryRead(Application.Current.Resources, out var mode) ? mode : GlassRenderingMode.Auto;

		static bool TryRead(ResourceDictionary resources, out GlassRenderingMode mode)
		{
			mode = GlassRenderingMode.Auto;
			return resources.TryGetValue(CupertinoConstants.GlassRenderingModeKey, out var value)
				&& value is string text
				&& Enum.TryParse(text, out mode);
		}
	}
#endif
}
