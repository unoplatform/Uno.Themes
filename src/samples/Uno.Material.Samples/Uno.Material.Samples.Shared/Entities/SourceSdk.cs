using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Uno.Material.Samples.Entities
{
	public enum SourceSdk
	{
		[Description("WinUI/Uno.UI")]
		WinUI,
		[Description("Uno.Material")]
		UnoMaterial,
		[Description("Uno.Cupertino")]
		UnoCupertino,
	}
}
