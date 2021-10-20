using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace Uno.Material.Controls
{
	[TemplatePart(Name = RemoveButtonName, Type = typeof(Button))]
	public partial class Chip : ToggleButton
	{
		private const string RemoveButtonName = "PART_RemoveButton";

		public event ChipRemovingEventHandler Removing;
		public event RoutedEventHandler Removed;

		/// <summary>
		/// Fires when a ToggleButton is checked or unchecked, except when set with <see cref="SetIsCheckedSilently"/>.
		/// </summary>
		internal event RoutedEventHandler IsCheckedChanged;

#region DependencyProperty: IsCheckable = true

		public static DependencyProperty IsCheckableProperty { get; } = DependencyProperty.Register(
			nameof(IsCheckable),
			typeof(bool),
			typeof(Chip),
			new PropertyMetadata(true, (s, e) => (s as Chip)?.OnIsCheckableChanged(e)));

		/// <summary>
		/// Gets or sets whether this should behave like a ToggleButton or a Button
		/// </summary>
		public bool IsCheckable
		{
			get => (bool)GetValue(IsCheckableProperty);
			set => SetValue(IsCheckableProperty, value);
		}

#endregion

#region DependencyProperty: Icon

		public object Icon
		{
			get { return (IconElement)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public static readonly DependencyProperty IconProperty =
			DependencyProperty.Register("Icon", typeof(object), typeof(Chip), new PropertyMetadata(null));

#endregion

#region DependencyProperty: IconTemplate

		public DataTemplate IconTemplate
		{
			get { return (DataTemplate)GetValue(IconTemplateProperty); }
			set { SetValue(IconTemplateProperty, value); }
		}

		public static readonly DependencyProperty IconTemplateProperty =
			DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(Chip), new PropertyMetadata(null));

#endregion

#region DependencyProperty: CanRemove = false

		public bool CanRemove
		{
			get { return (bool)GetValue(CanRemoveProperty); }
			set { SetValue(CanRemoveProperty, value); }
		}

		public static readonly DependencyProperty CanRemoveProperty =
			DependencyProperty.Register("CanRemove", typeof(bool), typeof(Chip), new PropertyMetadata(false));

#endregion

#region DependencyProperty: RemovedCommand

		public static DependencyProperty RemovedCommandProperty { get; } = DependencyProperty.Register(
			nameof(RemovedCommand),
			typeof(ICommand),
			typeof(Chip),
			new PropertyMetadata(default));

		public ICommand RemovedCommand
		{
			get => (ICommand)GetValue(RemovedCommandProperty);
			set => SetValue(RemovedCommandProperty, value);
		}

#endregion

#region DependencyProperty: RemovedCommandParameter

		public static DependencyProperty RemovedCommandParameterProperty { get; } = DependencyProperty.Register(
			nameof(RemovedCommandParameter),
			typeof(object),
			typeof(Chip),
			new PropertyMetadata(default));

		public object RemovedCommandParameter
		{
			get => (object)GetValue(RemovedCommandParameterProperty);
			set => SetValue(RemovedCommandParameterProperty, value);
		}

#endregion

		private bool _isMuted = false;

		public Chip()
		{
			Checked += RaiseIsCheckedChanged;
			Unchecked += RaiseIsCheckedChanged;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (GetTemplateChild(RemoveButtonName) is Button removeButton)
			{
				removeButton.Click += RaiseRemoveButtonClicked;
			}
		}

		private void OnIsCheckableChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!IsCheckable)
			{
				IsChecked = false;
			}
		}

		private void RaiseIsCheckedChanged(object sender, RoutedEventArgs e)
		{
			if (!_isMuted)
			{
				IsCheckedChanged?.Invoke(sender, e);
			}
		}

		private void RaiseRemoveButtonClicked(object sender, RoutedEventArgs e)
		{
			// note: sender is the RemoveButton, do not pass it as the event sender
			// as ChipGroup expect the sender to be an instance of Chip

			if (CanRemove)
			{
				var removingArgs = new ChipRemovingEventArgs();
				Removing?.Invoke(this, removingArgs);

				if (!removingArgs.Cancel)
				{
					Removed?.Invoke(this, e);

					var param = RemovedCommandParameter;
					if (RemovedCommand is ICommand command && command.CanExecute(param))
					{
						command.Execute(param);
					}
				}
			}
		}

		internal void SetIsCheckedSilently(bool? value)
		{
			try
			{
				_isMuted = true;
				IsChecked = value;
			}
			finally
			{
				_isMuted = false;
			}
		}

		protected override void OnToggle()
		{
			if (!IsCheckable) return;

			base.OnToggle();
		}
	}
}
