using System.Xml.Linq;
using Uno.Markup.Xaml.Helpers;
using Uno.Markup.Xaml.Parsers;
using Uno.Markup.Xaml.UI.Xaml;
using Uno.Markup.Xaml.UI.Xaml.Controls;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml;

public record VisualTreeElement(string Type)
{
	private static readonly string[] WhiteListedTypes = @"
		// winui
		Border, Grid, StackPanel, StackLayout, PivotPanel, WrapPanel, GridView,
		ScrollViewer, ScrollContentPresenter, ScrollBar
		ItemsControl, ItemsPresenter, ItemsRepeater, ItemsRepeaterScrollHost, ListView, Pivot, ListViewItemPresenter,
		ItemsStackPanel, PivotHeaderPanel, CalendarPanel,
		Control, ContentControl, ContentPresenter, ScrollViewer, Button, RepeatButton, TextBox, TextBlock, Slider, RadioButton, ToggleButton, ToggleSwitch, ProgressBar, NumberBox
		CheckBox, ComboBox, ComboBoxItem,
		CalendarView, MediaPlayerPresenter,
		NavigationView, NavigationViewList, NavigationViewItemPresenter,
		AppBar, AppBarButton, CommandBar, CommandBarOverflowPresenter,
		Path, Rectangle, Ellipse, Polyline, Canvas, Image,
		PathIcon, FontIcon, BitmapIcon, SymbolIcon
		TickBar, Thumb,
		ColorSpectrum, ColorPickerSlider,
		MonochromaticOverlayPresenter,
		AnimatedIcon,
		AnimatedVisual,AnimatedAcceptVisualSource,AnimatedChevronDownSmallVisualSource,AnimatedChevronUpDownSmallVisualSource,AnimatedBackVisualSource,AnimatedGlobalNavigationButtonVisualSource
		InfoBarPanel
		TabViewListView, TreeViewList
		CarouselPanel,
		DataTemplate, ItemsPanelTemplate

		// winui\DependencyObjects... just lumping them here for now
		RowDefinition, ColumnDefinition

		// winui\etc containers/wrappers
		Flyout, Popup,
		SplitView, NavigationView, NavigationViewItem, NavigationViewItemSeparator,
		Viewbox, ElevatedView,

		// uno
		NativeCommandBarPresenter

		// themes, toolkit
		Ripple,
		Chip, Drawer, ShadowContainer,
		NativeNavigationBarPresenter, NavigationBarPresenter,
		TabBar, TabBarSelectionIndicatorPresenter, TabBarListPanel,
	".Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Where(x => !x.StartsWith("//"))
			.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			.ToArray();

	public string? Name { get; private set; }
	public Dictionary<string, object> Properties { get; private set; } = new();
	public Dictionary<string, object> AttachedProperties { get; private set; } = new();
	public List<VisualTreeElement> Children { get; private set; } = new();

