using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagCloud
{
    public class LayoutVisualizator
    {
        private static readonly string DefaultSavePath = GetDesktopPath();
        private static readonly Pen DefaultPen = new Pen(Color.Black, 1);
        private static readonly string DefaultFileNamePrefix = "TagCloud";

        private Bitmap canvas;

        public Bitmap VisualizeCloud(List<Rectangle> rectangles)
        {
            canvas = CreateCanvas(rectangles);
            var g = Graphics.FromImage(canvas);

            foreach (var rectangle in rectangles)
                g.DrawRectangle(DefaultPen, rectangle);

            return canvas;
        }

        public void SaveToDesktop(bool shouldOpenAfterCreate = true)
        {
            if (canvas == null)
                throw new NullReferenceException("Canvas was not created");

            var currentTime = DateTime.Now.ToString("hh-mm-ss-fff");
            var fileName = string.Join("", DefaultFileNamePrefix, "_", currentTime, ".png");
            var fullFileName = Path.Combine(DefaultSavePath, fileName);

            canvas.Save(fullFileName, ImageFormat.Png);

            if (shouldOpenAfterCreate)
                OpenLayout(fullFileName);
        }

        private void OpenLayout(string fileName)
        {
            Process.Start(fileName);
        }

        private Bitmap CreateCanvas(List<Rectangle> rectangles)
        {
            var canvasSize = GetSize(rectangles);
            return new Bitmap(canvasSize.Width, canvasSize.Height);
        }

        private Size GetSize(List<Rectangle> rectangles)
        {
            var width
                = rectangles.Max(rect => rect.X + rect.Width) - rectangles.Min(rect => rect.X);
            var height
                = rectangles.Max(rect => rect.Y + rect.Height) - rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        private static string GetDesktopPath()
        {
            var desktopPathSuffix = Path.Combine("desktop", "layouts");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), desktopPathSuffix);
        }
    }
}
