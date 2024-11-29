namespace TagsCloud;

public class NoEqualityComparer<T> : IComparer<T>
    where T : IComparable<T>
{
    public int Compare(T? x, T? y)
    {
        if (x == null)
        {
            return -1;
        }

        var comparison = x.CompareTo(y);
        return comparison == 0
            ? -1
            : comparison;
    }
}