	public static bool TryParse(XElement e, out object? result) // generic parser for all visual tree elements or DependencyObjects
	{
		if (WhiteListedTypes.Contains(e.Name.LocalName))
		{
			result = Parse(e);
			return true;
		}

		result = default;
		return false;
	}
	public static VisualTreeElement Parse(XElement e)
	{
		var result = new VisualTreeElement(e.Name.LocalName);
		foreach (var attribute in e.Attributes())
		{
			ParseAttributeMember(attribute, result);
		}
		foreach (var child in e.Elements())
		{
			ParseChildMember(child, result);
		}

		return result;
	}
	public static void ParseAttributeMember(XAttribute attribute, VisualTreeElement target)
	{
		if (ScuffedXamlParser.IsExplicitlyIgnored(attribute)) return;

		var (xmlns, name) = attribute.GetNameParts();
		if (xmlns == string.Empty)
		{
			xmlns = attribute.Parent?.GetDefaultNamespace().NamespaceName;
		}

		switch ((xmlns, name))
		{
			case (NSXmlns, _): // inline xmlns definition
				return;

			case (NSX, "Name"): target.Name = attribute.Value; return;

			case (NSPresentation, _):
			case (NSX, _):
			case ("using:Microsoft.UI.Xaml.Controls", _):
			case ("using:Windows.UI.Xaml.Controls.Primitives", _):
			case ("using:Microsoft.UI.Xaml.Controls.Primitives", _):
				target.Properties[name] = ScuffedXamlParser.ParseInlineValue(attribute.Value); return;

			case (_, _) when name.Contains('.'):
				target.AttachedProperties[name] = ScuffedXamlParser.ParseInlineValue(attribute.Value); return;

			default: //target.Properties.Add(name, attribute.Value); return;
				throw new NotImplementedException().PreDump(attribute, (xmlns, name).ToString());
		}
	}
	public static void ParseChildMember(XElement child, VisualTreeElement target)
	{
		//if (ScuffedXamlParser.IsExplicitlyIgnored(child)) return;

		var (xmlns, name) = child.GetNameParts();
		if (xmlns == string.Empty)
		{
			xmlns = child.GetDefaultNamespace().NamespaceName;
		}

		/* we have 3 cases here: // for child element only (not applicable to attribute)
			1. member property whose local name is $"{Parent.ClassName or its upper-class name}.{PropertyName}"
			2. attached property whose local name is $"{DeclaringType}.{PropertyName}"
			3. any remaining should be direct content
		*/

		var parts = name.Split('.', 2);
		var className = parts[0];
		var memberName = parts.ElementAtOrDefault(1);
		if (memberName != null && target.IsMatchingClass(className)) // case 1
		{
			if (ParseValue() is { } value /*&& value is not IScriptIgnorable*/)
			{
				target.Properties[memberName] = ParseValue();
			}
		}
		else if (memberName != null) // case 2
		{
			if (ParseValue() is { } value /*&& value is not IScriptIgnorable*/)
			{
				target.AttachedProperties[name] = ParseValue();
			}
		}
#if PARSE_VISUALELEMENT_CHILD
		else // case 3
		{
			var parsed = ScuffedXamlParser.Parse(child);
			if (parsed is Ignorable) return;
			if (parsed is VisualTreeElement vte)
			{
				target.Children.Add(vte);
			}
			else
			{
				throw new NotImplementedException().PreDump(child, (xmlns, name).ToString());
			}
		}
#endif

		object ParseValue()
		{
			var elements = child.Elements().ToArray();
			if (XamlObjectHelper.IsCollectionType(child))
			{
				return elements.Select(ScuffedXamlParser.Parse)/*.Where(x => x is not IScriptIgnorable)*/.ToArray();
			}
			else
			{
				if (elements.Length == 0) return null;
				else if (elements.Length == 1) return ScuffedXamlParser.Parse(elements[0]);
				throw new Exception($"{child.Name} is not a collection type, but nests more than one element.");
			}
		}
	}

	public IEnumerable<VisualStateGroup>? GetVisualStateGroups()
	{
		if (AttachedProperties.TryGetValue("VisualStateManager.VisualStateGroups", out var boxedVsgs) &&
			boxedVsgs.As<object[]>()?.Cast<VisualStateGroup>() is { } vsgs)
		{
			return vsgs;
		}
		else
		{
			return null;
		}
	}
	public IEnumerable<(string Path, VisualTreeElement Element)> Flatten()
	{
		return YieldNodeWalk(this, "");

		IEnumerable<(string Path, VisualTreeElement Element)> YieldNodeWalk(VisualTreeElement element, string path)
		{
			path += '/' + Describe(element);

			yield return (path, element);
			foreach (var property in element.Properties)
			{
				// control-template
				if (property.Key == "Template" &&
					property.Value is ControlTemplate { TemplateRoot: { } templateRoot })
				{
					foreach (var item in YieldNodeWalk(templateRoot, path + "/.Template"))
					{
						yield return (item.Path, item.Element);
					}
				}

				// todo: item-template
			}
			foreach (var child in element.Children)
			{
				// nested children
				foreach (var item in YieldNodeWalk(child, path))
				{
					yield return (item.Path, item.Element);
				}
			}
		}
		static string Describe(object node)
		{
			return node switch
			{
				VisualTreeElement vte => string.IsNullOrEmpty(vte.Name) ? vte.Type : $"#{vte.Name}",

				_ => throw new NotImplementedException().PreDump(node),
			};
		}
	}
	private bool IsMatchingClass(string className)
	{
		// todo: check through class hierarchy too
		return Type == className;
	}
}
