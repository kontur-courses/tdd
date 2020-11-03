using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagCloud
{
    public static class Vizualizator
    {
        public static void Draw(IEnumerable<Size> rectangleSizes, Point center)
        {
            var bitmap = new Bitmap(700, 800);
            var graphics = Graphics.FromImage(bitmap);
            DrawCenter(graphics, center);

            var layouter = new CircularCloudLayouter(center);
            foreach (var rectangleSize in rectangleSizes)
            {
                var rectangle = layouter.PutNextRectangle(rectangleSize);
                DrawAndFillRectangle(graphics, rectangle);
            }

            bitmap.Save(GetNewPngPath());
        }

        private static void DrawCenter(Graphics graphics, Point center)
        {
            var brush = new SolidBrush(Color.Red);
            graphics.FillRectangle(brush, center.X, center.Y, 1, 1);
        }

        private static string GetNewPngPath()
        {
            var workingDirectory = Directory.GetCurrentDirectory();
            var index = workingDirectory.IndexOf("TagCloud");
            var tagCloudPath = workingDirectory.Substring(0, index);
            return tagCloudPath + "MyPng" +  DateTime.Now.Millisecond + ".png";
        }

        private static void DrawAndFillRectangle(Graphics graphics, Rectangle rectangle)
        {
            var brushColor = Color.FromArgb(rectangle.X % 255, rectangle.Y % 255, 100);
            var brush = new SolidBrush(brushColor);
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            graphics.FillRectangle(brush, rectangle);
        }
    }
}