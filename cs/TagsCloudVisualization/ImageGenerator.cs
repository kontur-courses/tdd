using System.Drawing;

namespace TagsCloudVisualization;

public class ImageGenerator
{
    private static readonly Color[] Colors =
    {
        Color.Red,
        Color.Orange,
        Color.Yellow,
        Color.Green,
        Color.LightBlue,
        Color.Blue,
        Color.Purple
    };

    private readonly int widthOffset;
    private readonly int heightOffset;

    public ImageGenerator(int widthOffset = 200, int heightOffset = 200)
    {
        this.widthOffset = widthOffset;
        this.heightOffset = heightOffset;
    }

    public Bitmap Generate(ICollection<Rectangle> rectangles)
    {
        var canvasParameters = CalculateCanvasParameters(rectangles);

        var canvas = new Bitmap(canvasParameters.Width, canvasParameters.Height);
        using var graphics = Graphics.FromImage(canvas);
        var index = 0;
        foreach (var rectangle in rectangles)
        {
            using var pen = new Pen(Colors[index++ % Colors.Length]);
            rectangle.Offset(canvasParameters.Offset);
            graphics.DrawRectangle(pen, rectangle);
        }

        return canvas;
    }

    private CanvasParameters CalculateCanvasParameters(ICollection<Rectangle> rectangles)
    {
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);
        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var width = maxX - minX;
        var height = maxY - minY;

        return new CanvasParameters
        {
            Width = width + widthOffset,
            Height = height + heightOffset,
            Offset = new Point((width + widthOffset) / 2, (height + heightOffset) / 2)
        };
    }
}