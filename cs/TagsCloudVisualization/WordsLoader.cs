namespace TagsCloudVisualization;

public class WordsLoader
{
    private const string WordsFilePath = "words.txt";

    private string? _mainWord;

    public string MainWord
    {
        get
        {
            if (_mainWord is null)
                LoadWords();
            return _mainWord!;
        }
    }

    private string[]? _words;

    public IReadOnlyList<string> Words
    {
        get
        {
            if (_words is null)
                LoadWords();
            return _words!;
        }
    }

    public void LoadWords()
    {
        var words = File.ReadAllLines(WordsFilePath);
        _mainWord = words[0].Replace("\\n", "\n");

        var random = new Random();
        _words = words
            .Skip(1)
            .OrderBy(_ => random.Next())
            .ToArray();
    }
}