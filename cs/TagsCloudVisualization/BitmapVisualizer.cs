using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class BitmapVisualizer : IDisposable
    {
        private const int IndentFromRectangles = 200;

        private readonly Bitmap bmp;
        private readonly Rectangle[] rectangles;

        private Rectangle rectanglesContainer;

        public BitmapVisualizer(Rectangle[] rectangles)
        {
            this.rectangles = rectangles ?? throw new ArgumentNullException();
            if (rectangles.Length == 0) throw new ArgumentException("rectangles should contain at least 1 rectangle");
            rectanglesContainer = rectangles.GetRectanglesContainer();
            var optimalBmpSize = GetOptimalBitmapSize();
            this.bmp = new Bitmap(optimalBmpSize.Width, optimalBmpSize.Height);
            OffsetRectanglesToCenter();
        }

        public int Width => bmp.Width;
        public int Height => bmp.Height;


        public void Dispose()
        {
            bmp.Dispose();
        }

        private Size GetOptimalBitmapSize()
        {
            return new Size(rectanglesContainer.Width + IndentFromRectangles, rectanglesContainer.Height + IndentFromRectangles);
        }

        private void OffsetRectanglesToCenter()
        {
            var containerCenterX = (rectanglesContainer.Left + rectanglesContainer.Right) / 2;
            var containerCenterY = (rectanglesContainer.Top + rectanglesContainer.Bottom) / 2;
            var bmpCenter = new Point(Width / 2, Height / 2);
            var offset = new Point(bmpCenter.X - containerCenterX, bmpCenter.Y - containerCenterY);
            for (var i = 0; i < rectangles.Length; i++) rectangles[i].Offset(offset);
        }

        public void Save(string fileName, double scaleCoeff = 1, DirectoryInfo dir = null)
        {
            dir = dir ?? new DirectoryInfo(Environment.CurrentDirectory);
            if (!dir.Exists) dir.Create();
            using (var resizedBmp = Resize(scaleCoeff))
            {
                var path = Path.Combine(dir.FullName, fileName);
                try
                {
                    resizedBmp.Save(path);
                }
                catch
                {
                    throw new Exception($"Can't save file to: {path}");
                }
            }
        }

        private Bitmap Resize(double scale)
        {
            if (scale <= 0) throw new ArgumentException("scale should be positive");
            var newWidth = (int)(Width * scale);
            var newHeight = (int)(Height * scale);
            var result = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(result))
                graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);
            return result;

        }

        public void DrawRectangles(Color backgroundColor, Color outlineColor)
        {
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(backgroundColor);
                using (var pen = new Pen(outlineColor))
                {
                    graphics.DrawRectangles(pen, rectangles);
                }
            }

        }
    }
}