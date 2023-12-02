namespace TagsCloudVisualization;

public abstract class WordsDataSet
{
    public static Dictionary<string, int> CreateFrequencyDict(string filePath)
    {
        // TODO fix separator
        var words = File.ReadAllText(filePath)
            .Split(new[] { " ", ",", "\n", ", " }, StringSplitOptions.RemoveEmptyEntries);

        var dict = new Dictionary<string, int>();

        foreach (var word in words)
        {
            if (dict.ContainsKey(word))
                dict[word]++;
            else
                dict.Add(word, 1);
        }

        return dict;
    }
}