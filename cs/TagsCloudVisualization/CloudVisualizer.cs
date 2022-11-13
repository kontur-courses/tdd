using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CloudVisualizer
    {
        private readonly Point center;
        private readonly string path;
        private readonly string imageName;
        private readonly List<Rectangle> rectangles;
        private readonly Size imageSize;
        private readonly Bitmap image;
        private readonly List<Rectangle> centeredRectangles;

        public CloudVisualizer(
            Point center,
            List<Rectangle> rectangles,
            string path,
            string imageName)
        {
            this.center = center;
            this.path = path;
            this.imageName = imageName;
            this.rectangles = rectangles;
            imageSize = GetImageSize();
            image = new Bitmap(imageSize.Width, imageSize.Height);
            centeredRectangles = CenterRectangles();
        }

        public void CreateImage()
        {
            var gr = Graphics.FromImage(image);
            var brush = new SolidBrush(Color.Black);
            var pen = new Pen(Color.Black);

            gr.FillRectangle(brush, new Rectangle(0, 0, image.Width, image.Height));

            foreach (var rectangle in centeredRectangles)
            {
                pen.Color = GetRandomColor();
                gr.DrawRectangle(pen, rectangle);
            }

            image.Save($"{path}{imageName}.png", ImageFormat.Png);
        }

        private Size GetImageSize()
        {
            if (rectangles.Count == 0)
                return new Size(800, 800);

            var maxY = rectangles.Max(rectangle => rectangle.Bottom);
            var minY = rectangles.Min(rectangle => rectangle.Top);
            var maxX = rectangles.Max(rectangle => rectangle.Right);
            var minX = rectangles.Min(rectangle => rectangle.Left);

            return new Size(maxX - minX + 100, maxY - minY + 100);
        }

        private Point GetCenteredRectangleCoordinates(Rectangle rectangle)
        {
            return new Point(imageSize.Width / 2 + rectangle.X - center.X,
                imageSize.Height / 2 + rectangle.Y - center.Y);
        }

        private List<Rectangle> CenterRectangles()
        {
            return rectangles.Select(rectangle => new Rectangle(
                GetCenteredRectangleCoordinates(rectangle), rectangle.Size)).ToList();
        }

        private static Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromArgb(
                (byte)random.Next(0, 255),
                (byte)random.Next(0, 255),
                (byte)random.Next(0, 255));
        }
    }
}