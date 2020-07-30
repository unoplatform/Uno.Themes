using System;
using Windows.UI.Xaml;

namespace Uno.Material
{
	public static class Resources
	{
		/// <summary>
		/// Mandatory Init of the Material resources in Application's OnLaunched method.
		/// </summary>
		/// <param name="app">application instance. Typically "this" since you're in OnLaunched.</param>
		/// <param name="colorPaletteOverride">ResourceDictionary containing themeresources for you color palette. Those will override the default colors</param>
		/// <remarks>See https://github.com/unoplatform/Uno.Material for the full documentation</remarks>
		public static void Init(Application app, ResourceDictionary colorPaletteOverride)
		{
			// NOTE: The order below is very important!

			// Set a default palette to make sure all colors exist and avoid possible crashes.
			// We cannot guarantee the consuming application will override 100% of the resources.
			app.Resources.MergedDictionaries.Add(new MaterialColorPalette());

			if(colorPaletteOverride != null)
			{
				// Overlap the default colors with the application's colors palette. 
				app.Resources.MergedDictionaries.Add(colorPaletteOverride);
			}			

			// Lastly, add all the material resources. Those resources depend on the colors above, which is why this one must be added last.
			app.Resources.MergedDictionaries.Add(new MaterialResources());
		}
	}
}
