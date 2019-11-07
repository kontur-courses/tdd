using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main(string[] args)
        {
            var imageSize = new Size(1080, 1080);
            var sizes = Generator.GetRandomSizesList(5, 50, 5, 50, 1000, new Random());
            var center = new Point(imageSize.Width / 2, imageSize.Height / 2);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = layouter.PutNextRectangles(sizes);
            var visualisator = new RectanglesVisualisator(imageSize, rectangles);
            visualisator.DrawRectangles();
            visualisator.Save("image.png");
        }
    }
}
