using System.Drawing;
using System.Drawing.Drawing2D;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class BitmapTagsCloudVisualization : ITagsCloudVisualization
{
    public void SaveTagsCloud(CircularCloudLayouter layouter, string path)
    {
        var bitmap = DrawRectangles(layouter);

        bitmap.Save(path);
    }


    private static Bitmap DrawRectangles(CircularCloudLayouter layouter)
    {
        var image = new Bitmap(1024, 1024);
        var graphics = Graphics.FromImage(image);
        graphics.Clear(Color.White);
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        var pen = new Pen(Brushes.Black);
        foreach (var rectangle in layouter.Rectangles)
        {
            var offsetRectangle = GetRectangleOffsetToCenter(rectangle, layouter);
            graphics.DrawRectangle(pen, offsetRectangle);
        }

        return image;
    }

    private static Rectangle GetRectangleOffsetToCenter(Rectangle rectangle, CircularCloudLayouter layouter)
    {
        Point rectanglePosition = new Point(rectangle.Location.X + 1024 / 2, rectangle.Location.Y + 1024 / 2);
        return new Rectangle(rectanglePosition, rectangle.Size);
    }
}