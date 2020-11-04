using System;
using System.Drawing;
using System.Linq;

namespace CloudTag
{
    public static class CloudPainter
    {
        public static Bitmap DrawTagCloud(Pen pen, CircularCloudLayouter layouter)
        {
            var rectangles = layouter.GetRectangles().ToArray();
            
            var width = rectangles.Max(rectangle => rectangle.Right) +
                        Math.Abs(rectangles.Min(rectangle => rectangle.Left));

            var height = rectangles.Max(rectangle => rectangle.Bottom) +
                         Math.Abs(rectangles.Min(rectangle => rectangle.Top));
            
            var bitmap = new Bitmap(width, height);
            Graphics.FromImage(bitmap).DrawRectangles(pen, rectangles);

            return bitmap;
        }
    }
}