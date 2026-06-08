#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

#if WinUI
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
#else
using Windows.System;
using Windows.UI.Xaml;
#endif

[assembly: MetadataUpdateHandler(typeof(Uno.Themes.BaseTheme))]

namespace Uno.Themes;

public abstract partial class BaseTheme
{
	private static readonly List<WeakReference<BaseTheme>> _liveInstances = new();
	private static readonly object _liveInstancesLock = new();

	// Captured during construction (XAML init runs on the UI thread) so the
	// hot-reload handler can marshal UpdateSource() back to the right thread.
	private readonly DispatcherQueue? _hotReloadDispatcher = DispatcherQueue.GetForCurrentThread();

	// Names emitted by Uno's XAML code generator for anything that backs the
	// application's static resources. A XAML resource-dictionary edit regenerates
	// the per-assembly "GlobalStaticResources" class and the nested
	// "ResourceDictionarySingleton__<Name>_<hash>" holders (plus their
	// compiler-generated closures, e.g. "<>c"). None of these derive from
	// ResourceDictionary, so an IsAssignableFrom check never sees them.
	private static readonly string[] _resourceTypeMarkers =
	{
		"GlobalStaticResources",
		"ResourceDictionarySingleton",
	};

	/// <summary>
	/// Determines whether a hot-reloaded type represents (or is nested within) the
	/// generated code that builds the application's static resources. Walks the
	/// declaring-type chain so compiler-generated closures (<c>&lt;&gt;c</c>),
	/// whose own name carries no signal, resolve to their owning singleton/global
	/// class. Falls back to the direct <see cref="ResourceDictionary"/> check for
	/// page-level dictionaries that genuinely compile to a subclass.
	/// </summary>
	private static bool AffectsResources(Type? type)
	{
		for (var current = type; current is not null; current = current.DeclaringType)
		{
			var name = current.Name;
			foreach (var marker in _resourceTypeMarkers)
			{
				if (name.Contains(marker, StringComparison.Ordinal))
				{
					return true;
				}
			}
		}

		return type is not null && typeof(ResourceDictionary).IsAssignableFrom(type);
	}

	private static void RegisterInstance(BaseTheme instance)
	{
		lock (_liveInstancesLock)
		{
			for (var i = _liveInstances.Count - 1; i >= 0; i--)
			{
				if (!_liveInstances[i].TryGetTarget(out _))
				{
					_liveInstances.RemoveAt(i);
				}
			}
			_liveInstances.Add(new WeakReference<BaseTheme>(instance));
		}
	}

	/// <summary>
	/// Called by the .NET runtime via <see cref="MetadataUpdateHandlerAttribute"/> when a hot-reload
	/// update is applied. Every live <see cref="BaseTheme"/> instance rebuilds its merged
	/// dictionaries so the updated XAML takes effect immediately. Uno's own hot-reload
	/// pipeline is responsible for refreshing the underlying URI-backed resource
	/// dictionaries; this handler only re-runs the theme's dynamic layers (spacing/shape/
	/// density scales, seed palette, overrides) on top of the refreshed base.
	/// </summary>
	internal static void UpdateApplication(Type[]? updatedTypes)
	{
		if (updatedTypes is null || updatedTypes.Length == 0)
		{
			return;
		}

		var affectsResources = false;
		foreach (var type in updatedTypes)
		{
			if (AffectsResources(type))
			{
				affectsResources = true;
				break;
			}
		}

		if (!affectsResources)
		{
			return;
		}

		List<BaseTheme> alive;
		lock (_liveInstancesLock)
		{
			alive = new List<BaseTheme>(_liveInstances.Count);
			for (var i = _liveInstances.Count - 1; i >= 0; i--)
			{
				if (_liveInstances[i].TryGetTarget(out var theme))
				{
					alive.Add(theme);
				}
				else
				{
					_liveInstances.RemoveAt(i);
				}
			}
		}

		foreach (var theme in alive)
		{
			var dispatcher = theme._hotReloadDispatcher;
			if (dispatcher is null || dispatcher.HasThreadAccess)
			{
				theme.UpdateSource();
			}
			else
			{
				dispatcher.TryEnqueue(theme.UpdateSource);
			}
		}
	}
}
