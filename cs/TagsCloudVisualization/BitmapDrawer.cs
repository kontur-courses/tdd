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

        private CircularCloudLayouter _layouter;
        private Size _bitmapSize;
        private Bitmap _bitmap;
        private Point Center { get => new Point(_bitmapSize.Width / 2, _bitmapSize.Height / 2); }

        public BitmapDrawer(CircularCloudLayouter layouter)
        {
            _layouter = layouter;
            _bitmapSize = Size.Round(new SizeF(layouter.CanvasSize.Width * 1.1f, layouter.CanvasSize.Height * 1.1f));
            _bitmap = new Bitmap(_bitmapSize.Width, _bitmapSize.Height);
        }

        public void Draw()
        {
            var rnd = new Random(Seed: 100);
            var g = Graphics.FromImage(_bitmap);
            g.FillRectangle(Brushes.White, 0, 0, _bitmapSize.Width, _bitmapSize.Height);
            foreach (var rect in _layouter.Rectangles)
            {
                var color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
                var pen = new Pen(color, 0)
                {
                    Alignment = PenAlignment.Inset
                };
                rect.Offset(new Point(Center.X - _layouter.Center.X, Center.Y - _layouter.Center.Y));
                g.DrawRectangle(pen, rect);
            }
        }

        public void Save(string filename = null)
        {
            if (filename != null)
            {
                _bitmap.Save($"{filename}.png", ImageFormat.Png);
            }
            else
            {
                _bitmap.Save($"tag_cloud_0{_count}.png", ImageFormat.Png);
            }
            _count++;
        }
    }
}
