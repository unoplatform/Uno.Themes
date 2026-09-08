using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>
/// Provides extension methods on <see cref="Application"/> for theme access.
/// </summary>
public static class ApplicationExtensions
{
	/// <summary>
	/// How many levels of <see cref="ResourceDictionary.MergedDictionaries"/> the walk descends.
	/// Real resource graphs are a handful of levels deep; the bound is a guard against a
	/// pathological graph, so that a caller's cost stays predictable even though the visited set
	/// below is what rules out a cycle.
	/// </summary>
	private const int MaxDepth = 32;

	/// <summary>
	/// Gets the <see cref="BaseTheme"/> instance from the given application's resources, wherever
	/// its resource graph merges it: <see cref="ResourceDictionary.MergedDictionaries"/> is walked
	/// breadth-first, so the shallowest theme wins, and within a level the first one does.
	/// Returns <c>null</c> if no <see cref="BaseTheme"/> is found, or if <paramref name="application"/> is <c>null</c>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The walk exists because an application that keeps its design system in a
	/// <see cref="ResourceDictionary"/> of its own — the layout hot-reload guidance recommends,
	/// since <c>App.xaml</c> itself yields no reloadable type — merges the theme one or more levels
	/// below <c>Application.Resources</c>. Repeated dictionaries are visited once, so the diamonds
	/// that resource graphs form by construction (several dictionaries merging one shared palette)
	/// cost nothing extra and a cycle cannot hang the walk.
	/// </para>
	/// <para>
	/// <see cref="ResourceDictionary.ThemeDictionaries"/> is deliberately not walked: a design
	/// system is not an appearance-specific resource, and enumerating that collection is not
	/// supported on all platforms.
	/// </para>
	/// <para>
	/// The match is by type, so this does not reach across an assembly-load-context boundary when
	/// the host isolates <c>Uno.Themes</c> per context — the guest's <see cref="BaseTheme"/> is
	/// then a different type. A hosted application should resolve its own <see cref="Application"/>
	/// from inside its own context and call this on that.
	/// </para>
	/// </remarks>
	/// <param name="application">The application whose resources to search. May be <c>null</c>.</param>
	/// <returns>The theme, or <c>null</c> when the application merges none.</returns>
	public static BaseTheme GetTheme(this Application application)
	{
		if (application?.Resources is not { } resources)
		{
			return null;
		}

		var topLevel = resources.MergedDictionaries;

		// The theme merged straight into Application.Resources is both the layout that already
		// worked and the common one, so it is answered without allocating anything.
		for (var i = 0; i < topLevel.Count; i++)
		{
			if (topLevel[i] is BaseTheme theme)
			{
				return theme;
			}
		}

		return topLevel.Count > 0 ? FindNestedTheme(resources) : null;
	}

	/// <summary>
	/// Continues the search below the top level, breadth-first.
	/// </summary>
	/// <param name="resources">The application's resources, whose own level has already been searched.</param>
	/// <returns>The shallowest theme below the top level, or <c>null</c>.</returns>
	private static BaseTheme FindNestedTheme(ResourceDictionary resources)
	{
		// Reference semantics: ResourceDictionary does not override equality, and identity is what
		// makes a repeated dictionary cheap and a cycle finite.
		var visited = new HashSet<ResourceDictionary>() { resources };
		var current = new List<ResourceDictionary>();
		var next = new List<ResourceDictionary>();

		var topLevel = resources.MergedDictionaries;
		for (var i = 0; i < topLevel.Count; i++)
		{
			if (visited.Add(topLevel[i]))
			{
				current.Add(topLevel[i]);
			}
		}

		// The top level is already searched, so start one below it.
		for (var depth = 2; depth <= MaxDepth && current.Count > 0; depth++)
		{
			foreach (var dictionary in current)
			{
				var merged = dictionary.MergedDictionaries;
				for (var i = 0; i < merged.Count; i++)
				{
					var child = merged[i];
					if (!visited.Add(child))
					{
						continue;
					}

					if (child is BaseTheme theme)
					{
						return theme;
					}

					next.Add(child);
				}
			}

			(current, next) = (next, current);
			next.Clear();
		}

		return null;
	}
}
