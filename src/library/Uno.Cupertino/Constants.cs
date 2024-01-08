using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Cupertino
{
	internal static class Constants
	{
#if WinUI
		public const string FrameworkLineage = "WinUI";
		public const string PackageName = "Uno.Cupertino.WinUI";
#else
		public const string FrameworkLineage = "UWP";
		public const string PackageName = "Uno.Cupertino";
#endif
	}
}
