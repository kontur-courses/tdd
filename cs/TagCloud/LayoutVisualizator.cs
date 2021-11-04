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
        private static readonly Pen DefaultPen;
        private static readonly string DefaultSavePath;
        private static readonly string DefaultFileNamePrefix;

        private Bitmap canvas;

        static LayoutVisualizator()
        {
            DefaultPen = new Pen(ParametersKeeper.DefaultColor, ParametersKeeper.DefaultPenWidth);
            DefaultSavePath = GetDesktopPath();
            DefaultFileNamePrefix = "TagCloud";
        }

        public Bitmap VisualizeCloud(List<Rectangle> rectangles)
        {
            canvas = CreateCanvas(rectangles);
            var g = Graphics.FromImage(canvas);

            DrawBoundary(g);
            DrawAxis(g);
            DrawRectangles(g, rectangles);

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

        private Bitmap CreateCanvas(List<Rectangle> rectangles)
        {
            var canvasSize = GetSize(rectangles);
            rectangles = RelocateRectangles(rectangles, canvasSize);
            return new Bitmap(canvasSize.Width, canvasSize.Height);
        }

        private Size GetSize(List<Rectangle> rectangles)
        {
            var width
                = rectangles.Max(rect => rect.X + rect.Width) - rectangles.Min(rect => rect.X);
            var height
                = rectangles.Max(rect => rect.Y + rect.Height) - rectangles.Min(rect => rect.Y);

            var sizeMultiplier = 2;
            return new Size(width * sizeMultiplier, height * sizeMultiplier);
        }

        private List<Rectangle> RelocateRectangles(List<Rectangle> rectangles, Size canvasSize)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                var newX = rectangles[i].X + canvasSize.Width / 2;
                var newY = rectangles[i].Y + canvasSize.Height / 2;

                var newRect = new Rectangle(new Point(newX, newY), rectangles[i].Size);
                rectangles[i] = newRect;
            }

            return rectangles;
        }

        private void DrawBoundary(Graphics g)
        {
            DefaultPen.Color = Color.Red;
            DefaultPen.Width = 3;

            var location = new Point(0, 0);
            var boundarySize = new Size(canvas.Width - 1, canvas.Height - 1);
            var boundary = new Rectangle(location, boundarySize);

            g.DrawRectangle(DefaultPen, boundary);

            ResetPenToDefault();
        }

        private void DrawAxis(Graphics g)
        {
            DefaultPen.Color = Color.Green;
            DefaultPen.Width = 2;

            g.DrawLine(DefaultPen, canvas.Width / 2, 0, canvas.Width / 2, canvas.Height);
            g.DrawLine(DefaultPen, 0, canvas.Height / 2, canvas.Width, canvas.Height / 2);

            ResetPenToDefault();
        }

        private void DrawRectangles(Graphics g, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                g.DrawRectangle(DefaultPen, rectangle);
        }

        private void ResetPenToDefault()
        {
            DefaultPen.Color = ParametersKeeper.DefaultColor;
            DefaultPen.Width = ParametersKeeper.DefaultPenWidth;
        }

        private void OpenLayout(string fileName)
        {
            Process.Start(fileName);
        }

        private static string GetDesktopPath()
        {
            var desktopPathSuffix = Path.Combine("desktop", "layouts");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), desktopPathSuffix);
        }

        private static class ParametersKeeper
        {
            public static readonly Color DefaultColor = Color.Black;
            public static readonly int DefaultPenWidth = 1;
        }
    }
}