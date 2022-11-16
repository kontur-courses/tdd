using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var center = new Point(200, 200);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = GenerateRectangles(50, layouter);
            
            RectangleVisualisator visualisator = new RectangleVisualisator(rectangles, center);
            visualisator.Paint();
            visualisator.Save("Rectangles.png");
        }

        public static List<Rectangle> GenerateRectangles(int amount, CircularCloudLayouter layouter)
        {
            if (amount <= 0)
                throw new ArgumentException();

            var random = new Random(1);
            List<Rectangle> rectangles = new List<Rectangle>(); 
            for (int i = 0; i < amount; i++)
            {
                var size = new Size(random.Next() % 255 + 1, random.Next() % 255 + 1);
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            return rectangles;
        }
    }
}