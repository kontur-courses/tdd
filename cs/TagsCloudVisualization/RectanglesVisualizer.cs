using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Linq;

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

        public void DrawCenter(Point center)
        {
            var brush = Brushes.Red;
            var location = center - new Size(1, 1);
            graphics.FillRectangle(brush, new Rectangle(location, new Size(2, 2)));
        }

        public void DrawRectangles()
        {
            var brush = Brushes.Green;
            graphics.FillRectangles(brush, rectangles.ToArray());
            var pen = new Pen(Color.Black);
            graphics.DrawRectangles(pen, rectangles.ToArray());
        }

        public void Save(string directoryName, string filename)
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory);
            while (directoryInfo != null && directoryInfo.Name != "TagsCloudVisualization")
                directoryInfo = directoryInfo.Parent;
            if (directoryInfo == null)
                throw new DirectoryNotFoundException("Directory with the name TagsCloudVisualization not found");
            Directory.CreateDirectory(directoryInfo.FullName + @"\" + directoryName);
            bitmap.Save(directoryInfo.FullName + $@"\{directoryName}\{filename}");
        }

        public static Size GetOptimalSizeForImage(List<Rectangle> rectangles, int indent)
        {
            if (rectangles.Count == 0)
                throw new ArgumentException("Empty rectangles list");
            var minTop = rectangles.Min(rect => rect.Top);
            var maxBottom = rectangles.Max(rect => rect.Bottom);
            var maxRight = rectangles.Max(rect => rect.Right);
            var minLeft = rectangles.Min(rect => rect.Left);
            var width = maxRight - minLeft;
            var height = maxBottom - minTop;
            return new Size(width + indent * 2, height + indent * 2);
        }

        public static Point GetCenter(Size size)
        {
            return new Point(size.Width / 2, size.Height / 2);
        }

        public static List<Rectangle> GetRectanglesWithOptimalLocation(List<Rectangle> rectangles, Size offset)
        {
            return rectangles
                .Select(rectangle => new Rectangle(rectangle.Location + offset, rectangle.Size))
                .ToList();
        }

        public static void SaveNewRectanglesLayout(List<Rectangle> rectangles, string directoryName, string filename)
        {
            var imageSize = GetOptimalSizeForImage(rectangles, 5);
            var center = GetCenter(imageSize);
            var offset = new Size(center);
            rectangles = GetRectanglesWithOptimalLocation(rectangles, offset);
            var visualizer = new RectanglesVisualizer(imageSize, rectangles);
            visualizer.DrawRectangles();
            visualizer.DrawCenter(center);
            visualizer.Save(directoryName, filename);
            visualizer.Dispose();
        }

        public void Dispose()
        {
            bitmap?.Dispose();
            graphics?.Dispose();
        }
    }
}
