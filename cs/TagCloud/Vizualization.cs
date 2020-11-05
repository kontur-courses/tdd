using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagCloud
{
    public class Vizualizator
    {
        public string Draw(IEnumerable<Rectangle> rectangles, Point center)
        {
            var bitmap = new Bitmap(800, 700);
            var graphics = Graphics.FromImage(bitmap);

            foreach (var rectangle in rectangles)
            {
                DrawAndFillRectangle(graphics, rectangle);
            }
            var path = GetNewPngPath();
            bitmap.Save(path);
            return path;
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