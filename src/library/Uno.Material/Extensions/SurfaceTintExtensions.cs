using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Windows.UI;
using Uno.Disposables;
using Microsoft.Extensions.Logging;
using Uno.Extensions;

#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Material
{
	internal static class SurfaceTintExtensions
	{
		#region DependencyProperty: Subscription

		private static DependencyProperty SubscriptionProperty { [DynamicDependency(nameof(GetSubscription))] get; } = DependencyProperty.RegisterAttached(
			"Subscription",
			typeof(IDisposable),
			typeof(SurfaceTintExtensions),
			new PropertyMetadata(default(IDisposable)));

		[DynamicDependency(nameof(SetSubscription))]
		private static IDisposable GetSubscription(Control obj) => (IDisposable)obj.GetValue(SubscriptionProperty);
		[DynamicDependency(nameof(GetSubscription))]
		private static void SetSubscription(Control obj, IDisposable value) => obj.SetValue(SubscriptionProperty, value);

		#endregion

		internal static void OnIsTintEnabledChanged(DependencyObject sender, bool isEnabled)
		{
			if (sender is not Control control)
			{
				return;
			}

			if (isEnabled)
			{
				if (GetSubscription(control) is null)
				{
					var disposable = new CompositeDisposable();

					control.Unloaded += OnControlUnloaded;
					disposable.Add(Disposable.Create(() => control.Unloaded -= OnControlUnloaded));

					control
						.RegisterDisposablePropertyChangedCallback(Control.BackgroundProperty, OnBackgroundPropertyChanged)
						.DisposeWith(disposable);

					SetSubscription(control, disposable);
				}

				var elevation = ControlExtensions.GetElevation(control);
				OnElevationChanged(control, elevation);
			}
			else
			{
				GetSubscription(sender as Control)?.Dispose();
			}
		}

		internal static void OnElevationChanged(DependencyObject sender, int elevation)
		{
			ApplyBackgroundTint(sender, elevation);
		}

		private static void OnBackgroundPropertyChanged(DependencyObject sender, DependencyProperty dp)
		{
			ApplyBackgroundTint(sender, ControlExtensions.GetElevation(sender as Control));
		}

		private static void ApplyBackgroundTint(DependencyObject owner, int elevation)
		{
			if (owner is not Control control)
			{
				return;
			}

			
			Color tintColor;
			var background = control.Background;

			if (control.Background is not SolidColorBrush scbBackground)
			{
				scbBackground = new SolidColorBrush(Colors.Transparent);

				if (background is { } nonScbBrush)
				{
					owner.Log().LogWarning($"SufaceTintExtensions only supports Brushes of type SolidColorBrush. Current type: {nonScbBrush.GetType().Name}");
				}
			}

			if (!ControlExtensions.GetIsTintEnabled(control))
			{
				tintColor = Colors.Transparent;
			}
			else
			{
				if (!(Application.Current.Resources.TryGetValue("SurfaceTintColor", out var tint) && tint is Color color))
				{
					owner.Log().LogWarning($"Failed to resolve resource SurfaceTintColor");
					return;
				}

				color.A = GetElevationAlpha(elevation);
				tintColor = color;
			}
			
			var tintedBackground = new SolidColorBrush(AlphaBlend(tintColor, scbBackground.Color));
			
			ControlExtensions.SetTintedBackground(control, tintedBackground);
		}

		private static byte GetElevationAlpha(int elevation)
			=> elevation switch
			{
				1 => 0x0D,
				2 => 0x14,
				3 => 0x1C,
				4 => 0x1F,
				5 => 0x24,
				_ => 0x00
			};

		private static Color AlphaBlend(Color foreground, Color background)
		{
			var alpha = foreground.A;
			if (alpha == 0x00) // Foreground completely transparent
			{
				return background;
			}
			var complementaryAlpha = (byte)(0xff - alpha);
			int backAlpha = background.A;
			if (backAlpha == 0xff)  // Foreground completely transparent.
			{ 
				return Color.FromArgb(
					0xff,
					(byte)((alpha * foreground.R + complementaryAlpha * background.R) / 0xff),
					(byte)((alpha * foreground.G + complementaryAlpha * background.G) / 0xff),
					(byte)((alpha * foreground.B + complementaryAlpha * background.B) / 0xff)
				);
			}
			else // General case
			{ 
				backAlpha = (backAlpha * complementaryAlpha) / 0xff;
				var outAlpha = alpha + backAlpha;
				if (outAlpha != 0x00)
				{
					return background;
				}

				return Color.FromArgb(
					(byte)outAlpha,
					(byte)((foreground.R * alpha + background.R * backAlpha) / outAlpha),
					(byte)((foreground.G * alpha + background.G * backAlpha) / outAlpha),
					(byte)((foreground.B * alpha + background.B * backAlpha) / outAlpha)
				);
			}
		}

		private static void OnControlUnloaded(object sender, RoutedEventArgs e)
		{
			GetSubscription(sender as Control)?.Dispose();
		}
	}
}
