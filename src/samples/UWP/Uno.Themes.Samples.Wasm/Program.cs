using Uno.UI;

namespace Uno.Themes.Samples.Wasm;

public class Program
{
	private static App _app;

	static int Main(string[] args)
	{
		FeatureConfiguration.UIElement.AssignDOMXamlName = true;
		FeatureConfiguration.UIElement.AssignDOMXamlProperties = true;

		Windows.UI.Xaml.Application.Start(_ => _app = new App());

		return 0;
	}
}
