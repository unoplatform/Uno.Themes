namespace Uno.Themes.Simple.Samples.Entities;

public class Sample
{
    public string Title { get; }

    public SampleCategory Category { get; }

    public Type ViewType { get; }

    public Symbol Symbol { get; }

    public int SortOrder { get; }

    public Sample(SamplePageAttribute attribute, Type viewType)
    {
        Title = attribute.Title;
        Category = attribute.Category;
        ViewType = viewType;
        Symbol = attribute.Symbol;
        SortOrder = attribute.SortOrder;
    }
}
