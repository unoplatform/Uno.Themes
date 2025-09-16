using System.ComponentModel;

namespace Uno.Themes.Samples.Helpers;

public static class EnumHelper
{
	public static T GetAttribute<T>(this Enum @enum) where T : Attribute
	{
		return @enum.GetType()
			.GetField(@enum.ToString())
			.GetCustomAttribute<T>();
	}

	/// <summary>
	/// Get the description value from DescriptionAttribute
	/// </summary>
	public static string GetDescription(this Enum @enum) => @enum.GetAttribute<DescriptionAttribute>()?.Description;
}
