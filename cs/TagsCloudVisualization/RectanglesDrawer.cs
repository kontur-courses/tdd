using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectanglesDrawer
    {
        private const int SideShift = 10;

        public static void GenerateImage(IEnumerable<Rectangle> rectangles, string imageName)
        {
            var rectanglesArray = rectangles as Rectangle[] ?? rectangles.ToArray();
            var size = rectanglesArray.Sum(rectangle => Math.Max(rectangle.Height, rectangle.Width)) + SideShift;
            var image = new Bitmap(size, size);
            var graphics = Graphics.FromImage(image);
            graphics.TranslateTransform(size / 2, size / 2);
            foreach (var rectangle in rectanglesArray)
                graphics.DrawRectangle(Pens.Black, rectangle);
            image.Save(imageName);
        }
    }
}