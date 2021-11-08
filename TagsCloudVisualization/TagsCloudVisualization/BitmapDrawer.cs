using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace TagsCloudVisualization
{
    public class BitmapDrawer
    {
        private static int _count;

        public static Bitmap Draw(CircularCloudLayouter layouter)
        {
            var bitmapSize = Size.Round(new SizeF(layouter.CanvasSize.Width * 1.1f, layouter.CanvasSize.Height * 1.1f));
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);

            var rnd = new Random(Seed: 100);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmapSize.Width, bitmapSize.Height);
            foreach (var rect in layouter.Rectangles)
            {
                var color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
                var pen = new Pen(color, 0) { Alignment = PenAlignment.Inset };
                var brush = new SolidBrush(Color.FromArgb(25, color));
                rect.Offset(new Point(center.X - layouter.Center.X, center.Y - layouter.Center.Y));
                g.DrawRectangle(pen, rect);
                g.FillRectangle(brush, rect);
            }
            g.Dispose();
            return bitmap;
        }

        public static void Save(Bitmap bitmap, string filename = null)
        {
            if (filename != null)
            {
                bitmap.Save($"{filename}.png", ImageFormat.Png);
            }
            else
            {
                bitmap.Save($"tag_cloud_0{_count}.png", ImageFormat.Png);
            }
            _count++;
        }
    }
}
