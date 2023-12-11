using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var cloud = new GenerateRandomRectangles();
            var rectangles = cloud.RectangleGenerator(layouter, 50);
            var drawer = new DrawCloud( 1500, 1500);
            drawer.CreateImage(rectangles, "RandomRectangles50.png");
        }
    }
}