using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.LayoutGeneration;

namespace TagsCloudVisualization.LayoutVizualization
{
    public class Vizualizer
    {
        public Bitmap GetLayoutImage(List<Rectangle> rectangles)
        {
            var sizeBackground = GetSizeBackground(rectangles);

            var mainFrame = new Rectangle(new Point(0, 0), sizeBackground);
            rectangles = ScaleRectangles(rectangles, mainFrame);

            var image = new Bitmap(sizeBackground.Width, sizeBackground.Height);
            var pen = new Pen(Color.Black, 1);
            var graphics = Graphics.FromImage(image);
            
            graphics.FillRectangle(Brushes.Wheat, mainFrame);
            var colors = new List<Brush>()
            {
                Brushes.Red,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.Cyan,
                Brushes.DarkMagenta,
                Brushes.Aqua,
                Brushes.Chartreuse,
                Brushes.HotPink
            };

            var random = new Random();
            foreach (var rectangle in rectangles)
            {
                var numberColor = random.Next(0, colors.Count);
                graphics.FillRectangle(colors[numberColor], rectangle);
                graphics.DrawRectangle(pen, rectangle);
            }

            return image;
        }

        private static List<Rectangle> ScaleRectangles(List<Rectangle> rectangles, Rectangle mainFrame)
        {
            var rects = new List<Rectangle>();
            var mainFrameCenter = mainFrame.GetRectangleCenter();
            var first = rectangles.First();
            var firstRectangleCenter = first.GetRectangleCenter();
            var deltaY = mainFrameCenter.Y - firstRectangleCenter.Y;
            var deltaX = mainFrameCenter.X - firstRectangleCenter.X;

            deltaY -= first.Height / 2;
            deltaX -= first.Width / 2;

            foreach (var rectangle in rectangles)
            {
                rectangle.Offset(deltaX, deltaY);
                rects.Add(rectangle);
            }

            return rects;
        }

        private static Size GetSizeBackground(List<Rectangle> rectangles)
        {
            var rightLeftOffset = 30;
            var topBottomOffset = 60;
            var max_X = rectangles.Select(r => r.Right).Max() + rightLeftOffset;
            var max_Y = rectangles.Select(r => r.Top).Max() + topBottomOffset;

            var min_X = rectangles.Select(r => r.Left).Min();
            var min_Y = rectangles.Select(r => r.Bottom).Min();

            var width = Math.Sqrt(Math.Pow((max_X - min_X), 2));
            var height = Math.Sqrt(Math.Pow((max_Y - min_Y), 2));

            Size sizeBackground = new Size((int) width, (int) height);
            return sizeBackground;
        }

        public void SaveImage(string path, Bitmap img)
        {
            img.Save(path);
        }
    }
}