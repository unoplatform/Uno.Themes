using System.Windows.Input;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Material.Controls
{
	public enum SnackBarStatus
	{
		[FallbackValue]
		Default = 0,
		Visible = 1,
		Hidden = 2,
	}

	[TemplateVisualState(Name = "Default", GroupName = SnackBarStatesName)]
	[TemplateVisualState(Name = "Visible", GroupName = SnackBarStatesName)]
	[TemplateVisualState(Name = "Hidden", GroupName = SnackBarStatesName)]

	public partial class SnackBar : Control
	{
		private const string SnackBarStatesName = "SnackBarStates";
		private bool _isLoaded = false; // This flag indicates whether the control is attached to the visual tree (Loaded/Unloaded events).
		private bool _isVisualResetRequired = true; // This flag indicates whether the initial visual states should be applied.
		private string _visualState = "Default"; // This flag indicates whether the initial visual states should be applied.

		public SnackBar()
		{
			DefaultStyleKey = typeof(SnackBar);

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}

		private void Reset()
		{
			if (_isVisualResetRequired)
			{
				_isVisualResetRequired = false;
				VisualStateManager.GoToState(this, _visualState, useTransitions: true);
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = true;
			Reset();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = false;
		}

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(
				nameof(Text),
				typeof(string),
				typeof(SnackBar),
				new PropertyMetadata(string.Empty));

		public string ActionLabel
		{
			get => (string)GetValue(ActionLabelProperty);
			set => SetValue(ActionLabelProperty, value);
		}

		public static readonly DependencyProperty ActionLabelProperty =
			DependencyProperty.Register(
				nameof(ActionLabel),
				typeof(string),
				typeof(SnackBar),
				new PropertyMetadata(string.Empty));

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register(
				nameof(Command),
				typeof(ICommand),
				typeof(SnackBar),
				new PropertyMetadata(null));

		public SnackBarStatus SnackBarStatus
		{
			get { return (SnackBarStatus)GetValue(SnackBarStatusProperty); }
			set { SetValue(SnackBarStatusProperty, value); }
		}

		public static readonly DependencyProperty SnackBarStatusProperty =
			DependencyProperty.Register(
				"SnackBarStatus",
				typeof(SnackBarStatus),
				typeof(SnackBar),
				new PropertyMetadata(SnackBarStatus.Default, SnackBarStatusChanged));

		private static void SnackBarStatusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			var control = sender as SnackBar;

			switch ((SnackBarStatus)args.NewValue)
			{
				case SnackBarStatus.Visible:
					control._visualState = "Visible";
					break;
				case SnackBarStatus.Hidden:
					control._visualState = "Hidden";
					break;
				default:
					control._visualState = "Default";
					break;
			}

			// Visual state can only be applied when control is loaded. 
			if (control._isLoaded)
			{
				VisualStateManager.GoToState(control, control._visualState, useTransitions: false);
			}
			else
			{
				control._isVisualResetRequired = true;
			}
		}
	}
}
