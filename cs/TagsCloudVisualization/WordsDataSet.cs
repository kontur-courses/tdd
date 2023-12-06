using System.Text.RegularExpressions;

namespace TagsCloudVisualization;

public class WordsDataSet
{
    public Dictionary<string, int> CreateFrequencyDict(string fileName)
    {
        var words = Regex
            .Matches(File.ReadAllText(
                    $"../../../../TagsCloudVisualization/src/{fileName}.txt"), @"[\w\d]+"
            )
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