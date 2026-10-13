// TEMPORARY - stand-in for Uno's input validation until unoplatform/uno#24838 is merged and published.
// Delete this folder when bumping to that version: the framework ships the same types in the same namespace,
// and drives the same visual states and ErrorPresenter part that the Simple input templates declare.
#if HAS_UNO
#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions;
using Windows.Foundation.Collections;

namespace Uno.Extras.Input;

/// <summary>
/// Input validation for TextBox, PasswordBox, AutoSuggestBox and ComboBox: with <see cref="ModeProperty"/> set to
/// <see cref="InputValidationMode.Auto"/>, the errors that the binding source reports through
/// <see cref="INotifyDataErrorInfo"/> for the bound property are mirrored into <see cref="ErrorsProperty"/> and
/// presented by the control template.
/// </summary>
/// <remarks>
/// Temporary implementation with the framework's API surface. It drives the template the way the framework does:
/// the <c>InputValidationEnabledStates</c> / <c>InputValidationErrorStates</c> visual state groups, and an
/// <c>ErrorPresenter</c> part filled with the <see cref="ErrorTemplateProperty"/> content (Inline) or with the
/// <c>DefaultCompactErrorIconTemplate</c> resource carrying that content in its tooltip (Compact).
/// Limitations: only <c>{Binding}</c> with a single-segment path is tracked (no <c>{x:Bind}</c>), and errors are
/// tracked while the control is loaded.
/// </remarks>
[Microsoft.UI.Xaml.Data.Bindable]
public static class Validation
{
	private const string ErrorPresenterPartName = "ErrorPresenter";
	private const string CompactErrorIconTemplateKey = "DefaultCompactErrorIconTemplate";

	#region DependencyProperty: Mode

	/// <summary>
	/// Identifies the Mode attached property: whether the control validates its bound value.
	/// </summary>
	public static DependencyProperty ModeProperty { [DynamicDependency(nameof(GetMode))] get; } = DependencyProperty.RegisterAttached(
		"Mode",
		typeof(InputValidationMode),
		typeof(Validation),
		new PropertyMetadata(InputValidationMode.Default, (d, e) => GetTracker(d)?.Refresh(forceSync: true)));

	/// <summary>Gets whether the control validates its bound value.</summary>
	[DynamicDependency(nameof(SetMode))]
	public static InputValidationMode GetMode(Control control) => (InputValidationMode)control.GetValue(ModeProperty);

	/// <summary>Sets whether the control validates its bound value.</summary>
	[DynamicDependency(nameof(GetMode))]
	public static void SetMode(Control control, InputValidationMode value) => control.SetValue(ModeProperty, value);

	#endregion

	#region DependencyProperty: Kind

	/// <summary>
	/// Identifies the Kind attached property: how the control presents its errors.
	/// </summary>
	public static DependencyProperty KindProperty { [DynamicDependency(nameof(GetKind))] get; } = DependencyProperty.RegisterAttached(
		"Kind",
		typeof(InputValidationKind),
		typeof(Validation),
		new PropertyMetadata(InputValidationKind.Auto, (d, e) => GetTracker(d, create: false)?.UpdatePresentation()));

	/// <summary>Gets how the control presents its errors.</summary>
	[DynamicDependency(nameof(SetKind))]
	public static InputValidationKind GetKind(Control control) => (InputValidationKind)control.GetValue(KindProperty);

	/// <summary>Sets how the control presents its errors.</summary>
	[DynamicDependency(nameof(GetKind))]
	public static void SetKind(Control control, InputValidationKind value) => control.SetValue(KindProperty, value);

	#endregion

	#region DependencyProperty: ErrorTemplate

	/// <summary>
	/// Identifies the ErrorTemplate attached property: the template presenting the errors. Nothing is presented
	/// while it is null.
	/// </summary>
	public static DependencyProperty ErrorTemplateProperty { [DynamicDependency(nameof(GetErrorTemplate))] get; } = DependencyProperty.RegisterAttached(
		"ErrorTemplate",
		typeof(DataTemplate),
		typeof(Validation),
		new PropertyMetadata(default(DataTemplate), (d, e) => GetTracker(d, create: false)?.UpdatePresentation()));

	/// <summary>Gets the template presenting the errors.</summary>
	[DynamicDependency(nameof(SetErrorTemplate))]
	public static DataTemplate? GetErrorTemplate(Control control) => (DataTemplate?)control.GetValue(ErrorTemplateProperty);

	/// <summary>Sets the template presenting the errors.</summary>
	[DynamicDependency(nameof(GetErrorTemplate))]
	public static void SetErrorTemplate(Control control, DataTemplate? value) => control.SetValue(ErrorTemplateProperty, value);

	#endregion

	#region DependencyProperty: HasErrors

	/// <summary>
	/// Identifies the read-only HasErrors attached property: whether the control currently has errors.
	/// </summary>
	public static DependencyProperty HasErrorsProperty { [DynamicDependency(nameof(GetHasErrors))] get; } = DependencyProperty.RegisterAttached(
		"HasErrors",
		typeof(bool),
		typeof(Validation),
		new PropertyMetadata(false));

