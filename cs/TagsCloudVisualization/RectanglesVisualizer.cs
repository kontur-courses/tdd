using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static TagsCloudVisualization.RectanglesVisualizerСalculations;

namespace TagsCloudVisualization
{
    class RectanglesVisualizer : IDisposable
    {
        private readonly List<Rectangle> rectangles;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public RectanglesVisualizer(Size imageSize, List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        private void DrawCenter(Point center)
        {
            var brush = Brushes.Red;
            var location = center - new Size(1, 1);
            graphics.FillRectangle(brush, new Rectangle(location, new Size(2, 2)));
        }

        private void DrawRectangles()
        {
            var brush = Brushes.Green;
            graphics.FillRectangles(brush, rectangles.ToArray());
            var pen = new Pen(Color.Black);
            graphics.DrawRectangles(pen, rectangles.ToArray());
        }

        private void Save(string directoryName, string filename)
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory);
            while (directoryInfo != null && directoryInfo.Name != "TagsCloudVisualization")
                directoryInfo = directoryInfo.Parent;
            if (directoryInfo == null)
                throw new DirectoryNotFoundException("Directory with the name TagsCloudVisualization not found");
            Directory.CreateDirectory(directoryInfo.FullName + @"\" + directoryName);
            bitmap.Save(directoryInfo.FullName + $@"\{directoryName}\{filename}");
        }

        public static void SaveNewRectanglesLayout(List<Rectangle> rectangles, string directoryName, string filename)
        {
            var imageSize = GetOptimalSizeForImage(rectangles, 5);
            var center = GetCenter(imageSize);
            var offset = new Size(center);
            rectangles = GetRectanglesWithOptimalLocation(rectangles, offset);
            using var visualizer = new RectanglesVisualizer(imageSize, rectangles);
            visualizer.DrawRectangles();
            visualizer.DrawCenter(center);
            visualizer.Save(directoryName, filename);
        }

        public void Dispose()
        {
            bitmap?.Dispose();
            graphics?.Dispose();
        }
    }
}
