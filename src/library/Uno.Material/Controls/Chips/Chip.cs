using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Uno.Material.Controls
{
	public partial class Chip : ToggleButton
	{
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

		private bool _isMuted = false;

		public Chip()
		{
			Checked += RaiseIsCheckedChanged;
			Unchecked += RaiseIsCheckedChanged;
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
