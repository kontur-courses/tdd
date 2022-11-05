using CircularCloudLayouter;
using Timer = System.Windows.Forms.Timer;

namespace TagsCloudVisualization;

public partial class TagsCloudForm : Form
{
    private readonly Random _random = new();
    private Timer? _timer;

    private readonly ICircularCloudLayouter _circularCloudLayouter =
        new WeightedCircularCloudLayouter(new Point(900, 500));

    private readonly List<(Rectangle rect, Brush brush)> _rectanglesBrushes = new();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ClientSize = new Size(1800, 1000);
        BackColor = Color.Black;
        
        var ghx = CreateGraphics();
        _timer = new Timer {Interval = 1};
        _timer.Tick += (_, _) =>
        {
            _rectanglesBrushes.Add((GetNextRectangle(), GenerateRandomBrush()));
            var (rectangle, brush) = _rectanglesBrushes.Last();
            ghx.FillRectangle(brush, rectangle);
        };
        _timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var ghx = e.Graphics;
        foreach (var (rect, brush) in _rectanglesBrushes)
            ghx.FillRectangle(brush, rect);
    }

    private Rectangle GetNextRectangle()
    {
        var size = new Size(_random.Next(10, 50), _random.Next(10, 50));
        return _circularCloudLayouter.PutNextRectangle(size);
    }

    private Brush GenerateRandomBrush()
    {
        return new SolidBrush(
            Color.FromArgb(_random.Next(50, 255), _random.Next(50, 255), _random.Next(50, 255)));
    }
}