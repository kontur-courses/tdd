namespace TagsCloudVisualization.Extensions;

public static class EnumeratorExtension
{
    public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator) {
        while ( enumerator.MoveNext() ) {
            yield return enumerator.Current;
        }
    }
}