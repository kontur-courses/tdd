using System;
using System.Collections.Generic;
using TagsCloudVisualization;
using System.Drawing;

namespace TagsCloudVisualizationPerformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var sameSizeRectangles = LayoutSameSizeRectangles(50, new Size(200, 100));
            var picture = TagsCloudVisualizer.GetPictureOfRectangles(sameSizeRectangles);
            picture.Save("cloud1.png");

            var randomSizeRectangles = LayoutRandomSizeRectangles(50, new Size(20, 50), new Size(200, 300));
            var secondPicture = TagsCloudVisualizer.GetPictureOfRectangles(randomSizeRectangles);
            secondPicture.Save("cloud2.png");
        }

        private static List<Rectangle> LayoutSameSizeRectangles(int amount, Size size)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var placedRectangles = new List<Rectangle>();
            for (var i = 0; i < amount; i++)
                placedRectangles.Add(layouter.PutNextRectangle(size));

            return placedRectangles;
        }

        private static List<Rectangle> LayoutRandomSizeRectangles(int amount, Size minSize, Size maxSize)
        {
            var layouter = new CircularCloudLayouter(new Point(100, 200));
            var placedRectangles = new List<Rectangle>();
            var rnd = new Random();
            for (var i = 0; i < amount; i++)
            {
                var width = rnd.Next(minSize.Width, maxSize.Width);
                var height = rnd.Next(minSize.Height, maxSize.Height);

                placedRectangles.Add(layouter.PutNextRectangle(new Size(width, height)));
            }

            return placedRectangles;
        }
    }
}
