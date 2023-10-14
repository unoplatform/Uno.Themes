using System.Xml.Linq;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Extensions;

public static class XNameExtensions
{
	public static (string Xmlns, string LocalName) GetNameParts(this XElement e, bool trimApiContract = true) => e.Name.GetNameParts(trimApiContract);
	public static (string Xmlns, string LocalName) GetNameParts(this XAttribute e, bool trimApiContract = true) => e.Name.GetNameParts(trimApiContract);
	public static (string Xmlns, string LocalName) GetNameParts(this XName name, bool trimApiContract = true)
	{
		var xmlns = name.NamespaceName;
		if (trimApiContract && xmlns.IndexOf('?') is var index && index > 0)
		{
			xmlns = xmlns[..(index)];
		}
#if REPLACE_UNO_PLATFORM_XMLNS
		if (xmlns.StartsWith("http://uno.ui/"))
		{
			xmlns = NSPresentation;
		}
#endif

		return (xmlns, name.LocalName);
	}
	public static string Pretty(this XName name)
	{
		var (xmlns, localName) = name.GetNameParts();

		return $"{{{xmlns}}}{localName}";
	}

	public static bool Match(this XName x, string xmlns, string localName) => x.GetNameParts() == (xmlns, localName);

	public static bool Is<T>(this XName x) => x.LocalName == typeof(T).Name;
	public static bool Is(this XName x, string name) => x.LocalName == name;
	public static bool IsAnyOf(this XName x, params string[] names) => names.Contains(x.LocalName);

	public static bool IsMemberOf<T>(this XName x) => x.IsMemberOf(typeof(T));
	public static bool IsMemberOf(this XName x, Type type) => x.IsMemberOf(type.Name);
	public static bool IsMemberOf(this XName x, string typeName) => x.IsMemberOf(typeName, out var _);
	public static bool IsMemberOf<T>(this XName x, string memberName) => x.IsMemberOf(typeof(T), memberName);
	public static bool IsMemberOf(this XName x, Type type, string memberName) => x.IsMemberOf(type.Name, memberName);
	public static bool IsMemberOf(this XName x, string typeName, string memberName) => x.LocalName == $"{typeName}.{memberName}";
	public static bool IsMemberOf<T>(this XName x, out string? memberName) => x.IsMemberOf(typeof(T), out memberName);
	public static bool IsMemberOf(this XName x, Type type, out string? memberName) => x.IsMemberOf(type.Name, out memberName);
	public static bool IsMemberOf(this XName x, string typeName, out string? memberName)
	{
		memberName = x.LocalName.Split(".") is { Length: 2 } parts && parts[0] == typeName
			? parts[1]
			: default;

		return memberName != default;
	}

	public static XElement? GetMember(this XElement e, string memberName) => e.Element(e.Name.Namespace + $"{e.Name.LocalName}.{memberName}");
}
