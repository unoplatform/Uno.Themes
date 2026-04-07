---
uid: Uno.Themes.Simple.GetStarted
---

# Uno Simple

> [!IMPORTANT]
> UnoFeatures: **SimpleTheme** — add `<UnoFeatures>SimpleTheme</UnoFeatures>` to your app's `.csproj` to enable Uno Simple.

Uno Simple is enabled through the `SimpleTheme` UnoFeatures and lets you apply Simple Design System (SDS) styling to your application with a few lines of code.

## Getting Started

> [!NOTE]
> Make sure to setup your environment first by [following our instructions](xref:Uno.GetStarted.vs2022).

### Creating a new project with Uno Simple

1. Install the [`dotnet new` CLI templates](xref:Uno.GetStarted.dotnet-new) with:

    ```bash
    dotnet new install Uno.Templates
    ```

2. Create a new application with:

    ```bash
    dotnet new unoapp -o UnoSimpleApp -theme simpletheme
    ```

### Installing Uno Simple in an existing project

Depending on the type of project template that the Uno Platform application was created with, follow the instructions below to install Uno Simple.

#### [**Single Project Template**](#tab/singleproj)

1. Edit your project file (`PROJECT_NAME.csproj`) and add `SimpleTheme` to the list of `UnoFeatures`:

    ```xml
    <UnoFeatures>SimpleTheme</UnoFeatures>
    ```

2. Initialize the Simple theme resources in the `App.xaml`:

    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>

                <!-- Code omitted for brevity -->

                <us:SimpleTheme xmlns:us="using:Uno.Simple" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

#### [**Multi-Head Project Template (Legacy)**](#tab/multihead)

> [!NOTE]
> Use this only when working on older multi-head templates that do not support `UnoFeatures`. Modern templates should use the Single Project instructions above.

1. In the Solution Explorer panel, right-click on your app's **App Code Library** project (`PROJECT_NAME.csproj`) and select `Manage NuGet Packages...`
1. Install the [`Uno.Simple.WinUI`](https://www.nuget.org/packages/Uno.Simple.WinUI)
1. Add the following Simple resources to `AppResources.xaml`:

    ```xml
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>

            <us:SimpleTheme xmlns:us="using:Uno.Simple" />

        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    ```

---

## Customization

The following guides require the creation of new `ResourceDictionary` files in your application project. For more information on how to define styles and resources in a separate `ResourceDictionary`, refer to the [resource management documentation](xref:Guide.HowTo.Create-Control-Library#moving-the-control-style-in-a-separate-resource-dictionary).

### Customize Color Palette

You can override the default Simple color palette by providing a `ResourceDictionary` with color overrides:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.ColorOverrideDictionary>
        <ResourceDictionary>
            <!-- Add color overrides here -->
        </ResourceDictionary>
    </us:SimpleTheme.ColorOverrideDictionary>
</us:SimpleTheme>
```

### Customize Fonts

You can override the default Simple fonts by providing a `ResourceDictionary` with font overrides:

```xml
<us:SimpleTheme xmlns:us="using:Uno.Simple">
    <us:SimpleTheme.FontOverrideDictionary>
        <ResourceDictionary>
            <!-- Add font overrides here -->
        </ResourceDictionary>
    </us:SimpleTheme.FontOverrideDictionary>
</us:SimpleTheme>
```

### Seed Color Palette

Instead of manually defining every color, you can provide a single **seed color** and let the library generate the full Light and Dark palette algorithmically using the Material Design 3 HCT color system. See the [Seed Color Palette documentation](xref:Uno.Themes.SeedColors#getting-started).
