using System.Text.RegularExpressions;

namespace TagsCloudVisualization;

public class WordsDataSet(string text)
{
    public Dictionary<string, int> CreateFrequencyDict()
    {
        var words = Regex
            .Matches(text, @"[\w\d]+")
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