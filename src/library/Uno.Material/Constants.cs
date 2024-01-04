using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Material
{
	internal static class Constants
	{
#if WinUI
		public const string FrameworkLineage = "WinUI";
#else
		public const string FrameworkLineage = "UWP";
#endif
	}
}
