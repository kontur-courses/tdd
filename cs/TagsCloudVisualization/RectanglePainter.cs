using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectanglePainter
    {
        private const int Margin = 100;
        
        public static Bitmap GetBitmapWithRectangles(IEnumerable<Rectangle> rectangles)
        {
            var rects = rectangles.ToList();
            if (rects.Count is 0)
                throw new ArgumentException($"rectangle list is empty");

            var middleRectangle = rects.First();
            var actualCenter = new Point(middleRectangle.X + middleRectangle.Width / 2, 
                                         middleRectangle.Y + middleRectangle.Height / 2);

            var bmp = new Bitmap(
                Math.Abs(rects.Max(x => x.Right) - rects.Min(x => x.Left)) + Margin,
                Math.Abs(rects.Max(x => x.Bottom) - rects.Min(x => x.Top)) + Margin
                );

            var centersDelta = new Size(actualCenter.X - bmp.Width / 2, 
                                        actualCenter.Y - bmp.Height / 2);

            using var graphics = Graphics.FromImage(bmp);
            DrawRectangles(graphics, rects.Select(rect => rect.Move(-centersDelta.Width, -centersDelta.Height)));

            return bmp;
        }

        // Подразумевается, что этот метод будет использоваться из вне, просто еще случай не подвернулся.
        // ReSharper disable once MemberCanBePrivate.Global
        public static void DrawRectangles(Graphics graphics, IEnumerable<Rectangle> rectangles)
        {
            var rnd = new Random();

            foreach (var rectangle in rectangles)
            {
                using var pen = new Pen(Brushes.Black, 1);
                pen.Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                graphics.DrawRectangle(pen, rectangle);
            }
        }
    }
}