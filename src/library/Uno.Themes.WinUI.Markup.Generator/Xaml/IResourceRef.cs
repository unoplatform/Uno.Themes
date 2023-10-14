namespace Uno.Markup.Xaml;

public interface IResourceRef : IComparable
{
	string ResourceKey { get; }

	int IComparable.CompareTo(object? obj)
	{
		if (obj is IResourceRef rr)
		{
			return ResourceKey.CompareTo(rr.ResourceKey);
		}

		return 1;
	}

	public string GetTypename() => GetType().Name.RemoveTail("Ref");
}

// Static/ThemeResource indicates the resources was stored whether under RD or under RD.ThemeDictionaries
// Static/ThemeResourceRef indicates the type of resource alias(<StaticResource ResourceKey="..." />) or markup({StaticResource ...})

public record StaticResource(ResourceKey Key, object? Value) : IKeyedResource;

public record ThemeResource(ResourceKey Key, object? LightValue, object? DarkValue) : IKeyedResource // naive impl only, it should be a dict<TKey, object>
{
	public bool AreThemeDefinitionEqual()
	{
		return LightValue?.Equals(DarkValue) == true;
	}
}
