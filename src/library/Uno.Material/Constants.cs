using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Material
{
	internal static class Constants
	{
#if WinUI
		public const string FrameworkLineage = "WinUI";
		public const string PackageName = "Uno.Material.WinUI";
#else
		public const string FrameworkLineage = "UWP";
		public const string PackageName = "Uno.Material";
#endif
	}
}
