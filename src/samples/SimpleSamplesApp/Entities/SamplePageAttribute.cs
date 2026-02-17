namespace Uno.Themes.Simple.Samples.Entities;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SamplePageAttribute : Attribute
{
    public SampleCategory Category { get; }

    public string Title { get; }

    public Symbol Symbol { get; set; }

    public int SortOrder { get; set; }

    public SamplePageAttribute(SampleCategory category, string title)
    {
        Category = category;
        Title = title;
    }
}
