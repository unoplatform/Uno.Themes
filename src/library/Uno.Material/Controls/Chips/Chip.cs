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

		#region DependencyProperty: Thumbnail

		public object Thumbnail
		{
			get { return (object)GetValue(ThumbnailProperty); }
			set { SetValue(ThumbnailProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailProperty =
			DependencyProperty.Register("Thumbnail", typeof(object), typeof(Chip), new PropertyMetadata(null));

		#endregion

		#region DependencyProperty: ThumbnailTemplate

		public DataTemplate ThumbnailTemplate
		{
			get { return (DataTemplate)GetValue(ThumbnailTemplateProperty); }
			set { SetValue(ThumbnailTemplateProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailTemplateProperty =
			DependencyProperty.Register("ThumbnailTemplate", typeof(DataTemplate), typeof(Chip), new PropertyMetadata(null));

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
