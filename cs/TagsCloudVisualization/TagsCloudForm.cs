using CircularCloudLayouter.WeightedLayouter.Forming;
using Timer = System.Windows.Forms.Timer;

namespace TagsCloudVisualization;

public partial class TagsCloudForm : Form
{
    private readonly Color _backColor = Color.FromArgb(0, 35, 45);
    private readonly FormFactor _formFactor = StandardFormFactors.Ellipse;

    private readonly Timer _timer = new();
    private const int DrawingInterval = 5;

    private readonly WordsLoader _wordsLoader = new();
    private WordsDrawingDataHandler _drawingDataHandler = null!;
    private Graphics _graphics = null!;

    private int _wordsToPaintCount = -1;


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        WindowState = FormWindowState.Maximized;
        BackColor = _backColor;

        _graphics = CreateGraphics();
        _graphics.TranslateTransform(ClientSize.Width / 2f, ClientSize.Height / 2f);

        _drawingDataHandler = new WordsDrawingDataHandler(
            new Point(0, 0),
            _formFactor.WithRatio((double) ClientSize.Width / ClientSize.Height)
        );
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        _timer.Interval = DrawingInterval;
        _timer.Tick += OnTimerOnTick;
        _timer.Start();
    }

    private void OnTimerOnTick(object? o, EventArgs eventArgs)
    {
        if (_wordsToPaintCount >= _wordsLoader.Words.Count)
        {
            _timer.Stop();
            Parallel.Invoke(() => WordsImageSaver.SaveResult(_drawingDataHandler));
            return;
        }

        if (_wordsToPaintCount < 0)
            DrawWord(_wordsLoader.MainWord, _graphics, true);
        else
            DrawWord(_wordsLoader.Words[_wordsToPaintCount], _graphics);
        _wordsToPaintCount++;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        DrawWord(_wordsLoader.MainWord, _graphics, true);

        var lastWordIndex = Math.Min(_wordsToPaintCount, _wordsLoader.Words.Count);
        for (var i = 0; i < lastWordIndex; i++)
            DrawWord(_wordsLoader.Words[i], _graphics);
    }

    private void DrawWord(string word, Graphics ghx, bool isMain = false)
    {
        var data = _drawingDataHandler.GetTextDrawingData(word, ghx, isMain);
        ghx.DrawString(data.Text, data.Font, data.Brush, data.Rectangle, data.Format);
    }
}