using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace CloudTag
{
    public static class CloudPainter
    {
        public static void DrawAndSaveToFile(Color penColor, Rectangle[] rectangles, string pathWithName)
        {
            if(rectangles.Length == 0)
                return;
            
            var width = rectangles.Max(rectangle => rectangle.Right) +
                        Math.Abs(rectangles.Min(rectangle => rectangle.Left));

            var height = rectangles.Max(rectangle => rectangle.Bottom) +
                         Math.Abs(rectangles.Min(rectangle => rectangle.Top));

            var shiftVector = new Point(Math.Abs(rectangles.Min(rect => rect.Left)), Math.Abs(rectangles.Min(rect => rect.Top)));

            var rectanglesToDraw = GetShiftedOnVector(rectangles, shiftVector);

            using (var bitmap = new Bitmap(width + 10, height + 10)) 
            {
                using (var pen = new Pen(penColor))
                    using (var graphics = Graphics.FromImage(bitmap))
                    graphics.DrawRectangles(pen, rectanglesToDraw);
                
                bitmap.Save(pathWithName, ImageFormat.Png);
            }
        }

        private static Rectangle[] GetShiftedOnVector(Rectangle[] rectangles, Point vector)
        {
            return rectangles.Select(rectangle =>
                new Rectangle(new Point(rectangle.X + vector.X, rectangle.Y + vector.Y), rectangle.Size)).ToArray();
        }
    }
}