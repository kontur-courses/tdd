using System.Drawing;

namespace TagsCloudVisualization;

public static class BitmapExtensions
{
#pragma warning disable CA1416
    
    public static Bitmap DrawRectangles(this Bitmap bitmap, Rectangle[] rectangles, Pen pen)
    {
        var graphics = Graphics.FromImage(bitmap);
        graphics.DrawRectangles(pen, rectangles);
        graphics.Dispose();
        return bitmap;
    }

    public static Bitmap DrawEllipse(this Bitmap bitmap, Rectangle boundingRect, Pen pen)
    {
        var graphics = Graphics.FromImage(bitmap);
        graphics.DrawEllipse(pen, boundingRect);
        graphics.Dispose();
        return bitmap;
    }

#pragma warning restore CA1416
}