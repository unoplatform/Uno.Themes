// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Uno.Themes.WinUI.Samples.Content.Styles;

public sealed partial class ColorPaletteControl : UserControl
{


	public Brush ColorState
	{
		get { return (Brush)GetValue(ColorStateProperty); }
		set { SetValue(ColorStateProperty, value); }
	}

	// Using a DependencyProperty as the backing store for ColorState.  This enables animation, styling, binding, etc...
	public static readonly DependencyProperty ColorStateProperty =
		DependencyProperty.Register("ColorState", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null,ColorStateChanged));

	private static void ColorStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPaletteControl palette &&
	e.NewValue is Brush brush)
		{
			palette.Color1State = brush;
			palette.Color2State = brush;
			palette.Color3State = brush;
			palette.Color4State = brush;
			palette.Color5State = brush;
		}
	}



	public Brush Color1State
	{
		get { return (Brush)GetValue(Color1StateProperty); }
		set { SetValue(Color1StateProperty, value); }
	}
	public static readonly DependencyProperty Color1StateProperty =
		DependencyProperty.Register("Color1State", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color2State
	{
		get { return (Brush)GetValue(Color2StateProperty); }
		set { SetValue(Color2StateProperty, value); }
	}
	public static readonly DependencyProperty Color2StateProperty =
		DependencyProperty.Register("Color2State", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color3State
	{
		get { return (Brush)GetValue(Color3StateProperty); }
		set { SetValue(Color3StateProperty, value); }
	}
	public static readonly DependencyProperty Color3StateProperty =
		DependencyProperty.Register("Color3State", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color4State
	{
		get { return (Brush)GetValue(Color4StateProperty); }
		set { SetValue(Color4StateProperty, value); }
	}
	public static readonly DependencyProperty Color4StateProperty =
		DependencyProperty.Register("Color4State", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color5State
	{
		get { return (Brush)GetValue(Color5StateProperty); }
		set { SetValue(Color5StateProperty, value); }
	}
	public static readonly DependencyProperty Color5StateProperty =
		DependencyProperty.Register("Color5State", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));



	public Brush ColorContent
	{
		get { return (Brush)GetValue(ColorContentProperty); }
		set { SetValue(ColorContentProperty, value); }
	}
	public static readonly DependencyProperty ColorContentProperty =
		DependencyProperty.Register("ColorContent", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorContentChanged));

	private static void ColorContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if(d is ColorPaletteControl palette &&
			e.NewValue is Brush brush)
		{
			palette.Color1Content= brush;
			palette.Color2Content = brush;
			palette.Color3Content = brush;
			palette.Color4Content = brush;
			palette.Color5Content = brush;
		}
	}

	public Brush Color1Content
	{
		get { return (Brush)GetValue(Color1ContentProperty); }
		set { SetValue(Color1ContentProperty, value); }
	}

	public static readonly DependencyProperty Color1ContentProperty =
		DependencyProperty.Register("Color1Content", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color2Content
	{
		get { return (Brush)GetValue(Color2ContentProperty); }
		set { SetValue(Color2ContentProperty, value); }
	}

	public static readonly DependencyProperty Color2ContentProperty =
		DependencyProperty.Register("Color2Content", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color3Content
	{
		get { return (Brush)GetValue(Color3ContentProperty); }
		set { SetValue(Color3ContentProperty, value); }
	}

	public static readonly DependencyProperty Color3ContentProperty =
		DependencyProperty.Register("Color3Content", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color4Content
	{
		get { return (Brush)GetValue(Color4ContentProperty); }
		set { SetValue(Color4ContentProperty, value); }
	}

	public static readonly DependencyProperty Color4ContentProperty =
		DependencyProperty.Register("Color4Content", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, Color4ContentChanged));

	private static void Color4ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
	}

	public Brush Color5Content
	{
		get { return (Brush)GetValue(Color5ContentProperty); }
		set { SetValue(Color5ContentProperty, value); }
	}

	public static readonly DependencyProperty Color5ContentProperty =
		DependencyProperty.Register("Color5Content", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));


	public string Color1Name
	{
		get { return (string)GetValue(Color1NameProperty); }
		set { SetValue(Color1NameProperty, value); }
	}

	public static readonly DependencyProperty Color1NameProperty =
		DependencyProperty.Register("Color1Name", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorNameChanged));

	public string Color2Name
	{
		get { return (string)GetValue(Color2NameProperty); }
		set { SetValue(Color2NameProperty, value); }
	}

	public static readonly DependencyProperty Color2NameProperty =
		DependencyProperty.Register("Color2Name", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorNameChanged));

	public string Color3Name
	{
		get { return (string)GetValue(Color3NameProperty); }
		set { SetValue(Color3NameProperty, value); }
	}

	public static readonly DependencyProperty Color3NameProperty =
		DependencyProperty.Register("Color3Name", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorNameChanged));

	public string Color4Name
	{
		get { return (string)GetValue(Color4NameProperty); }
		set { SetValue(Color4NameProperty, value); }
	}

	public static readonly DependencyProperty Color4NameProperty =
		DependencyProperty.Register("Color4Name", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorNameChanged));

	public string Color5Name
	{
		get { return (string)GetValue(Color5NameProperty); }
		set { SetValue(Color5NameProperty, value); }
	}

	public static readonly DependencyProperty Color5NameProperty =
		DependencyProperty.Register("Color5Name", typeof(string), typeof(ColorPaletteControl), new PropertyMetadata(null, ColorNameChanged));




	public Brush ColorContainer
	{
		get { return (Brush)GetValue(ColorContainerProperty); }
		set { SetValue(ColorContainerProperty, value); }
	}

	public static readonly DependencyProperty ColorContainerProperty =
		DependencyProperty.Register("ColorContainer", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null, AllColorContainerChanged));

	private static void AllColorContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPaletteControl palette &&
			e.NewValue is Brush brush)
		{
			palette.Color1Container = brush;
			palette.Color2Container = brush;
			palette.Color3Container = brush;
			palette.Color4Container = brush;
			palette.Color5Container = brush;
		}
	}

	public Brush Color1Container
	{
		get { return (Brush)GetValue(Color1ContainerProperty); }
		set { SetValue(Color1ContainerProperty, value); }
	}

	public static readonly DependencyProperty Color1ContainerProperty =
		DependencyProperty.Register("Color1Container", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	public Brush Color2Container
	{
		get { return (Brush)GetValue(Color2ContainerProperty); }
		set { SetValue(Color2ContainerProperty, value); }
	}

	public static readonly DependencyProperty Color2ContainerProperty =
		DependencyProperty.Register("Color2Container", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color3Container
	{
		get { return (Brush)GetValue(Color3ContainerProperty); }
		set { SetValue(Color3ContainerProperty, value); }
	}

	public static readonly DependencyProperty Color3ContainerProperty =
		DependencyProperty.Register("Color3Container", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color4Container
	{
		get { return (Brush)GetValue(Color4ContainerProperty); }
		set { SetValue(Color4ContainerProperty, value); }
	}

	public static readonly DependencyProperty Color4ContainerProperty =
		DependencyProperty.Register("Color4Container", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));
	public Brush Color5Container
	{
		get { return (Brush)GetValue(Color5ContainerProperty); }
		set { SetValue(Color5ContainerProperty, value); }
	}

	public static readonly DependencyProperty Color5ContainerProperty =
		DependencyProperty.Register("Color5Container", typeof(Brush), typeof(ColorPaletteControl), new PropertyMetadata(null));

	private static void ColorNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPaletteControl palette)
		{
			if (palette.Color5Name is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has5Color), false);
			}
			else if (palette.Color4Name is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has4Color), false);
			}
			else if (palette.Color3Name is not null)
			{
				VisualStateManager.GoToState(palette, nameof(Has3Color), false);
			}
			else if (palette.Color2Name is not null)
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

	public Brush Color1TagBrush => (Color1State is SolidColorBrush brush && brush.Color != Colors.Transparent)? brush : Color1Container;
	public Brush Color2TagBrush => (Color2State is SolidColorBrush brush && brush.Color != Colors.Transparent)? brush :  Color2Container;
	public Brush Color3TagBrush => (Color3State is SolidColorBrush brush && brush.Color != Colors.Transparent)? brush :  Color3Container;
	public Brush Color4TagBrush => (Color4State is SolidColorBrush brush && brush.Color != Colors.Transparent)? brush :  Color4Container;
	public Brush Color5TagBrush => (Color5State is SolidColorBrush brush && brush.Color != Colors.Transparent) ? brush : Color5Container;

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


