using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class BitmapVisualizer : IDisposable
    {
        private const int IndentFromRectangles = 200;

        private readonly Rectangle[] rectangles;

        private Bitmap bmp;
        private Graphics graphics;
        private Rectangle rectanglesContainer;

        public int BitmapWidth
        {
            get
            {
                if (bmp == null)
                    throw new Exception($"Bitmap instance was not created. Call method {nameof(DrawRectangles)} at least one time");
                return bmp.Width;
            }
        }

        public int BitmapHeight
        {
            get
            {
                if(bmp == null)
                    throw new Exception($"Bitmap instance was not created. Call method {nameof(DrawRectangles)} at least one time");
                return bmp.Height;
            }
        }

        public BitmapVisualizer(Rectangle[] rectangles)
        {
            this.rectangles = rectangles.ToArray() ?? throw new ArgumentNullException();
            if (rectangles.Length == 0) throw new ArgumentException("rectangles should contain at least 1 rectangle");
            rectanglesContainer = rectangles.GetRectanglesContainer();
        }


        public void Dispose()
        {
            graphics?.Dispose();
            bmp?.Dispose();
        }

        private Size GetOptimalBitmapSize()
        {
            return new Size(rectanglesContainer.Width + IndentFromRectangles, rectanglesContainer.Height + IndentFromRectangles);
        }

        private void OffsetRectanglesToCenter()
        {
            var containerCenterX = (rectanglesContainer.Left + rectanglesContainer.Right) / 2;
            var containerCenterY = (rectanglesContainer.Top + rectanglesContainer.Bottom) / 2;
            var bmpCenter = new Point(bmp.Width / 2, bmp.Height / 2);
            var offset = new Point(bmpCenter.X - containerCenterX, bmpCenter.Y - containerCenterY);
            for (var i = 0; i < rectangles.Length; i++) rectangles[i].Offset(offset);
        }

        public void Save(string fileName, double scaleCoeff = 1, DirectoryInfo dir = null)
        {
            if (bmp == null)
                throw new InvalidOperationException($"{nameof(DrawRectangles)} should be called at least one time before saving");
            dir = dir ?? new DirectoryInfo(Environment.CurrentDirectory);
            if (!dir.Exists) dir.Create();
            using (var resizedBmp = Resize(scaleCoeff))
            {
                var path = Path.Combine(dir.FullName, fileName);
                try
                {
                    resizedBmp.Save(path);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Can't save file to: {path}", ex);
                }
            }
        }

        private Bitmap Resize(double scale)
        {
            if (scale <= 0) throw new ArgumentException("scale should be positive");
            var newWidth = (int)(bmp.Width * scale);
            var newHeight = (int)(bmp.Height * scale);
            var result = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(result))
                graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);
            return result;

        }

        private void InitializeBitmap()
        {
            var optimalBmpSize = GetOptimalBitmapSize();
            this.bmp = new Bitmap(optimalBmpSize.Width, optimalBmpSize.Height);
        }

        public void DrawRectangles(Color backgroundColor, Color outlineColor)
        {
            if (bmp == null)
            {
                InitializeBitmap();
                OffsetRectanglesToCenter();
            }
            graphics = graphics ?? Graphics.FromImage(bmp);
            graphics.Clear(backgroundColor);
            using (var pen = new Pen(outlineColor))
            {
                graphics.DrawRectangles(pen, rectangles);
            }
            
        }
    }
}