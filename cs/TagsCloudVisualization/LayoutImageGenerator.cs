using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class LayoutImageGenerator
    {
        private const int AddedImageSize = 200;

        public static string GenerateImage(Point center, int rectanglesCount)
        {
            var layouter = CircularCloudLayouter.Generate(center, rectanglesCount);
            return CreateImage(layouter);
        }

        public static string CreateImage(CircularCloudLayouter layouter)
        {
            if (!layouter.Rectangles.Any())
                throw new ArgumentException("No rectangles received");
            var imageSize = CalculateCoverageSize(layouter.Rectangles)
                + new Size(AddedImageSize, AddedImageSize);
            var imageOffset = imageSize / 2 - new Size(layouter.Center);

            return CreateImage(layouter.Rectangles, imageSize,
                imageOffset, Path.Combine(Path.GetFullPath(@"..\..\..\"), "layouts"));
        }

        public static string CreateImage(IReadOnlyCollection<Rectangle> rectangles, 
            Size imageSize, Size imageOffset, string path)
        {            
            var bm = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bm);

            foreach (var rectangle in rectangles)
            {
                rectangle.Offset(new Point(imageOffset));
                graphics.DrawRectangle(new Pen(Color.DarkOrange, 4), rectangle);
            }

            var savePath = $"{path}\\layout_{DateTime.Now:HHmmssddMM}.bmp";
            bm.Save(savePath, ImageFormat.Bmp);
            return savePath;
        }

        public static Size CalculateCoverageSize(IEnumerable<Rectangle> rectangles)
        {
            var maxX = rectangles.Max(x => x.X + x.Size.Width);           
            var minX = rectangles.Min(x => x.X);
            var width = maxX - minX;

            var maxY = rectangles.Max(x => x.Y);
            var minY = rectangles.Min(x => x.Y - x.Size.Height);            
            var height = maxY - minY;

            return new Size(width, height);
        }
    }
}
