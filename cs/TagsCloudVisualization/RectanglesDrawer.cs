using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class RectanglesDrawer
    {
        private readonly List<Rectangle> rectangles;

        public RectanglesDrawer(List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
        }

        public void GenerateImage(string imageName)
        {
            var size = rectangles.Sum(rectangle => Math.Max(rectangle.Height, rectangle.Width)) + 10;
            var image = new Bitmap(size, size);
            var graphics = Graphics.FromImage(image);
            graphics.TranslateTransform(size / 2, size / 2);
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(Pens.Black, rectangle);
            image.Save(imageName);
        }
    }
}