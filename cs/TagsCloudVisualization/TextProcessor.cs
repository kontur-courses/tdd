using System.Drawing;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization;

public class TextProcessor
{
    private readonly AppSettings appSettings;
    public List<WordData> ProcessWords { get; private set; } = new List<WordData>();
    public Dictionary<string, int> wordCounts = new Dictionary<string, int>();
    private int maxCount;
    private int minCount;

    public TextProcessor(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    public void Process()
    {
        GetWords(appSettings.WordsFilePath);
        foreach (var pair  in wordCounts)
        {
            ProcessWords.Add(CreateWordData(pair.Key, pair.Value));
        }
    }

    private void GetWords(string path)
    {
        var text = File.ReadAllText(path);
        const string delimitersPattern = @"\s*,\s*|\s*[\/\n]\s*|\s+";
        var words = Regex.Split(text, delimitersPattern, RegexOptions.IgnoreCase)
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.ToLowerInvariant());

        foreach (var word in words)
        {
            if (!wordCounts.ContainsKey(word))
                wordCounts.Add(word, 0);
            wordCounts[word]++;
        }
        maxCount = wordCounts.Values.Max();
        minCount = wordCounts.Values.Min();
    }
    
    private WordData CreateWordData(string word, int count)
    {
        var spread = maxCount - minCount;
        var normalize = spread == 0 ? 0.5 : (double)(count - minCount) / spread;
        
        var fontSize = (int)Math.Round(appSettings.MinFontSize + 
                                       (appSettings.MaxFontSize - appSettings.MinFontSize)
                                       * normalize);
        
        var wordFont = new Font(appSettings.DefaultFontName, fontSize);
        var size = MeasureWordSize(word, wordFont);
        
        return new WordData(word, wordFont, size);
    }

    private Size MeasureWordSize(string word, Font font)
    {
        using var tempBitmap = new Bitmap(1, 1);
        using var graphics = Graphics.FromImage(tempBitmap);
        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        var sizeF = graphics.MeasureString(word, font);
        
        return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
    }
}