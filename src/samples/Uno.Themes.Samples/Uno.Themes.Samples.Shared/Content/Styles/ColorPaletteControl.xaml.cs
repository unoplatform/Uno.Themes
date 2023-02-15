using System;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Uno.Themes.Samples.Content.Styles;

public sealed partial class ColorPaletteControl : UserControl
{


	public Brush ColorForeground
	{
		get { return (Brush)GetValue(ColorForegroundProperty); }
		set { SetValue(ColorForegroundProperty, value); }
	}
	public static readonly DependencyProperty ColorForegroundProperty =
		DependencyProperty.Register("ColorForeground", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorForegroundChanged));

	private static void ColorForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if(d is ColorPaletteControl palette &&
			e.NewValue is Brush brush)
		{
			palette.Color1Foreground= brush;
			palette.Color2Foreground = brush;
			palette.Color3Foreground = brush;
			palette.Color4Foreground = brush;
			palette.Color5Foreground = brush;
		}
	}

	public Brush Color1Foreground
	{
		get { return (Brush)GetValue(Color1ForegroundProperty); }
		set { SetValue(Color1ForegroundProperty, value); }
	}

	public static readonly DependencyProperty Color1ForegroundProperty =
		DependencyProperty.Register("Color1Foreground", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color2Foreground
	{
		get { return (Brush)GetValue(Color2ForegroundProperty); }
		set { SetValue(Color2ForegroundProperty, value); }
	}

	public static readonly DependencyProperty Color2ForegroundProperty =
		DependencyProperty.Register("Color2Foreground", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color3Foreground
	{
		get { return (Brush)GetValue(Color3ForegroundProperty); }
		set { SetValue(Color3ForegroundProperty, value); }
	}

	public static readonly DependencyProperty Color3ForegroundProperty =
		DependencyProperty.Register("Color3Foreground", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color4Foreground
	{
		get { return (Brush)GetValue(Color4ForegroundProperty); }
		set { SetValue(Color4ForegroundProperty, value); }
	}

	public static readonly DependencyProperty Color4ForegroundProperty =
		DependencyProperty.Register("Color4Foreground", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, Color4ForegroundChanged));

	private static void Color4ForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
	}

	public Brush Color5Foreground
	{
		get { return (Brush)GetValue(Color5ForegroundProperty); }
		set { SetValue(Color5ForegroundProperty, value); }
	}

	public static readonly DependencyProperty Color5ForegroundProperty =
		DependencyProperty.Register("Color5Foreground", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));


	public string Color1BrushName
	{
		get { return (string)GetValue(Color1BrushNameProperty); }
		set { SetValue(Color1BrushNameProperty, value); }
	}

	public static readonly DependencyProperty Color1BrushNameProperty =
		DependencyProperty.Register("Color1BrushName", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public string Color2BrushName
	{
		get { return (string)GetValue(Color2BrushNameProperty); }
		set { SetValue(Color2BrushNameProperty, value); }
	}

	public static readonly DependencyProperty Color2BrushNameProperty =
		DependencyProperty.Register("Color2BrushName", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public string Color3BrushName
	{
		get { return (string)GetValue(Color3BrushNameProperty); }
		set { SetValue(Color3BrushNameProperty, value); }
	}

	public static readonly DependencyProperty Color3BrushNameProperty =
		DependencyProperty.Register("Color3BrushName", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public string Color4BrushName
	{
		get { return (string)GetValue(Color4BrushNameProperty); }
		set { SetValue(Color4BrushNameProperty, value); }
	}

	public static readonly DependencyProperty Color4BrushNameProperty =
		DependencyProperty.Register("Color4BrushName", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public string Color5BrushName
	{
		get { return (string)GetValue(Color5BrushNameProperty); }
		set { SetValue(Color5BrushNameProperty, value); }
	}

	public static readonly DependencyProperty Color5BrushNameProperty =
		DependencyProperty.Register("Color5BrushName", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null));



	public Brush Color1Brush
	{
		get { return (Brush)GetValue(Color1BrushProperty); }
		set { SetValue(Color1BrushProperty, value); }
	}

	public static readonly DependencyProperty Color1BrushProperty =
		DependencyProperty.Register("Color1Brush", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorBrushChanged));

	public Brush Color2Brush
	{
		get { return (Brush)GetValue(Color2BrushProperty); }
		set { SetValue(Color2BrushProperty, value); }
	}

	public static readonly DependencyProperty Color2BrushProperty =
		DependencyProperty.Register("Color2Brush", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorBrushChanged));
	public Brush Color3Brush
	{
		get { return (Brush)GetValue(Color3BrushProperty); }
		set { SetValue(Color3BrushProperty, value); }
	}

	public static readonly DependencyProperty Color3BrushProperty =
		DependencyProperty.Register("Color3Brush", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorBrushChanged));
	public Brush Color4Brush
	{
		get { return (Brush)GetValue(Color4BrushProperty); }
		set { SetValue(Color4BrushProperty, value); }
	}

	public static readonly DependencyProperty Color4BrushProperty =
		DependencyProperty.Register("Color4Brush", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorBrushChanged));
	public Brush Color5Brush
	{
		get { return (Brush)GetValue(Color5BrushProperty); }
		set { SetValue(Color5BrushProperty, value); }
	}

	public static readonly DependencyProperty Color5BrushProperty =
		DependencyProperty.Register("Color5Brush", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorBrushChanged));

	private static void ColorBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPaletteControl palette)
		{
			if (palette.Color5Brush is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has5Color), false);
			}
			else if (palette.Color4Brush is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has4Color), false);
			}
			else if (palette.Color3Brush is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has3Color), false);
			}
			else if (palette.Color2Brush is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has2Color), false);
			}
			else
			{
				VisualStateManager.GoToState(palette, nameof(Has1Color), false);
			}
		}
	}

	public double Color1Opacity
	{
		get { return (double)GetValue(Color1OpacityProperty); }
		set { SetValue(Color1OpacityProperty, value); }
	}

	public static readonly DependencyProperty Color1OpacityProperty =
		DependencyProperty.Register("Color1Opacity", typeof(double), typeof(ColorPaletteControl), new PropertyMetadata(1.0));

	public double Color2Opacity
	{
		get { return (double)GetValue(Color2OpacityProperty); }
		set { SetValue(Color2OpacityProperty, value); }
	}

	public static readonly DependencyProperty Color2OpacityProperty =
		DependencyProperty.Register("Color2Opacity", typeof(double), typeof(ColorPaletteControl), new PropertyMetadata(1.0));

	public double Color3Opacity
	{
		get { return (double)GetValue(Color3OpacityProperty); }
		set { SetValue(Color3OpacityProperty, value); }
	}

	public static readonly DependencyProperty Color3OpacityProperty =
		DependencyProperty.Register("Color3Opacity", typeof(double), typeof(ColorPaletteControl), new PropertyMetadata(1.0));

	public double Color4Opacity
	{
		get { return (double)GetValue(Color4OpacityProperty); }
		set { SetValue(Color4OpacityProperty, value); }
	}

	public static readonly DependencyProperty Color4OpacityProperty =
		DependencyProperty.Register("Color4Opacity", typeof(double), typeof(ColorPaletteControl), new PropertyMetadata(1.0));

	public double Color5Opacity
	{
		get { return (double)GetValue(Color5OpacityProperty); }
		set { SetValue(Color5OpacityProperty, value); }
	}

	public static readonly DependencyProperty Color5OpacityProperty =
		DependencyProperty.Register("Color5Opacity", typeof(double), typeof(ColorPaletteControl), new PropertyMetadata(1.0));



	public ColorPaletteControl()
	{
		this.InitializeComponent();
	}
}

public class BrushToTextConverter:IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if(value is SolidColorBrush brush)
		{
			if (brush.Opacity < 1.0)
			{
				return $"{brush.Color.ToString()} {(int)(brush.Opacity * 100)}%";
			}
			return $"{brush.Color.ToString()}";

		}
		return value?.ToString()??string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}


