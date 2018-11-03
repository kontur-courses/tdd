using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var drawer = new RectanglesDrawer(GenerateRectanglesLayout(100, -1, 2, 1));
            drawer.GenerateImage("100BigToSmallRectangles.png");
            drawer = new RectanglesDrawer(GenerateRectanglesLayout(100, 1, 2, 1));
            drawer.GenerateImage("100SmallToBigRectangles.png");
            drawer = new RectanglesDrawer(GenerateRectanglesLayout(10, -1, 1, 1));
            drawer.GenerateImage("10BigToSmallSquares.png");
            drawer = new RectanglesDrawer(GenerateRectanglesLayout(10, -1, 1, 2));
            drawer.GenerateImage("10BigToSmallHighRectangles.png");
        }

        private static List<Rectangle> GenerateRectanglesLayout(int rectanglesAmount, int step,
            int widthMultiplier, int heightMultiplier)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0), new SpiralPointsGenerator(1));
            if (step > 0)
            {
                for (var size = 1; size <= rectanglesAmount; size += step)
                    AddRectangle(layouter, size, widthMultiplier, heightMultiplier);
            }
            else
            {
                for (var size = rectanglesAmount; size > 0; size += step)
                    AddRectangle(layouter, size, widthMultiplier, heightMultiplier);
            }
            return layouter.Rectangles;
        }

        private static void AddRectangle(CircularCloudLayouter layouter, int size, int widthMultiplier, int heightMultiplier)
        {
            var width = size * widthMultiplier;
            var height = size * heightMultiplier;
            layouter.PutNextRectangle(new Size(width, height));
        }
    }
}