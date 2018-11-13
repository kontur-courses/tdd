using System;
using System.Drawing;
using TagsCloudVisualization.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geom;

namespace TagsCloudVisualization
{
    public class EntryPoint
    {
        private const int DefaultImageWidth = 1024;
        private const int DefaultImageHeight = 1024;

        private static void Main(string[] args)
        {
            var randomLayouter = LayouterWithRandomSizeRectangles();
            Console.WriteLine("Random generated");
            var simpleLayouter = SimpleLayouter();
            Console.WriteLine("Simple generated");

            new ImageWriter("random.png", DefaultImageWidth, DefaultImageHeight).WriteLayout(randomLayouter);
            new ImageWriter("simple.png", DefaultImageWidth, DefaultImageHeight).WriteLayout(simpleLayouter);
        }

        private static CircularCloudLayouter LayouterWithRandomSizeRectangles()
        {
            var layouter = new CircularCloudLayouter(DefaultImageWidth / 2, DefaultImageHeight / 2, DefaultImageWidth, DefaultImageHeight);
            for (var i = 0; i < 800; i++)
                layouter.PutNextRectangle(new Size().GenerateRandom(3, 50, 3, 50));

            return layouter;
        }

        private static CircularCloudLayouter SimpleLayouter()
        {
            var layouter = new CircularCloudLayouter(DefaultImageWidth / 2, DefaultImageHeight / 2, DefaultImageWidth, DefaultImageHeight);
            for (var i = 0; i < 1000; i++)
                layouter.PutNextRectangle(new Size(20, 10));

            return layouter;
        }
    }
}