using System.Drawing;

namespace TagsCloudVisualization;

public class LayoutDrawer
{
    private readonly Pen _pen;

    public LayoutDrawer(Pen pen)
    {
        _pen = pen;
    }

    public void Draw(List<Rectangle> rectangles, string filename)
    {
        var size = GetSize(rectangles);
        var bitmap = new Bitmap(size.Width, size.Height);
        var graphics = Graphics.FromImage(bitmap);
        for (var i = 0; i < rectangles.Count; i++)
        {
            var centredRectangle = CentreRectangle(rectangles[i], rectangles[0].Location, bitmap);
            graphics.DrawRectangle(_pen, centredRectangle);
        }

        bitmap.Save(filename);
    }

    private Size GetSize(List<Rectangle> rectangles)
    {
        var leftBound = rectangles.Min(rectangle => rectangle.Left);
        var rightBound = rectangles.Max(rectangle => rectangle.Right);
        var bottomBound = rectangles.Max(rectangle => rectangle.Bottom);
        var topBound = rectangles.Min(rectangle => rectangle.Top);

        var width = rightBound - leftBound;
        var height = bottomBound - topBound;

        return new Size(width, height);
    }

    private Rectangle CentreRectangle(Rectangle rectangle, Point center, Bitmap bitmap)
    {
        var X = rectangle.Location.X - center.X + bitmap.Width / 2;
        var Y = rectangle.Location.Y - center.Y + bitmap.Height / 2;
        rectangle.Location = new Point(X, Y);
        return rectangle;
    }
}