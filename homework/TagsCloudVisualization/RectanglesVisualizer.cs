using System.Drawing;

namespace TagsCloudVisualization;

public class RectanglesVisualizer
{
    private readonly Random _random;
    private readonly List<Rectangle> _rectangles;

    public RectanglesVisualizer()
    {
        _random = new Random();
        _rectangles = [];
    }

    public void AddRectangles(IEnumerable<Rectangle> rectangles)
    {
        foreach (var rectangle in rectangles)
        {
            AddRectangle(rectangle);
        }
    }

    public void AddRectangle(Rectangle rectangle)
    {
        _rectangles.Add(rectangle);
    }

    public Bitmap GetImage()
    {
        if (_rectangles.Count == 0)
        {
            return new Bitmap(1, 1);
        }

        var minX = _rectangles.Min(rectangle => rectangle.X);
        var maxX = _rectangles.Max(rectangle => rectangle.Right);
        var minY = _rectangles.Min(rectangle => rectangle.Y);
        var maxY = _rectangles.Max(rectangle => rectangle.Bottom);

        var width = 2 * Math.Max(Math.Abs(minX), maxX);
        var height = 2 * Math.Max(Math.Abs(minY), maxY);
        var image = new Bitmap(width, height);

        DrawRectangles(image);

        return image;
    }

    private void DrawRectangles(Image image)
    {
        var penWidth = 1;
        var imageCenter = new Point(image.Width / 2, image.Height / 2);

        using var graphics = Graphics.FromImage(image);
        var pen = new Pen(Color.White, penWidth);

        foreach (var rectangle in _rectangles)
        {
            var rectangleToDraw = GetRectangleForDrawing(rectangle, imageCenter, penWidth);
            pen.Color = GetRandomColor();
            graphics.DrawRectangle(pen, rectangleToDraw);
        }
    }

    private Color GetRandomColor()
    {
        return Color.FromArgb(_random.Next(64, 256), _random.Next(64, 256), _random.Next(64, 256));
    }

    private Rectangle GetRectangleForDrawing(Rectangle rectangle, Point newCenter, int penWidth)
    {
        var position = new Point(rectangle.X + newCenter.X, rectangle.Y + newCenter.Y);
        var size = rectangle.Size - new Size(penWidth, penWidth);
        return new Rectangle(position, size);
    }
}
