using System.Drawing.Imaging;
using CircularCloudLayouter;
using CircularCloudLayouter.WeightedLayouter;
using CircularCloudLayouter.WeightedLayouter.Forming;
using Timer = System.Windows.Forms.Timer;

namespace TagsCloudVisualization;

public partial class TagsCloudForm : Form
{
    private readonly FormFactor _formFactor = StandardFormFactors.Circle;
    private ICircularCloudLayouter _circularCloudLayouter = null!;
    
    private readonly Random _random = new();
    private readonly Timer _timer = new();

    private const int DrawingInterval = 10;
    private const string WordsFilePath = "words.txt";
    private const string ResultsFolderPath = "results";

    private readonly Color _backColor = Color.FromArgb(0, 35, 45);

    private readonly Color _mainWordColor = Color.White;
    private const int MainWordMinFontSize = 25;
    private const int MainWordMaxFontSize = 32;
    private string _mainWord = null!;

    private readonly Color _wordsColor = Color.FromArgb(250, 100, 0);
    private const int WordsMinFontSize = 6;
    private const int WordsMaxFontSize = 15;
    private string[] _words = null!;

    private readonly Dictionary<string, TextDrawingData> _textDrawingDatas = new();
    private int _wordsToPaintCount = -1;
    
    private Graphics _graphics = null!;


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        WindowState = FormWindowState.Maximized;
        BackColor = _backColor;

        _circularCloudLayouter = new WeightedCircularCloudLayouter(
            new Point(ClientSize.Width / 2, ClientSize.Height / 2),
            _formFactor.WithRatio((double) ClientSize.Width / ClientSize.Height)
        );

        _graphics = CreateGraphics();

        LoadWords();
        InitializeTimer();
    }

    private void LoadWords()
    {
        var fileWords = File.ReadAllLines(WordsFilePath);

        _mainWord = fileWords[0];
        _words = fileWords
            .Skip(1)
            .OrderBy(_ => _random.Next())
            .ToArray();
    }

    private void InitializeTimer()
    {
        _timer.Interval = DrawingInterval;
        _timer.Tick += (_, _) =>
        {
            if (_wordsToPaintCount++ < 0)
            {
                DrawMainWord(_graphics);
            }
            else if (_wordsToPaintCount < _words.Length)
            {
                DrawWord(_graphics, _words[_wordsToPaintCount]);
            }
            else
            {
                _timer.Stop();
                Parallel.Invoke(SaveResult);
            }
        };
        _timer.Start();
    }

    private void SaveResult()
    {
        var bitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
        var ghx = Graphics.FromImage(bitmap);
        foreach (var (word, data) in _textDrawingDatas)
            ghx.DrawString(word, data.Font, data.Brush, data.Rectangle);
        
        if (!Directory.Exists(ResultsFolderPath))
            Directory.CreateDirectory(ResultsFolderPath);
        var resultName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".png";
        var resultPath = Path.Combine(ResultsFolderPath, resultName);
        bitmap.Save(resultPath, ImageFormat.Png);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var ghx = e.Graphics;
        DrawMainWord(ghx);
        var lastWordIndex = Math.Min(_wordsToPaintCount, _words.Length);
        for (var i = 0; i < lastWordIndex; i++)
            DrawWord(ghx, _words[i]);
    }

    private void DrawMainWord(Graphics ghx) =>
        DrawWord(ghx, _mainWord, MainWordMinFontSize, MainWordMaxFontSize, _mainWordColor);

    private void DrawWord(Graphics ghx, string word) =>
        DrawWord(ghx, word, WordsMinFontSize, WordsMaxFontSize, _wordsColor);

    private void DrawWord(Graphics ghx, string word, int minFont, int maxFont, Color color)
    {
        if (!_textDrawingDatas.ContainsKey(word))
            CreateTextData(ghx, word, minFont, maxFont, color);
        var data = _textDrawingDatas[word];
        ghx.DrawString(word, data.Font, data.Brush, data.Rectangle);
    }

    private void CreateTextData(
        Graphics graphics,
        string word,
        int minFont, int maxFont,
        Color color
    )
    {
        var font = new Font(FontFamily.GenericSansSerif, _random.Next(minFont, maxFont + 1));
        var brush = new SolidBrush(color);
        var rect = GetTextRectangle(word, font, graphics);
        _textDrawingDatas[word] = new TextDrawingData(font, brush, rect);
    }

    private Rectangle GetTextRectangle(string word, Font font, Graphics ghx)
    {
        var size = Size.Ceiling(ghx.MeasureString(word, font));
        return _circularCloudLayouter.PutNextRectangle(size);
    }

    private record TextDrawingData(Font Font, Brush Brush, Rectangle Rectangle);
}