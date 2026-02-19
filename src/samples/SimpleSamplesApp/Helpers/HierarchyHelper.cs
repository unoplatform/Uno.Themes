namespace Uno.Themes.Simple.Samples.Helpers;

public static class HierarchyHelper
{
    public static IEnumerable<T> Flatten<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
    {
        foreach (var item in source)
        {
            yield return item;

            foreach (var child in Flatten(childrenSelector(item), childrenSelector))
            {
                yield return child;
            }
        }
    }
}
