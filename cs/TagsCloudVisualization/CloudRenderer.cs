using System.Drawing;

namespace TagsCloudVisualization;

public class CloudRenderer
{
    private readonly int _padding;
    private readonly Brush _brush;
    private readonly Color _backgroundColor;
    private readonly Pen _pen;

    public CloudRenderer(int padding, Pen pen, Brush brush, Color backgroundColor)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(padding);

        _padding = padding;
        _brush = brush;
        _pen = pen;
        _backgroundColor = backgroundColor;
    }

    public Bitmap Render(IReadOnlyCollection<Rectangle> rectangles)
    {
        var bounds = rectangles.Aggregate(Rectangle.Union);

        var bitmap = new Bitmap(
            bounds.Width + _padding * 2,
            bounds.Height + _padding * 2);

        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(_backgroundColor);

        foreach (var rectangle in rectangles)
        {
            var translated = new Rectangle(
                rectangle.X - bounds.X + _padding,
                rectangle.Y - bounds.Y + _padding,
                rectangle.Width,
                rectangle.Height);

            graphics.FillRectangle(_brush, translated);

            graphics.DrawRectangle(
                _pen,
                translated.X,
                translated.Y,
                translated.Width - 1,
                translated.Height - 1);
        }

        return bitmap;
    }
}
