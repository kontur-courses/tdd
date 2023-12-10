using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class CloudLayouterDrawer
    {
        private static readonly Random Random = new Random();


        public static void DrawCloudLayout(CircularCloudLayouter layouter, string filename)
        {
            int imageWidth, imageHeight;
            SetImageSize(layouter.WordPositions, out imageWidth, out imageHeight);
            var wordImagePositions = layouter.WordPositions.Select(rec =>
                new Rectangle(rec.X + imageWidth / 2, rec.Y + imageHeight / 2, rec.Width, rec.Height));


            using (var bitmap = new Bitmap(imageWidth, imageHeight))
            {
                foreach (var rectangle in wordImagePositions)
                    DrawRectangle(bitmap, rectangle);
                SaveImageAsPng(bitmap, filename);
            }
        }


        private static void DrawRectangle(Bitmap bitmap, Rectangle rectangle)
        {
            var randomColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
            Brush brush = new SolidBrush(randomColor);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(brush, rectangle);
            }
        }


        private static void SaveImageAsPng(Bitmap bitmap, string filename)
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var filePath = Path.Combine(projectDirectory, "images", filename);
            bitmap.Save(filePath, ImageFormat.Png);
        }


        private static void SetImageSize(IEnumerable<Rectangle> rectangles, out int imageWidth, out int imageHeight)
        {
            var maxX = rectangles.Select(rec => rec.X + rec.Width / 2).Max();
            var minX = rectangles.Select(rec => rec.X - rec.Width / 2).Min();
            var maxY = rectangles.Select(rec => rec.Y + rec.Height / 2).Max();
            var minY = rectangles.Select(rec => rec.Y - rec.Height / 2).Min();

            imageWidth = maxX - minX + 10;
            imageHeight = maxY - minY + 10;
        }
    }
}