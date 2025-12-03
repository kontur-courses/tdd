using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Drawers;

public class TagCloudDrawer
{
    private readonly IReadOnlyCollection<Rectangle> _rectangles;
    private readonly Random _random;
    private readonly int _padding;

    public TagCloudDrawer(IReadOnlyCollection<Rectangle> rectangles, int padding = 64)
    {
        _rectangles = rectangles ?? throw new ArgumentNullException(nameof(rectangles));
        _padding = padding;
        _random = new Random();
    }

    public void DrawRectanglesToFile(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);

        using var bitmap = CreateBitmap();
        using var graphics = Graphics.FromImage(bitmap);
        
        DrawRectangles(graphics);
        bitmap.Save(path, ImageFormat.Png);
    }

    private Bitmap CreateBitmap()
    {
        if (_rectangles.Count == 0)
            return new Bitmap(100, 100);

        var bounds = CalculateBounds();
        var width = bounds.Width + _padding * 2;
        var height = bounds.Height + _padding * 2;
        
        return new Bitmap(width, height);
    }

    private void DrawRectangles(Graphics graphics)
    {
        graphics.Clear(Color.White);
        
        if (_rectangles.Count == 0)
            return;

        var bounds = CalculateBounds();
        
        using var brush = new SolidBrush(Color.Black);
        using var pen = new Pen(Color.Black, 1);

        foreach (var rectangle in _rectangles)
        {
            var adjusted = AdjustRectangle(rectangle, bounds);
            
            brush.Color = GenerateRandomColor();
            graphics.FillRectangle(brush, adjusted);
            graphics.DrawRectangle(pen, adjusted);
        }
    }

    private CloudBounds CalculateBounds()
    {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        foreach (var rect in _rectangles)
        {
            minX = Math.Min(minX, rect.Left);
            minY = Math.Min(minY, rect.Top);
            maxX = Math.Max(maxX, rect.Right);
            maxY = Math.Max(maxY, rect.Bottom);
        }

        return new CloudBounds(minX, minY, maxX, maxY);
    }

    private Rectangle AdjustRectangle(Rectangle rect, CloudBounds bounds)
    {
        return rect with
        {
            X = rect.Left - bounds.MinX + _padding,
            Y = rect.Top - bounds.MinY + _padding
        };
    }

    private Color GenerateRandomColor()
    {
        return Color.FromArgb(
            _random.Next(256),
            _random.Next(256),
            _random.Next(256)
        );
    }
}