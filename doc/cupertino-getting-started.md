<p align="center">
  <img src="assets/cupertino-design-system.png">
</p>

# Uno.Cupertino
Uno Cupertino is an add-on package that lets you apply [Cupertino - Human Interface Guideline styling](https://developer.apple.com/design/human-interface-guidelines) to your application with a few lines of code.

## Getting Started
1. Open an existing Uno project, or create a new Uno project using the `Uno Platform App` template.
2. In the Solution Explorer panel, right-click on your solution name and select `Manage NuGet Packages for Solution ...`. Choose [`Uno.Cupertino`](https://www.nuget.org/packages/Uno.Cupertino/) package for Uno UWP solution or choose the [`Uno.Cupertino.WinUI`](https://www.nuget.org/packages/Uno.Cupertino.WinUI) package for Uno WinUI solution. Then, select the following projects to include it:
	- `PROJECT_NAME.Wasm.csproj`
	- `PROJECT_NAME.Mobile.csproj` (or `PROJECT_NAME.iOS.csproj`, `PROJECT_NAME.Droid.csproj`, `PROJECT_NAME.macOS.csproj` if you have an existing project)
	- `PROJECT_NAME.Skia.Gtk.csproj`
	- `PROJECT_NAME.Skia.WPF.csproj`
	- `PROJECT_NAME.Windows.csproj` (or `PROJECT_NAME.UWP.csproj` for existing projects)
1. Add the following resources inside `App.xaml`:
    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Load WinUI resources -->
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

                <CupertinoColors xmlns="using:Uno.Cupertino"  />
				<CupertinoResources xmlns="using:Uno.Cupertino" />

                <!-- Load custom application resources -->
				<!-- ... -->

            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

For complete instructions on using Uno Cupertino in your projects, check out this walkthrough: [How to use Uno.Cupertino](https://platform.uno/docs/articles/guides/uno-cupertino-walkthrough.html).

## Customization
### Customize Color Palette
The colors used in the Cupertino styles are part of the color palette system, which can be customized to suit the theme of your application. Since this is decoupled from the styles, the application theme can be changed, without having to make a copy of every style and edit each of them.