using System.Drawing;

namespace TagsCloud;

public class RectanglesVisualizer : IRectanglesVisualizer
{
    private List<Rectangle> rectangles;
    private Graphics graphics;
    private Point shift;
    private const int Border = 50;

    public Bitmap GetTagsCloudImage(List<Rectangle> rectangles)
    {
        this.rectangles = rectangles;
        var sizeImage = GetImageSize();
        var image = new Bitmap(sizeImage.Width, sizeImage.Height);
        using (graphics = Graphics.FromImage(image))
        {
            var background = new SolidBrush(Color.Black);
            graphics.FillRectangle(background, new Rectangle(0, 0, image.Width, image.Height));
            DrawTagsCloud();
            return image;
        }
    }

    private Size GetImageSize()
    {
        Size defaultSize = new(100, 100);

        if (!rectangles.Any())
            return defaultSize;

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

    private void DrawTagsCloud()
    {
        foreach (var rectangle in rectangles)
        {
            var vizRect = new Rectangle(new Point(rectangle.X + shift.X, rectangle.Y + shift.Y), rectangle.Size);
            using var pen = new Pen(Utils.GetRandomColor());
            graphics.DrawRectangle(pen, vizRect);
        }
    }
}