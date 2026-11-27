using System.ComponentModel;

namespace Uno.Themes.Samples;

/// <summary>
/// Displays the errors that an <see cref="INotifyDataErrorInfo"/> <see cref="Source"/> reports for one
/// <see cref="Property"/>, straight from the source, independently of how (or whether) the validated
/// control renders them.
/// </summary>
public sealed partial class IndeiStateViewer : UserControl
{
	#region DependencyProperty: Source

	public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(
		nameof(Source),
		typeof(object),
		typeof(IndeiStateViewer),
		new PropertyMetadata(default, (s, e) => ((IndeiStateViewer)s).OnSourceChanged()));

	/// <summary>The object to read the errors from, normally an <see cref="INotifyDataErrorInfo"/>.</summary>
	public object? Source
	{
		get => GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	#endregion

	#region DependencyProperty: Property

	public static DependencyProperty PropertyProperty { get; } = DependencyProperty.Register(
		nameof(Property),
		typeof(string),
		typeof(IndeiStateViewer),
		new PropertyMetadata(default, (s, e) => ((IndeiStateViewer)s).Refresh()));

	/// <summary>The name of the property whose errors are displayed.</summary>
	public string? Property
	{
		get => (string?)GetValue(PropertyProperty);
		set => SetValue(PropertyProperty, value);
	}

	#endregion

	private const double TextSize = 12;

	private readonly TextBlock _summary = new() { FontSize = TextSize };
	private readonly StackPanel _errors = new();
	private INotifyDataErrorInfo? _subscribedSource;

	public IndeiStateViewer()
	{
		Content = new StackPanel { Spacing = 2, Children = { _summary, _errors } };

		Loaded += (s, e) => Subscribe();
		Unloaded += (s, e) => Unsubscribe();
	}

	private void OnSourceChanged()
	{
		Unsubscribe();
		if (IsLoaded)
		{
			Subscribe();
		}

		Refresh();
	}

	private void Subscribe()
	{
		if (_subscribedSource is null && Source is INotifyDataErrorInfo source)
		{
			source.ErrorsChanged += OnErrorsChanged;
			_subscribedSource = source;
		}

		Refresh();
	}

	private void Unsubscribe()
	{
		if (_subscribedSource is { } source)
		{
			source.ErrorsChanged -= OnErrorsChanged;
			_subscribedSource = null;
		}
	}

	private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		// A null or empty name is the "all properties" convention.
		if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == Property)
		{
			Refresh();
		}
	}

	private void Refresh()
	{
		_errors.Children.Clear();

		if (Source is not INotifyDataErrorInfo source)
		{
			_summary.Text = $"INDEI {Property}: (no INotifyDataErrorInfo source)";
			return;
		}

		var errors = source.GetErrors(Property)?
			.Cast<object?>()
			.Select(error => error?.ToString())
			.OfType<string>()
			.ToArray() ?? [];

		_summary.Text = errors.Length == 0
			? $"INDEI {Property}: no errors"
			: $"INDEI {Property}: {errors.Length} error(s)";

		foreach (var error in errors)
		{
			_errors.Children.Add(new TextBlock { Text = $"• {error}", FontSize = TextSize, TextWrapping = TextWrapping.Wrap });
		}
	}
}
