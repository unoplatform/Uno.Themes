namespace Uno.Themes.Samples.Converters;

public class FromStringToValueConverter : IValueConverter
{
	public enum CheckMethod { IsNullOrEmpty, IsNullOrWhitespace, IsEqualToParameterValue}

	public CheckMethod Check { get; set; }

	public object TrueValue { get; set; }

	public object FalseValue { get; set; }

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		// A null string is "null or empty": without this, a sample with no description or documentation link
		// showed the empty text and the link.
		if (value is null or string)
		{
			var text = value as string;
			if (Check == CheckMethod.IsEqualToParameterValue && parameter is string param)
			{
				return param.Equals(text) ? TrueValue : FalseValue;
			}
			else if (Check == CheckMethod.IsNullOrEmpty)
			{
				return string.IsNullOrEmpty(text) ? TrueValue : FalseValue;
			}
			else if (Check == CheckMethod.IsNullOrWhitespace)
			{
				return string.IsNullOrWhiteSpace(text) ? TrueValue : FalseValue;
			}
		}
		
		return FalseValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException("Only one-way conversion is supported.");
}
