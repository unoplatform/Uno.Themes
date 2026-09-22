#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace Uno.Cupertino;

// The alert template supplies primary, secondary, then close. Keep the default action trailing in a
// two-button row, or first when localization/text sizing or a third action requires a vertical stack.
internal sealed class CupertinoAlertActionsPanel : Panel
{
	private const double ActionSpacing = 8;
	private readonly List<UIElement> _ordered = new(3);
	private bool _horizontal;

	public ContentDialogButton DefaultButton
	{
		get => (ContentDialogButton)GetValue(DefaultButtonProperty);
		set => SetValue(DefaultButtonProperty, value);
	}

	public static readonly DependencyProperty DefaultButtonProperty = DependencyProperty.Register(
		nameof(DefaultButton), typeof(ContentDialogButton), typeof(CupertinoAlertActionsPanel),
		new PropertyMetadata(ContentDialogButton.None, (sender, _) => ((CupertinoAlertActionsPanel)sender).InvalidateMeasure()));

	protected override Size MeasureOverride(Size availableSize)
	{
		_ordered.Clear();
		var preferredIndex = DefaultButton switch
		{
			ContentDialogButton.Secondary => 1,
			ContentDialogButton.Close => 2,
			_ => 0,
		};
		if (preferredIndex < Children.Count && Children[preferredIndex].Visibility == Visibility.Visible)
		{
			_ordered.Add(Children[preferredIndex]);
		}
		for (var i = 0; i < Children.Count; i++)
		{
			if (i != preferredIndex && Children[i].Visibility == Visibility.Visible)
			{
				_ordered.Add(Children[i]);
			}
		}

		var naturalWidth = 0d;
		foreach (var child in _ordered)
		{
			child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			naturalWidth = Math.Max(naturalWidth, child.DesiredSize.Width);
		}
		_horizontal = _ordered.Count == 2 && naturalWidth * 2 + ActionSpacing <= availableSize.Width;
		var width = double.IsPositiveInfinity(availableSize.Width)
			? naturalWidth * (_horizontal ? 2 : 1) + (_horizontal ? ActionSpacing : 0)
			: availableSize.Width;
		var childWidth = _horizontal ? Math.Max(0, (width - ActionSpacing) / 2) : width;
		var height = 0d;
		foreach (var child in _ordered)
		{
			child.Measure(new Size(childWidth, double.PositiveInfinity));
			height = _horizontal ? Math.Max(height, child.DesiredSize.Height) : height + child.DesiredSize.Height;
		}
		if (!_horizontal)
		{
			height += Math.Max(0, _ordered.Count - 1) * ActionSpacing;
		}
		return new Size(width, height);
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		var offset = 0d;
		for (var i = 0; i < _ordered.Count; i++)
		{
			var child = _ordered[i];
			if (_horizontal)
			{
				var width = Math.Max(0, (finalSize.Width - ActionSpacing) / 2);
				// Layout coordinates are mirrored by the framework for RTL.
				child.Arrange(new Rect(i == 0 ? width + ActionSpacing : 0, 0, width, finalSize.Height));
			}
			else
			{
				child.Arrange(new Rect(0, offset, finalSize.Width, child.DesiredSize.Height));
				offset += child.DesiredSize.Height + ActionSpacing;
			}
		}
		return finalSize;
	}
}
