using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;



namespace Uno.Material.Samples.UITest
{ 
	public class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android)
			{
				return ConfigureApp
					.Android
					.InstalledApp("uno.platform.material")
					.EnableLocalScreenshots()
					.Debug()
					.StartApp();



			}



			return ConfigureApp
				.iOS
				.InstalledApp("uno.platform.material")
				.EnableLocalScreenshots()
				.Debug()
				.StartApp();
		}
	}
}