	/// <summary>Gets whether the control currently has errors.</summary>
	public static bool GetHasErrors(Control control) => (bool)control.GetValue(HasErrorsProperty);

	#endregion

	#region DependencyProperty: Errors

	/// <summary>
	/// Identifies the read-only Errors attached property: the errors the control presents.
	/// </summary>
	public static DependencyProperty ErrorsProperty { [DynamicDependency(nameof(GetErrors))] get; } = DependencyProperty.RegisterAttached(
		"Errors",
		typeof(IObservableVector<InputValidationError>),
		typeof(Validation),
		new PropertyMetadata(default(IObservableVector<InputValidationError>)));

	/// <summary>Gets the errors the control presents, created on first read.</summary>
	public static IObservableVector<InputValidationError> GetErrors(Control control)
	{
		if (control.GetValue(ErrorsProperty) is not IObservableVector<InputValidationError> errors)
		{
			errors = new InputValidationErrorCollection();
			control.SetValue(ErrorsProperty, errors);
		}

		return errors;
	}

	#endregion

	#region DependencyProperty: Tracker (private)

	private static DependencyProperty TrackerProperty { get; } = DependencyProperty.RegisterAttached(
		"Tracker",
		typeof(ValidationTracker),
		typeof(Validation),
		new PropertyMetadata(default(ValidationTracker)));

	private static ValidationTracker? GetTracker(DependencyObject d, bool create = true)
	{
		if (d is not Control control)
		{
			return null;
		}

		var tracker = control.GetValue(TrackerProperty) as ValidationTracker;
		if (tracker is null && create)
		{
			tracker = new ValidationTracker(control);
			control.SetValue(TrackerProperty, tracker);
		}

		return tracker;
	}

	#endregion

	/// <summary>The property each supported control validates.</summary>
	private static DependencyProperty? GetValidatedProperty(Control control) => control switch
	{
		TextBox => TextBox.TextProperty,
		PasswordBox => PasswordBox.PasswordProperty,
		AutoSuggestBox => AutoSuggestBox.TextProperty,
		ComboBox => Selector.SelectedItemProperty,
		_ => null,
	};

	/// <summary>
	/// Mirrors the binding source's errors onto one control and drives its template. Subscribed to the source only
	/// while the control is loaded, so the source never keeps an unloaded control alive.
	/// </summary>
	private sealed class ValidationTracker
	{
		private readonly Control _control;
		private readonly DependencyProperty? _validatedProperty;

		private INotifyDataErrorInfo? _source;
		private string? _propertyName;

		private ContentPresenter? _presenter;
		private InputValidationKind _presentedKind;
		private DataTemplate? _presentedTemplate;
		private FrameworkElement? _presentedTemplateRoot;

		public ValidationTracker(Control control)
		{
			_control = control;
			_validatedProperty = GetValidatedProperty(control);

			control.Loaded += (s, e) => Refresh(forceSync: true);
			control.Unloaded += (s, e) => DetachSource();
			control.DataContextChanged += (s, e) => Refresh(forceSync: false);

			// Stands in for OnApplyTemplate, which the framework hooks: a control laid out for the first time (e.g. once a
			// collapsed ancestor shows) or re-templated has a template root its visual states were never applied to.
			control.SizeChanged += (s, e) =>
			{
				if (!ReferenceEquals(GetTemplateRoot(), _presentedTemplateRoot))
				{
					UpdatePresentation();
				}
			};

			if (_validatedProperty is { } property)
			{
				// The binding resolving, or being replaced, writes the validated property.
				control.RegisterPropertyChangedCallback(property, (s, dp) => Refresh(forceSync: false));
			}
		}

		/// <summary>Re-resolves the source; syncs the errors when it changed, or when <paramref name="forceSync"/> is set.</summary>
		public void Refresh(bool forceSync)
		{
			try
			{
				if (GetMode(_control) != InputValidationMode.Auto || _validatedProperty is null)
				{
					DetachSource();
					SyncErrors();
					return;
				}

				if (!_control.IsLoaded)
				{
					return;
				}

				var expression = _control.GetBindingExpression(_validatedProperty);
				var source = expression?.DataItem as INotifyDataErrorInfo;
				var propertyName = expression?.ParentBinding?.Path?.Path;

				if (!ReferenceEquals(source, _source) || propertyName != _propertyName)
				{
					DetachSource();
					_source = source;
					_propertyName = propertyName;
					if (_source is not null)
					{
						_source.ErrorsChanged += OnSourceErrorsChanged;
					}
				}
				else if (!forceSync)
				{
					return;
				}

				SyncErrors();
			}
			catch (Exception ex)
			{
				LogFailure(ex);
			}
		}

		private void DetachSource()
		{
			if (_source is not null)
			{
				_source.ErrorsChanged -= OnSourceErrorsChanged;
				_source = null;
				_propertyName = null;
			}
		}

