using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// The reflection-based sweeps compensating verified Uno 6.7-dev per-ALC cleanup gaps.
/// </summary>
/// <remarks>
/// Each gap is filed upstream with a repro and a suggested fix; delete each sweep when its
/// fix ships:
/// <list type="bullet">
/// <item>unoplatform/uno#24075 — guest finalizers re-populate swept per-ALC caches during unload.</item>
/// <item>unoplatform/uno#24073 — RemoveNonDefaultAlcEntries misses cross-ALC _getPropertyCache entries.</item>
/// <item>unoplatform/uno#24074 — ALC teardown does not prune SystemNavigationManager subscriptions.</item>
/// </list>
/// unoplatform/uno#24076 (native X11 window/GL context leak) has no host-side sweep.
/// Full write-ups in specs/05-alc-wrapper-app/upstream-issues.md.
/// Internal API by necessity; every step degrades to a logged warning (memory stays resident
/// until the next guest exits), never an exception.
/// </remarks>
internal sealed partial class GuestAppLoader
{
	// Same sweep ExitAlcApplication runs, needed a second time after guest finalizers finish.
	// Upstream: unoplatform/uno#24075.
	private static readonly MethodInfo? _cleanupNonDefaultAlcCaches =
		SafeGetMethod(typeof(Application), "CleanupNonDefaultAlcCaches", BindingFlags.Static | BindingFlags.NonPublic);

	// DependencyProperty._getPropertyCache memoizes (targetType, "ns:Owner.Property") -> DP
	// lookups from style/VSM target paths. A guest style targeting an attached property on a
	// framework element caches a DEFAULT-ALC key (e.g. Button) with a GUEST-ALC value, which
	// Uno's per-key ALC sweep can never remove — pinning the whole guest ALC (verified via
	// heap dump). It is a pure cache over DependencyPropertyRegistry, so clearing it wholesale
	// is safe; it repopulates on demand. Upstream: unoplatform/uno#24073.
	private static readonly FieldInfo? _getPropertyCacheField =
		SafeGetField(typeof(DependencyProperty), "_getPropertyCache", BindingFlags.Static | BindingFlags.NonPublic);
	private static readonly MethodInfo? _getPropertyCacheClear =
		_getPropertyCacheField is { } cacheField
			? SafeGetMethod(cacheField.FieldType, "Clear", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			: null;

	// Lookup failures surface at the sweep call sites (which log); throwing here would turn a
	// future Uno rename into a TypeInitializationException at app launch.
	private static MethodInfo? SafeGetMethod(Type type, string name, BindingFlags flags)
	{
		try
		{
			return type.GetMethod(name, flags);
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static FieldInfo? SafeGetField(Type type, string name, BindingFlags flags)
	{
		try
		{
			return type.GetField(name, flags);
		}
		catch (Exception)
		{
			return null;
		}
	}

	private void SweepNonDefaultAlcCaches()
	{
		// Each mitigation is independent: one failing must not skip the others.
		try
		{
			if (_cleanupNonDefaultAlcCaches is { } cleanup)
			{
				cleanup.Invoke(null, null);
			}
			else if (_logger.IsEnabled(LogLevel.Warning))
			{
				_logger.LogWarning("Application.CleanupNonDefaultAlcCaches was not found; guest ALC memory may stay resident until the next guest exits.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Application.CleanupNonDefaultAlcCaches failed; guest ALC memory may stay resident.");
		}

		try
		{
			if (_getPropertyCacheField?.GetValue(null) is { } propertyCache && _getPropertyCacheClear is { } clear)
			{
				clear.Invoke(propertyCache, null);
			}
			else if (_logger.IsEnabled(LogLevel.Warning))
			{
				_logger.LogWarning("DependencyProperty._getPropertyCache was not reachable; cross-ALC cache entries may pin the guest ALC.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Clearing DependencyProperty._getPropertyCache failed; cross-ALC cache entries may pin the guest ALC.");
		}

		PruneGuestNavigationHandlers();
	}

	// The samples' Shell subscribes to the process-wide SystemNavigationManager.BackRequested
	// and nothing unsubscribes it when a hosted guest is torn down (Uno's per-ALC sweep does
	// not cover this singleton's event fields), so the whole guest visual tree stays rooted —
	// verified via heap dump. Remove any handler whose origin lives in a collectible ALC.
	// Upstream: unoplatform/uno#24074.
	private static readonly string[] _navigationManagerEventFields = ["_backRequested", "InternalBackRequested"];

	private void PruneGuestNavigationHandlers()
	{
		try
		{
			var manager = global::Windows.UI.Core.SystemNavigationManager.GetForCurrentView();
			var anyFieldFound = false;
			foreach (var fieldName in _navigationManagerEventFields)
			{
				var field = typeof(global::Windows.UI.Core.SystemNavigationManager)
					.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
				if (field is null)
				{
					continue;
				}

				anyFieldFound = true;
				if (field.GetValue(manager) is not MulticastDelegate handlers)
				{
					continue;
				}

				var pruned = (Delegate?)handlers;
				foreach (var handler in handlers.GetInvocationList())
				{
					// Closed delegates carry their target's assembly; a static (open) guest
					// handler has a null Target and only reveals its origin through the
					// declaring method's module.
					var originAssembly = handler.Target?.GetType().Assembly ?? handler.Method.Module.Assembly;
					var targetAlc = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(originAssembly);
					if (targetAlc is not null && targetAlc != System.Runtime.Loader.AssemblyLoadContext.Default)
					{
						pruned = Delegate.Remove(pruned, handler);
					}
				}

				if (!ReferenceEquals(pruned, handlers))
				{
					field.SetValue(manager, pruned);
				}
			}

			// Mirror the sibling sweeps: a silent no-op after an Uno rename would let the guest
			// visual tree stay rooted with nothing in the logs to say why.
			if (!anyFieldFound && _logger.IsEnabled(LogLevel.Warning))
			{
				_logger.LogWarning("SystemNavigationManager event fields were not found; guest navigation handlers may keep the guest visual tree rooted.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Pruning guest navigation handlers failed; the guest visual tree may stay rooted.");
		}
	}
}
