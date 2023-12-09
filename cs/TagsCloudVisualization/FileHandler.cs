namespace TagsCloudVisualization;

public static class FileHandler
{
    public static string ReadText(string fileName)
    {
        return File.ReadAllText(GetRelativeFilePath(fileName));
    }

    public static string GetRelativeFilePath(string fileName)
    {
        return $"../../../../TagsCloudVisualization/{fileName}";
    }
}