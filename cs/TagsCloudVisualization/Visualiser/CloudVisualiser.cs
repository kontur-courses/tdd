using System.Drawing;

namespace TagsCloudVisualization;

public class CloudVisualiser : ICloudVisualiser
{
    private const int IMAGE_MARGIN = 200;
    public CloudVisualiser() { }

    public void VisualiseAndSave(IEnumerable<Rectangle> rectangles, string path)
    {
        var width = GetWidthOfImage(rectangles);
        var center = width / 2;
        rectangles = rectangles.Select(r => new Rectangle(center + r.X, center + r.Y, r.Width, r.Height));
        var bitmap = new Bitmap(width, width);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);

        foreach (var rectangle in rectangles)
        {
            var brash = new SolidBrush(GetRandomColor());
            graphics.FillRectangle(brash, rectangle);
        }

        bitmap.Save(path);
    }

    private Color GetRandomColor()
    {
        var random = new Random();

        return Color.FromArgb(random.Next(0, 255), random.Next(0,255), random.Next(0,255));
    }

    private int GetWidthOfImage(IEnumerable<Rectangle> rectangles)
    {
        var left = rectangles.Min(r => r.Left);
        var right = rectangles.Max(r => r.Right);

        return right - left + IMAGE_MARGIN;
    }
}
