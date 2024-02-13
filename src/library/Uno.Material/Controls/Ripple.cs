using System;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Material;

/*
 * Starting implementation acknowledgement from project https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/2740f14a814896d42032ae0013b765a8a0ec04c3/MaterialDesignThemes.Uwp/Ripple.cs
 */
public partial class Ripple : ContentControl
{
	public Ripple()
	{
		DefaultStyleKey = typeof(Ripple);
		SizeChanged += OnSizeChanged;

#if NETSTANDARD2_0
		SetBinding(MyParentProperty, new Binding { RelativeSource = new RelativeSource { Mode = RelativeSourceMode.TemplatedParent } });
		Loaded += ReRegisterPointerPressedHandler;
		Unloaded += UnRegisterPointerPressedHandler;
#endif
	}

	private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
	{
		RippleSize = Math.Max(sizeChangedEventArgs.NewSize.Width, sizeChangedEventArgs.NewSize.Height);
		Clip = new RectangleGeometry { Rect = new Windows.Foundation.Rect(0, 0, sizeChangedEventArgs.NewSize.Width, sizeChangedEventArgs.NewSize.Height) };
	}

	public static readonly DependencyProperty FeedbackProperty = DependencyProperty.Register(
		"Feedback", typeof(Brush), typeof(Ripple), new PropertyMetadata(default(Brush)));

	public Brush Feedback
	{
		get { return (Brush)GetValue(FeedbackProperty); }
		set { SetValue(FeedbackProperty, value); }
	}

	/// <summary>
	/// Opacity of the feedback ripple effect
	/// </summary>
	/// <remarks>Using the Opacity property would affect it's content, while this property only affects the feedback</remarks>
	public double FeedbackOpacity
	{
		get { return (double)GetValue(FeedbackOpacityProperty); }
		set { SetValue(FeedbackOpacityProperty, value); }
	}

	public static readonly DependencyProperty FeedbackOpacityProperty =
		DependencyProperty.Register("FeedbackOpacity", typeof(double), typeof(Ripple), new PropertyMetadata(1));

	protected override void OnApplyTemplate()
	{
		VisualStateManager.GoToState(this, "Normal", false);

		base.OnApplyTemplate();
	}

	// Workaround for themes#373 WASM nested Ripple control
#if NETSTANDARD2_0
	private UIElement _touchTarget;

	private static uint _lastHandledFrameId = 0;

	private static readonly DependencyProperty MyParentProperty = DependencyProperty.Register(
		"MyParent", typeof(object), typeof(Ripple), new PropertyMetadata(default(object), (snd, e) => (snd as Ripple)?.ReRegisterPointerPressedHandler()));

	private static void ReRegisterPointerPressedHandler(object snd, RoutedEventArgs _)
		=> (snd as Ripple)?.ReRegisterPointerPressedHandler();

	private void ReRegisterPointerPressedHandler()
	{
		// On WASM when the Ripple control is nested into a control that does capture the pointer,
		// we won't receive the pointer events as we should.
		// **Noticeably, it's the case for all buttons. **
		// To work around this issue, we are listening pointers directly on the TemplatedParent if possible.

		var updatedTouchTarget = GetValue(MyParentProperty) as UIElement ?? this;
		if (_touchTarget == updatedTouchTarget)
		{
			return;
		}

		UnRegisterPointerPressedHandler();

		_touchTarget = updatedTouchTarget;
		_touchTarget.AddHandler(PointerPressedEvent, new PointerEventHandler(StartRippling), handledEventsToo: true);
	}

	private static void UnRegisterPointerPressedHandler(object snd, RoutedEventArgs _)
		=> (snd as Ripple)?.UnRegisterPointerPressedHandler();

	private void UnRegisterPointerPressedHandler()
	{
		_touchTarget?.RemoveHandler(PointerPressedEvent, new PointerEventHandler(StartRippling));
		_touchTarget = null;
	}
#else
	protected override void OnPointerPressed(PointerRoutedEventArgs e)
	{
		StartRippling(null, e);
		base.OnPointerPressed(e);
	}

	// Uncomment for hover effect
	//protected override void OnPointerMoved(PointerRoutedEventArgs e)
	//{
	//	var point = e.GetCurrentPoint(this);

	//	RippleX = point.Position.X - RippleSize / 2;
	//	RippleY = point.Position.Y - RippleSize / 2;

	//	VisualStateManager.GoToState(this, "Normal", true);
	//	VisualStateManager.GoToState(this, "PointerOver", true);

	//	base.OnPointerMoved(e);
	//}
#endif

	private void StartRippling(object _, PointerRoutedEventArgs e)
	{
		var point = e.GetCurrentPoint(this);

		// workaround for uno#5033 and uno#5874 nested `Ripple` controls all ripples, instead of just the innermost (side effect of workaround themes#373)
#if NETSTANDARD2_0
		// Because the event could've been already handled by other control, e.Handled cannot be used here.
		// FrameId is used instead to verify if a ripple has already occurred with this Id, to prevent the next on the line to ripple again.
		if (point.FrameId <= _lastHandledFrameId)
		{
			return;
		}

		_lastHandledFrameId = point.FrameId;
#endif

		RippleX = point.Position.X - RippleSize / 2;
		RippleY = point.Position.Y - RippleSize / 2;

		VisualStateManager.GoToState(this, "Normal", false);
		VisualStateManager.GoToState(this, "Pressed", true);
	}

	public static readonly DependencyProperty RippleSizeMultiplierProperty = DependencyProperty.Register(
		"RippleSizeMultiplier", typeof(double), typeof(Ripple), new PropertyMetadata(2 * Math.Sqrt(2)));

	public double RippleSizeMultiplier
	{
		get => (double)GetValue(RippleSizeMultiplierProperty);
		set => SetValue(RippleSizeMultiplierProperty, value);
	}

	public static readonly DependencyProperty RippleSizeProperty = DependencyProperty.Register(
		"RippleSize", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

	public double RippleSize
	{
		get => (double)GetValue(RippleSizeProperty);
		private set => SetValue(RippleSizeProperty, value);
	}
	public static readonly DependencyProperty RippleYProperty = DependencyProperty.Register(
		"RippleY", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

	public double RippleY
	{
		get => (double)GetValue(RippleYProperty);
		private set => SetValue(RippleYProperty, value);
	}
	public static readonly DependencyProperty RippleXProperty = DependencyProperty.Register(
		"RippleX", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

	public double RippleX
	{
		get => (double)GetValue(RippleXProperty);
		private set => SetValue(RippleXProperty, value);
	}
}
