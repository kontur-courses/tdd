using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CloudImageGenerator
{
    private const double IndentCoefficient = 1.1;
    private const int MaxColorValue = 240;

    public static Bitmap Generate(ICloudLayouter cloudLayouter)
    {
        var rectangles = cloudLayouter.Rectangles();
        var size = GetImageSize(cloudLayouter.Rectangles(), cloudLayouter.Center());

        var bitmap = new Bitmap(size.Width, size.Height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);

        DrawRectangles(graphics, rectangles, size);
        DrawAxes(graphics, size);
        return bitmap;
    }

    private static void DrawRectangles(Graphics graphics, IEnumerable<Rectangle> rectangles, Size imageSize)
    {
        foreach (var rectangle in rectangles)
        {
            var offsetRectangle = rectangle;
            offsetRectangle.Location = CalculateImageLocation(offsetRectangle, imageSize);
            graphics.FillRectangle(new SolidBrush(GetRandomColor()), offsetRectangle);
        }
    }

    private static Point CalculateImageLocation(Rectangle rectangle, Size imageSize)
    {
        var x = imageSize.Width / 2 + rectangle.Location.X;
        var y = imageSize.Height / 2 - rectangle.Location.Y - rectangle.Size.Height;
        return new Point(x, y);
    }

    private static Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromArgb(
            random.Next(MaxColorValue),
            random.Next(MaxColorValue),
            random.Next(MaxColorValue));
    }

    private static void DrawAxes(Graphics graphics, Size imageSize)
    {
        var top = new Point(imageSize.Width / 2, 0);
        var bottom = new Point(imageSize.Width / 2, imageSize.Height);
        var left = new Point(0, imageSize.Height / 2);
        var right = new Point(imageSize.Width, imageSize.Height / 2);

        graphics.DrawLine(Pens.Black, top, bottom);
        graphics.DrawLine(Pens.Black, left, right);
    }

    private static Size GetImageSize(IReadOnlyCollection<Rectangle> rectangles, Point center)
    {
        var minX = rectangles.Min(r => r.Left);
        var maxX = rectangles.Max(r => r.Right);
        var minY = rectangles.Min(r => r.Top);
        var maxY = rectangles.Max(r => r.Bottom);

        var width = (int)(IndentCoefficient * (maxX - minX) + 2 * Math.Abs(center.X));
        var height = (int)(IndentCoefficient * (maxY - minY) + 2 * Math.Abs(center.Y));
        return new Size(width, height);
    }
}