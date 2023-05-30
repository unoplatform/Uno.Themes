using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Windows.UI;
using Uno.Disposables;
using System.Windows.Input;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
#endif

namespace Uno.Material
{
	[Bindable]
	public static class TextBoxExtensions
	{
		#region DependencyProperty: LeadingIcon

		public static DependencyProperty LeadingIconProperty { [DynamicDependency(nameof(GetLeadingIcon))] get; } = DependencyProperty.RegisterAttached(
			"LeadingIcon",
			typeof(IconElement),
			typeof(TextBoxExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(SetLeadingIcon))]
		public static IconElement GetLeadingIcon(Control obj) => (IconElement)obj.GetValue(LeadingIconProperty);

		[DynamicDependency(nameof(GetLeadingIcon))]
		public static void SetLeadingIcon(Control obj, IconElement value) => obj.SetValue(LeadingIconProperty, value);

		#endregion

		#region DependencyProperty: IsLeadingIconVisible

		public static DependencyProperty IsLeadingIconVisibleProperty { [DynamicDependency(nameof(GetIsLeadingIconVisible))] get; } = DependencyProperty.RegisterAttached(
			"IsLeadingIconVisible",
			typeof(bool),
			typeof(TextBoxExtensions),
			new PropertyMetadata(true));

		[DynamicDependency(nameof(SetIsLeadingIconVisible))]
		public static bool GetIsLeadingIconVisible(Control obj) => (bool)obj.GetValue(IsLeadingIconVisibleProperty);

		[DynamicDependency(nameof(GetIsLeadingIconVisible))]
		public static void SetIsLeadingIconVisible(Control obj, bool value) => obj.SetValue(IsLeadingIconVisibleProperty, value);

		#endregion

		#region DependencyProperty: LeadingCommand
		public static DependencyProperty LeadingCommandProperty { [DynamicDependency(nameof(GetLeadingCommand))] get; } = DependencyProperty.RegisterAttached(
			"LeadingCommand",
			typeof(ICommand),
			typeof(TextBoxExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(GetLeadingCommand))]
		public static ICommand GetLeadingCommand(Control obj) => (ICommand)obj.GetValue(LeadingCommandProperty);

		[DynamicDependency(nameof(SetLeadingCommand))]
		public static void SetLeadingCommand(Control obj, ICommand value) => obj.SetValue(LeadingCommandProperty, value);
		#endregion

		#region DependencyProperty: TrailingIcon

		public static DependencyProperty TrailingIconProperty { [DynamicDependency(nameof(GetTrailingIcon))] get; } = DependencyProperty.RegisterAttached(
			"TrailingIcon",
			typeof(IconElement),
			typeof(TextBoxExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(SetTrailingIcon))]
		public static IconElement GetTrailingIcon(Control obj) => (IconElement)obj.GetValue(TrailingIconProperty);

		[DynamicDependency(nameof(GetTrailingIcon))]
		public static void SetTrailingIcon(Control obj, IconElement value) => obj.SetValue(TrailingIconProperty, value);
		#endregion

		#region DependencyProperty: IsTrailingIconVisible

		public static DependencyProperty IsTrailingIconVisibleProperty { [DynamicDependency(nameof(GetIsTrailingIconVisible))] get; } = DependencyProperty.RegisterAttached(
			"IsTrailingIconVisible",
			typeof(bool),
			typeof(TextBoxExtensions),
			new PropertyMetadata(true));

		[DynamicDependency(nameof(SetIsTrailingIconVisible))]
		public static bool GetIsTrailingIconVisible(Control obj) => (bool)obj.GetValue(IsTrailingIconVisibleProperty);

		[DynamicDependency(nameof(GetIsTrailingIconVisible))]
		public static void SetIsTrailingIconVisible(Control obj, bool value) => obj.SetValue(IsTrailingIconVisibleProperty, value);

		#endregion

		#region DependencyProperty: TrailingCommand
		public static DependencyProperty TrailingCommandProperty { [DynamicDependency(nameof(GetTrailingCommand))] get; } = DependencyProperty.RegisterAttached(
			"TrailingCommand",
			typeof(ICommand),
			typeof(TextBoxExtensions),
			new PropertyMetadata(default));

		[DynamicDependency(nameof(GetTrailingCommand))]
		public static ICommand GetTrailingCommand(Control obj) => (ICommand)obj.GetValue(TrailingCommandProperty);

		[DynamicDependency(nameof(SetTrailingCommand))]
		public static void SetTrailingCommand(Control obj, ICommand value) => obj.SetValue(TrailingCommandProperty, value);
		#endregion
	}
}