		private void OnSourceErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
		{
			// A null or empty name is the "all properties" convention.
			if (!string.IsNullOrEmpty(e.PropertyName) && e.PropertyName != _propertyName)
			{
				return;
			}

			if (_control.DispatcherQueue.HasThreadAccess)
			{
				SyncErrorsSafe();
			}
			else
			{
				_control.DispatcherQueue.TryEnqueue(SyncErrorsSafe);
			}
		}

		private void SyncErrorsSafe()
		{
			try
			{
				SyncErrors();
			}
			catch (Exception ex)
			{
				LogFailure(ex);
			}
		}

		private void SyncErrors()
		{
			var messages = _source is not null && !string.IsNullOrEmpty(_propertyName)
				? _source.GetErrors(_propertyName)?
					.Cast<object?>()
					.Select(error => error is InputValidationError validationError ? validationError.ErrorMessage : error?.ToString())
					.Where(message => !string.IsNullOrEmpty(message))
					.Cast<string>()
					.ToArray() ?? []
				: [];

			var errors = GetErrors(_control);
			if (!errors.Select(error => error.ErrorMessage).SequenceEqual(messages))
			{
				errors.Clear();
				foreach (var message in messages)
				{
					errors.Add(new InputValidationError(message));
				}
			}

			_control.SetValue(HasErrorsProperty, errors.Count != 0);
			UpdatePresentation();
		}

		/// <summary>Moves the template to the visual states matching the mode, kind and errors.</summary>
		public void UpdatePresentation()
		{
			try
			{
				_presentedTemplateRoot = GetTemplateRoot();
				var isEnabled = GetMode(_control) == InputValidationMode.Auto && _validatedProperty is not null;
				var isInline = GetKind(_control) == InputValidationKind.Inline;

				VisualStateManager.GoToState(
					_control,
					!isEnabled ? "ValidationDisabled" : isInline ? "InlineValidationEnabled" : "CompactValidationEnabled",
					false);

				// The presenter is realized and filled before the error state targets it.
				if (isEnabled && GetHasErrors(_control) && EnsurePresenterContent(isInline ? InputValidationKind.Inline : InputValidationKind.Compact))
				{
					VisualStateManager.GoToState(_control, isInline ? "InlineErrors" : "CompactErrors", false);
				}
				else
				{
					VisualStateManager.GoToState(_control, "ErrorsCleared", false);
				}
			}
			catch (Exception ex)
			{
				LogFailure(ex);
			}
		}

		/// <summary>Fills the ErrorPresenter part for <paramref name="kind"/>; false when there is nothing to present with.</summary>
		private bool EnsurePresenterContent(InputValidationKind kind)
		{
			if (GetErrorTemplate(_control) is not { } template
				|| GetTemplateRoot() is not { } templateRoot
				|| templateRoot.FindName(ErrorPresenterPartName) is not ContentPresenter presenter)
			{
				return false;
			}

			if (ReferenceEquals(presenter, _presenter) && kind == _presentedKind && ReferenceEquals(template, _presentedTemplate))
			{
				return true;
			}

			// The template content binds to the control's attached Errors, which GetErrors has already created.
			var errorContent = template.LoadContent() as FrameworkElement;
			if (errorContent is not null)
			{
				errorContent.DataContext = _control;
			}

			if (kind == InputValidationKind.Compact
				&& FindResource(CompactErrorIconTemplateKey) is DataTemplate iconTemplate
				&& iconTemplate.LoadContent() is FrameworkElement icon)
			{
				if (ToolTipService.GetToolTip(icon) is ToolTip toolTip)
				{
					toolTip.Content = errorContent;
				}
				else
				{
					ToolTipService.SetToolTip(icon, errorContent);
				}

				presenter.Content = icon;
			}
			else
			{
				presenter.Content = errorContent;
			}

			_presenter = presenter;
			_presentedKind = kind;
			_presentedTemplate = template;
			return true;
		}

		private FrameworkElement? GetTemplateRoot() =>
			VisualTreeHelper.GetChildrenCount(_control) > 0 ? VisualTreeHelper.GetChild(_control, 0) as FrameworkElement : null;

		/// <summary>Looks the key up from the control outwards, then at application level, where the framework looks it up.</summary>
		private object? FindResource(string key)
		{
			for (DependencyObject? current = _control; current is not null; current = VisualTreeHelper.GetParent(current))
			{
				if (current is FrameworkElement element && element.Resources.TryGetValue(key, out var value))
				{
					return value;
				}
			}

			return Application.Current?.Resources.TryGetValue(key, out var appValue) == true ? appValue : null;
		}

		private void LogFailure(Exception ex)
		{
			if (this.Log().IsEnabled(LogLevel.Warning))
			{
				this.Log().LogWarning(ex, "Input validation could not update {Control}; its errors may not be presented.", _control.GetType().Name);
			}
		}
	}
}
#endif
