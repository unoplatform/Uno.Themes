using System.ComponentModel;

// MSTest 4.x introduces Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute,
// which collides with the ComponentModel one these display names use (CS0104).
using Description = System.ComponentModel.DescriptionAttribute;

namespace Uno.Themes.Samples.Entities;

public enum SourceSdk
{
	[Description("WinUI/Uno.UI")]
	WinUI,
	[Description("Uno.Material")]
	UnoMaterial,
	[Description("Uno.Cupertino")]
	UnoCupertino,
	[Description("Uno.Simple")]
	UnoSimple,
}
