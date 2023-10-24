using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Markup.Xaml;

namespace Uno.Markup.Extensions;

public static class KeyedResourceExtensions
{
	public static bool IsReferenceFor<T>(this IKeyedResource x)
	{
		return x switch
		{
			StaticResource sr => sr.Value is T,
			ThemeResource tr => tr.LightValue is T && tr.DarkValue is T,

			_ => false,
		};
	}
	public static bool IsStaticResourceFor<T>(this IKeyedResource x) => (x as StaticResource)?.Value is T;
	public static bool IsThemeResourceFor<T>(this IKeyedResource x) => x is ThemeResource tr && tr.LightValue is T && tr.DarkValue is T;
#pragma warning disable CS8600 // fixme: Converting null literal or possible null value to non-nullable type. 
	public static T? StaticValue<T>(this IKeyedResource x) => (T)((StaticResource)x).Value;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

	public static bool IsUnresolved(this IKeyedResource x)
	{
		return x switch
		{
			StaticResource sr when sr.Value is StaticResourceRef or ThemeResourceRef => true,
			ThemeResource tr when
				(tr.LightValue is StaticResourceRef or ThemeResourceRef) &&
				(tr.DarkValue is StaticResourceRef or ThemeResourceRef) => true,

			_ => false,
		};
	}

	public static IEnumerable<T> OfStaticResourceOf<T>(this IEnumerable<IKeyedResource> source)
	{
		return source
			.Select(x => (x as StaticResource)?.Value)
			.OfType<T>();
	}
	public static IEnumerable<StaticResource> OfStaticResource<T>(this IEnumerable<IKeyedResource> source)
	{
		return source.OfType<StaticResource>()
			.Where(x => x.IsStaticResourceFor<T>());
	}
	public static IEnumerable<ThemeResource> OfThemeResource<T>(this IEnumerable<IKeyedResource> source)
	{
		return source.OfType<ThemeResource>()
			.Where(x => x.IsThemeResourceFor<T>());
	}
}
