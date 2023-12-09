using System.Text.RegularExpressions;

namespace TagsCloudVisualization;

public class WordsDataSet
{
    private readonly string wordsFileName;

    public WordsDataSet(string wordsFileName)
    {
        this.wordsFileName = wordsFileName;
    }

    public Dictionary<string, int> CreateFrequencyDict()
    {
        var words = Regex
            .Matches(FileHandler.ReadText($"src/{wordsFileName}.txt"), @"[\w\d]+")
            .Select(m => m.Value)
            .ToArray();

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