using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class BitmapVisualizer : IDisposable
    {
        private readonly Bitmap bmp;
        private readonly Graphics graphics;
        private readonly Pen pen;
        private readonly Rectangle[] rectangles;

        private Rectangle rectanglesContainer;

        public BitmapVisualizer(Rectangle[] rectangles)
        {
            if (rectangles.Length == 0) throw new ArgumentException("rectangles should contain at least 1 rectangle");
            this.rectangles = rectangles ?? throw new ArgumentNullException();
            rectanglesContainer = rectangles.GetRectanglesContainer();
            var optimalSize = GetOptimalBitmapSize();
            bmp = new Bitmap(optimalSize.Width, optimalSize.Height);
            OffsetRectanglesToCenter();
            graphics = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black);
        }

        public int Width => bmp.Width;
        public int Height => bmp.Height;


        public void Dispose()
        {
            bmp?.Dispose();
            graphics?.Dispose();
            pen?.Dispose();
        }

        private Size GetOptimalBitmapSize()
        {
            return new Size(rectanglesContainer.Width + 200, rectanglesContainer.Height + 200);
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
            var resizedBmp = Resize(scaleCoeff);
            if (dir.CanWrite())
                resizedBmp.Save(Path.Combine(dir.FullName, fileName));
            else
                throw new AccessViolationException("can't write to this directory");
            resizedBmp.Dispose();
        }

        private Bitmap Resize(double scale)
        {
            if (scale <= 0) throw new ArgumentException("scale should be positive");
            var newWidth = (int) (Width * scale);
            var newHeight = (int) (Height * scale);
            var result = new Bitmap(newWidth, newHeight);
            var graphics = Graphics.FromImage(result);
            graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);
            graphics.Dispose();
            return result;
        }

        public void DrawRectangles(Color backgroundColor, Color outlineColor)
        {
            graphics.Clear(backgroundColor);
            pen.Color = outlineColor;
            graphics.DrawRectangles(pen, rectangles);
        }
    }
}