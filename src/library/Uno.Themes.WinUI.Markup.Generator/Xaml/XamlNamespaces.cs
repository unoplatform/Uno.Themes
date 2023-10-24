using System.Xml.Linq;

namespace Uno.Markup.Xaml;

internal class XamlNamespaces
{
	public static readonly XNamespace Presentation = XNamespace.Get(NSPresentation);
	public static readonly XNamespace X = XNamespace.Get(NSX);

	public const string NSXmlns = "http://www.w3.org/2000/xmlns/";
	public const string NSX = "http://schemas.microsoft.com/winfx/2006/xaml";
	public const string NSPresentation = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
}
