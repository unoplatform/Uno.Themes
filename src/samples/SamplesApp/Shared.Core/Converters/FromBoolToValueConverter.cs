namespace Uno.Themes.Samples.Converters;

public class FromBoolToValueConverter : IValueConverter
{
	public object? NullValue { get; set; }

	public object? TrueValue { get; set; }

	public object? FalseValue { get; set; }

	public object? NullOrTrueValue
	{
		get => TrueValue;
		set => NullValue = TrueValue = value;
	}

	public object? NullOrFalseValue
	{
		get => FalseValue;
		set => NullValue = FalseValue = value;
	}

	public object? Convert(object? value, Type targetType, object? parameter, string language) => value is bool x
		? (x ? TrueValue : FalseValue)
		: NullValue;

	public object ConvertBack(object? value, Type targetType, object? parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
}
