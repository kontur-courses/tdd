using CircularCloudLayouter;
using CircularCloudLayouter.WeightedLayouter;
using CircularCloudLayouter.WeightedLayouter.Forming;

namespace TagsCloudVisualization;

public class WordsDrawingDataHandler
{
    private static readonly FontFamily FontFamily = FontFamily.GenericSansSerif;
    private static readonly StringFormat StringFormat;

    private const int MainWordMinFontSize = 26;
    private const int MainWordMaxFontSize = 31;
    private static readonly Color MainWordColor = Color.White;

    private const int WordsMinFontSize = 7;
    private const int WordsMaxFontSize = 18;
    private static readonly Color WordsColor = Color.FromArgb(250, 100, 0);

    private readonly Random _random = new();
    private readonly Dictionary<string, TextDrawingData> _textDrawingDatas = new();
    private readonly ICircularCloudLayouter _circularCloudLayouter;

    static WordsDrawingDataHandler()
    {
        StringFormat = new StringFormat();
        StringFormat.Alignment = StringAlignment.Center;
        StringFormat.LineAlignment = StringAlignment.Center;
    }

    public WordsDrawingDataHandler(Point center, FormFactor formFactor)
    {
        Center = center;
        _circularCloudLayouter = new WeightedCircularCloudLayouter(Center, formFactor);
    }
    
    public Point Center { get; }

    public IReadOnlyCollection<TextDrawingData> SavedDrawingDatas =>
        _textDrawingDatas.Values;

    public TextDrawingData GetTextDrawingData(string text, Graphics graphics, bool isMain = false)
    {
        if (!_textDrawingDatas.ContainsKey(text))
            CreateData(
                graphics,
                text,
                isMain ? MainWordMinFontSize : WordsMinFontSize,
                isMain ? MainWordMaxFontSize : WordsMaxFontSize,
                isMain ? MainWordColor : WordsColor
            );
        return _textDrawingDatas[text];
    }

    private void CreateData(
        Graphics graphics,
        string text,
        int minFont, int maxFont,
        Color color
    )
    {
        var font = new Font(FontFamily, _random.Next(minFont, maxFont + 1));
        var brush = new SolidBrush(color);
        var rect = GetTextRectangle(text, font, graphics);
        _textDrawingDatas[text] = new TextDrawingData(text, font, brush, rect, StringFormat);
    }

    private Rectangle GetTextRectangle(string word, Font font, Graphics ghx)
    {
        var size = Size.Ceiling(ghx.MeasureString(word, font));
        return _circularCloudLayouter.PutNextRectangle(size);
    }

    public record TextDrawingData(string Text, Font Font, Brush Brush, Rectangle Rectangle, StringFormat Format);
}