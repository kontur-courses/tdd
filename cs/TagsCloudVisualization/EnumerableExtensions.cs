namespace TagsCloudVisualization;

public static class EnumerableExtensions
{
    public static IEnumerable<T> ReverseWithoutAllocation<T>(this IReadOnlyList<T> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            yield return list[i];
        }
    }
}