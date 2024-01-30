using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Uno.Cupertino")]
[assembly: InternalsVisibleTo("Uno.Material")]
namespace Uno.Themes;

internal static class Constants
{
	public const string ConverterResourcePath = "ms-appx:///Uno.Themes/Styles/Applications/Common/Converters.xaml";
	public const string SharedColorsResourcePath = "ms-appx:///Uno.Themes/Styles/Applications/Common/SharedColors.xaml";
	public const string SharedColorPaletteResourcePath = "ms-appx:///Uno.Themes/Styles/Applications/Common/SharedColorPalette.xaml";
}
