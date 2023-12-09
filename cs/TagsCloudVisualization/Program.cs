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
            var drawer = new DrawCloud(GenerateRandomRectangles(layouter, 50), 1500, 1500);
            drawer.CreateImage("RandomRectangles50.png");
           

            List<Rectangle> GenerateRandomRectangles(CircularCloudLayouter layouter, int count)
            {
                var rectangles = new List<Rectangle>();
                var random = new Random(1);
                for (var i = 0; i < count; i++)
                {
                    var size = new Size(random.Next(40,100), random.Next(20, 60));
                    rectangles.Add(layouter.PutNextRectangle(size));
                }
                return rectangles;
            }
        }
    }
}