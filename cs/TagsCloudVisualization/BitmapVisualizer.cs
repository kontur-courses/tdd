using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace TagsCloudVisualization
{
    public class BitmapVisualizer: IDisposable
    {
        private readonly Bitmap bmp;
        private readonly Graphics graphics;
        private readonly Rectangle[] rectangles;
        private readonly Pen pen;

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

        private Size GetOptimalBitmapSize() =>
            new Size(rectanglesContainer.Width + 200, rectanglesContainer.Height + 200);

        private void OffsetRectanglesToCenter()
        {
            var containerCenterX = (rectanglesContainer.Left + rectanglesContainer.Right) / 2;
            var containerCenterY = (rectanglesContainer.Top + rectanglesContainer.Bottom) / 2;
            var bmpCenter = new Point(Width / 2, Height / 2);
            var offset = new Point(bmpCenter.X - containerCenterX, bmpCenter.Y - containerCenterY);
            for (var i = 0; i < rectangles.Length; i++)
            {
                rectangles[i].Offset(offset);
            }
        }

        public int Width => bmp.Width;
        public int Height => bmp.Height;

        public void Save(string fileName, DirectoryInfo dir = null)
        {
            dir = dir ?? new DirectoryInfo(Environment.CurrentDirectory);
            if(!dir.Exists) dir.Create();
            if(dir.CanWrite())
                bmp.Save(Path.Combine(dir.FullName, fileName));
            else
                throw new AccessViolationException("can't write to this directory");
            
        }

        public void DrawRectangles(Color backgroundColor, Color outlineColor)
        {
            graphics.Clear(backgroundColor);
            pen.Color = outlineColor;
            graphics.DrawRectangles(pen, rectangles);
        }


        public void Dispose()
        {
            bmp?.Dispose();
            graphics?.Dispose();
            pen?.Dispose();
        }
    }
}