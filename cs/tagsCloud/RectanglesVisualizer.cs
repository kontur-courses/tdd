using System.Drawing;

namespace tagsCloud;

public class RectanglesVisualizer
{
    private readonly List<Rectangle> rectangles;
    private readonly Bitmap image;
    private readonly Graphics graphics;
    private Point shift;
    private static readonly Size DefaultSize = new(100, 100);
    private const int Border = 50;


    public RectanglesVisualizer(List<Rectangle> rectangles)
    {
        this.rectangles = rectangles;
        var sizeImage = PrepareImage();
        image = new Bitmap(sizeImage.Width, sizeImage.Height);
        graphics = Graphics.FromImage(image);
        var background = new SolidBrush(Color.Black);
        graphics.FillRectangle(background, new Rectangle(0, 0, image.Width, image.Height));
    }

    private Size PrepareImage()
    {
        if (!rectangles.Any())
            return DefaultSize;

        var leftmost = rectangles.Min(rectangle => rectangle.Left);
        var rightmost = rectangles.Max(rectangle => rectangle.Right);
        var topmost = rectangles.Min(rectangle => rectangle.Top);
        var bottommost = rectangles.Max(rectangle => rectangle.Bottom);

        var startX = topmost >= 0 ? 0 : topmost;
        var startY = leftmost >= 0 ? 0 : leftmost;
        shift = new Point(Math.Abs(startX) + Border, Math.Abs(startY) + Border);

        var height = Math.Abs(bottommost) + Math.Abs(topmost) + 2 * Border;
        var width = Math.Abs(rightmost) + Math.Abs(topmost) + 2 * Border;

        return new Size(width, height);
    }

    public Bitmap DrawTagCloud()
    {
        foreach (var rectangle in rectangles)
        {
            var vizRect = new Rectangle(new Point(rectangle.X + shift.X, rectangle.Y + shift.Y), rectangle.Size);
            var pen = new Pen(Utils.GetRandomColor());
            graphics.DrawRectangle(pen, vizRect);
        }

        return image;
    }
}