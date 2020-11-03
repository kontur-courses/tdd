using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.Core
{
    internal static class CircularCloudVisualization
    {
        private static readonly Random Random = new Random();

        public static Bitmap SaveImage(IReadOnlyCollection<Rectangle> rectangles, int imageWidth, int imageHeight)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            foreach (var rect in rectangles)
                Graphics.FromImage(image).FillRectangle(new SolidBrush(
                    Color.FromArgb(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255))), rect);

            return image;
        }
    }
}