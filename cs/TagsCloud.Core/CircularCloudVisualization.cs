using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.Core
{
    internal static class CircularCloudVisualization
    {
        public const int WindowWidth = 1920;
        public const int WindowHeight = 1080;
        private static readonly Random Random = new Random();

        public static void SaveImage(IReadOnlyCollection<Rectangle> rectangles, string imageName)
        {
            var image = new Bitmap(WindowWidth, WindowHeight);
            foreach (var rect in rectangles)
                Graphics.FromImage(image).FillRectangle(new SolidBrush(Color.FromArgb(Random.Next(0, 255),
                    Random.Next(0, 255), Random.Next(0, 255))), rect);

            image.Save(imageName);
        }
    }
}