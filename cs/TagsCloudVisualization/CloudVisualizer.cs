using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.ComTypes;

namespace TagsCloudVisualization;

public class CloudVisualizer
{
    private readonly Point center;
    private readonly string path;
    private readonly string imageName;
    private readonly List<Rectangle> rectangles;
    private readonly int indent;

    public CloudVisualizer(
        Point center, List<Rectangle> rectangles, int indent = 200,
        string path = @"..\..\..\Images\", string imageName = "Cloud")
    {
        this.center = center;
        this.path = path;
        this.imageName = $"{imageName} {rectangles.Count}";
        this.rectangles = rectangles;
        this.indent = indent;
    }

    private Size GetImageSize()
    {
        var maxY = rectangles.Max(rectangle => rectangle.Bottom);
        var minY = rectangles.Min(rectangle => rectangle.Top);
        var maxX = rectangles.Max(rectangle => rectangle.Right);
        var minX = rectangles.Min(rectangle => rectangle.Left);

        return new Size(maxX + indent - minX, maxY + indent  - minY);
    }

    private List<Rectangle> CenterRectangles(Size imageSize)
    {
        return rectangles.Select(rectangle =>
            new Rectangle(new Point(
                imageSize.Height / 2 + rectangle.X - center.X,
                imageSize.Width / 2 + rectangle.Y - center.Y), rectangle.Size)).ToList();
    }

    public List<Color> GetColors()
    {
        return new List<Color>()
        {
            Color.CornflowerBlue,
            Color.Blue,
            Color.White,
            Color.DarkSlateBlue,
            Color.DeepSkyBlue,
            Color.DarkBlue,
            Color.Red
        };
    }

    public void CreateImage()
    {
        var imageSize = GetImageSize();
        var image = new Bitmap(imageSize.Width, imageSize.Height);
        var gr = Graphics.FromImage(image);
        var brush = new SolidBrush(Color.Black);
        var pen = new Pen(Color.Black);
        var colors = GetColors();
        var centerReactangles = CenterRectangles(imageSize);

        gr.FillRectangle(brush, new Rectangle(0, 0, image.Width, image.Height));

        for (int i = 0; i < centerReactangles.Count; i++)
        {
            pen.Color = colors[i % colors.Count];
            gr.DrawRectangle(pen, centerReactangles[i]);
        }

        image.Save($"{path}{imageName}.png", ImageFormat.Png);
    }
}