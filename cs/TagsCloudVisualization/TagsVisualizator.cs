using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization
{
    public class TagsVisualizator
    {
        private List<Rectangle> Rectangles { get; }

        public TagsVisualizator(List<Rectangle> rectangles)
        {
            Rectangles = rectangles;
        }

        public void SaveVisualizationToDirectory(string savePath)
        {
            if (!PathInRightFormat(savePath))
            {
                throw new ArgumentException("wrong path format");
            }

            var imageSize = GetImageSize();
            var pen = new Pen(Color.MediumVioletRed, 4);

            using var bitmap = new Bitmap(imageSize.Width + (int)pen.Width,
                imageSize.Width + (int)pen.Width);
            using var graphics = Graphics.FromImage(bitmap);

            if (Rectangles.Count != 0)
            {
                graphics.DrawRectangles(pen, Rectangles.ToArray());
            }

            bitmap.Save(savePath, ImageFormat.Jpeg);
        }

        private bool PathInRightFormat(string path)
        {
            var pattern = @"((?:[^\\]*\\)*)(.*[.].+)";
            var match = Regex.Match(path, pattern);
            var directoryPath = match.Groups[1].ToString();

            return Directory.Exists(directoryPath) && match.Groups[2].Success;
        }

        private Size GetImageSize()
        {
            var width = 0;
            var height = 0;

            foreach (var rectangle in Rectangles)
            {
                if (rectangle.Left < 0 || rectangle.Top < 0)
                {
                    throw new ArgumentException("rectangle out of image boundaries");
                }

                if (width < rectangle.Right)
                {
                    width = rectangle.Right;
                }
                if (height < rectangle.Bottom)
                {
                    height = rectangle.Bottom;
                }
            }

            return new Size(width, height);
        }
    }
}