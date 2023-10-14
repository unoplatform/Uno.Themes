using System.Xml.Linq;

namespace Uno.Markup.Xaml.Helpers;

public static class XamlObjectHelper
{
	public static bool IsCollectionType(XElement e)
	{
		var (xmlns, name) = e.GetNameParts();
		return (xmlns, name) switch
		{
			(_, "VisualStateManager.VisualStateGroups") => true,
			(_, "Grid.RowDefinitions" or "Grid.ColumnDefinitions") => true,
			(_, _) when name.EndsWith(".PrimaryCommands") => true,
			(_, _) when name.EndsWith(".Resources") => true,

			_ => false,
		};
	}
}
