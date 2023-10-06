using System;

namespace Uno.Themes.WinUI.Markup;

// temporary workaround for ResourceKeyDefinition not applicable on field members.
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class ResourceKeyDefinitionAttribute : Attribute
{
	public ResourceKeyDefinitionAttribute(Type propertyType, string key)
	{
		this.PropertyType = propertyType;
		this.Key = key;
	}

	public string Key { get; }
	public Type PropertyType { get; }
	public Type? TargetType { get; set; }
}
