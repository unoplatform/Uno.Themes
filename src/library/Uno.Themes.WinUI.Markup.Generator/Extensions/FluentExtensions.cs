namespace Uno.Markup.Extensions;

public static class FluentExtensions
{
	public static T? As<T>(this object? x) where T : class => x as T;
	public static TResult Apply<T, TResult>(this T value, Func<T, TResult> selector)
	{
		return selector(value);
	}
}
