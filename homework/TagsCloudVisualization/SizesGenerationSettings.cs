namespace TagsCloudVisualization;

public record struct SizesGenerationSettings(int Count, int MinWidth, int MaxWidth, int MinHeight, int MaxHeight)
{
    public static SizesGenerationSettings Parse(string str)
    {
        var args = str
            .Split(default(char[]), StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        return new SizesGenerationSettings(args[0], args[1], args[2], args[3], args[4]);
    }
}
