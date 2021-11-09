using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class BitmapDrawer
    {
        private static int _count;

        public static Bitmap Draw(IEnumerable<Rectangle> rectangles, Point center, List<Color> colors = null)
        {
            var bitmapSize = GetCanvasSize(rectangles, center);
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var bitmapCenter = new Point(bitmap.Width / 2, bitmap.Height / 2);

            var rnd = new Random(Seed: 100);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, 0, 0, bitmapSize.Width, bitmapSize.Height);
                foreach (var rect in rectangles)
                {
                    var color = colors == null
                        ? Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256))
                        : colors[rnd.Next(colors.Count - 1)];
                    var pen = new Pen(color, 0) { Alignment = PenAlignment.Inset };
                    var brush = new SolidBrush(Color.FromArgb(25, color));
                    rect.Offset(new Point(bitmapCenter.X - center.X, bitmapCenter.Y - center.Y));
                    g.DrawRectangle(pen, rect);
                    g.FillRectangle(brush, rect);
                }
            }
            return bitmap;
        }

        private static Size GetCanvasSize(IEnumerable<Rectangle> rectangles, Point center)
        {
            var union = rectangles.First().UnionRange(rectangles);
            var distances = union.GetDistancesToInnerPoint(center);
            var horizontalIncrement = Math.Abs(distances[0] - distances[2]);
            var verticalIncrement = Math.Abs(distances[1] - distances[3]);
            union.Inflate(horizontalIncrement, verticalIncrement);
            return new Size((int)(union.Width * 1.1), (int)(union.Height*1.1));
        }

        public static void Save(Bitmap bitmap, string filename = null)
        {
            var name = filename ?? $"tag_cloud_0{_count}";
            bitmap.Save($"{name}.png", ImageFormat.Png);
            _count++;
        }
    }
}
