using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectanglePainter
    {
        public static Bitmap GetBitmapWithRectangles(IEnumerable<Rectangle> rectangles)
        {
            var rects = rectangles.ToArray();
            var center = new Size(rects[0].X + rects[0].Width / 2, rects[0].Y + rects[0].Height / 2);

            var bmp = new Bitmap(
                Math.Abs(rects.Max(x => x.Right) - rects.Min(x => x.Left)) + 100,
                Math.Abs(rects.Max(x => x.Bottom) - rects.Min(x => x.Top)) + 100
                );

            var centersDelta = new Size(center.Width - bmp.Width / 2, center.Height - bmp.Height / 2);
            
            using (var graphics = Graphics.FromImage(bmp))
            {
                DrawRectangles(graphics, rects.Select(x => new Rectangle(
                    x.X - centersDelta.Width , x.Y - centersDelta.Height , x.Width, x.Height
                    )));
            }

            return bmp;
        }

        public static void DrawRectangles(Graphics graphics, IEnumerable<Rectangle> rectangles)
        {
            var rnd = new Random();

            foreach (var rectangle in rectangles)
            {
                using (var pen = new Pen(Brushes.Black, 1))
                {
                    pen.Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
        }
    }
}