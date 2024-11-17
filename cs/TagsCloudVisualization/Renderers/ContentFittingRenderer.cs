using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Renderers;

public class ContentFittingRenderer : IRenderer
{
    private int left;
    private int right;
    private int top;
    private int bottom;
    private readonly List<Rectangle> rectangles = [];
    private readonly Pen pen = new(Color.Black);
    private readonly Random rand = new();

    public void AddRectangle(Rectangle rectangle)
    {
        left = Math.Min(left, rectangle.Left);
        right = Math.Max(right, rectangle.Right);
        top = Math.Min(top, rectangle.Top);
        bottom = Math.Max(bottom, rectangle.Bottom);

        rectangles.Add(rectangle);
    }

    public void SaveImage(string filename)
    {
        var width = Math.Abs(left) + Math.Abs(right) + 1;
        var height = Math.Abs(top) + Math.Abs(bottom) + 1;

        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        FillBackground(graphics, width, height, Color.Black);
        DrawRectangles(graphics);

        bitmap.Save(filename, ImageFormat.Png);
    }

    public void Clear()
    {
        rectangles.Clear();
        left = 0;
        right = 0;
        top = 0;
        bottom = 0;
    }

    public void AddRectangles(Rectangle[] inputRectangles)
    {
        foreach (var rect in inputRectangles)
        {
            AddRectangle(rect);
        }
    }

    private void FillBackground(Graphics graphics, int width, int height, Color color)
    {
        graphics.FillRectangle(new SolidBrush(color), 0, 0, width, height);
    }

    private void DrawRectangles(Graphics graphics)
    {
        foreach (var rectangle in rectangles)
        {
            SetRandomColor();
            var x = GetAdjustedCoordinate(rectangle.X, left);
            var y = GetAdjustedCoordinate(rectangle.Y, top);
            graphics.DrawRectangle(pen, x, y, rectangle.Width, rectangle.Height);
        }
    }
    
    private int GetAdjustedCoordinate(int coordinate, int offset)
    {
        return coordinate + Math.Abs(offset);
    }

    private void SetRandomColor()
    {
        pen.Color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
    }
}