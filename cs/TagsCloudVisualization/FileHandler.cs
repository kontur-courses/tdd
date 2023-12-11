namespace TagsCloudVisualization;

public static class FileHandler
{
    public static string ReadText(string fileName)
    {
        return File.ReadAllText(GetSourceRelativeFilePath($"{fileName}.txt"));
    }

    public static string GetOutputRelativeFilePath(string fileName)
    {
        return $"../../../../TagsCloudVisualization/out/{fileName}";
    }

    public static string GetSourceRelativeFilePath(string fileName)
    {
        return $"../../../../TagsCloudVisualization/src/{fileName}";
    }
}