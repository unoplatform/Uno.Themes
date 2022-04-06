using System;
using System.Collections.Generic;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material resources including colors, layout values and styles
	/// </summary>
	public sealed class MaterialResourcesV2 : ResourceDictionary
	{
		public MaterialResourcesV2()
		{
			Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml");
		}

		public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

		private string[] GetImplicitStyles()
		{
			return new[] { "M3MaterialBodyMedium" };
		}

		private void ExportImplicitStyles(bool value)
		{
			if (!value) return; // we don't support teardown

			var implicitResources = new ResourceDictionary();
			foreach (var key in GetImplicitStyles())
			{
				if (!this.TryGetValue(key, out var resource) || !(resource is Style style))
				{
					// uwp: If the {key} style is clearly defined in {info.Source}, but we can't find it here.
					// And, that it only happens on uwp, and not other uno platforms.
					// It means that the style references resources that are not directly included.
					// This can usually be fixed by including `<MaterialColors xmlns="using:Uno.Material" />` in the MergedDictionaries of {info.Source}.
					// note: Resources used on Style.Setters need to be directly defined/included, those used in Style.Template dont have to be.
					throw new ArgumentException($"Missing resource: key={key}");
				}
				if (style.TargetType == null)
				{
					throw new InvalidOperationException($"Missing TargetType on style: key={key}");
				}

				implicitResources.Add(style.TargetType, style);
			}

			// UWP don't allow for res-dict with Source set to contain resource directly:
			// > Local values are not allowed in resource dictionary with Source set
			// but, we can add them through merged-dict instead.
			this.MergedDictionaries.Add(implicitResources);
		}
	}
}
