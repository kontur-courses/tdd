using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class RandomLayoutGenerator
    {
        public static Random rnd = new Random();

        public static List<Rectangle> GenerateRandomLayout(Point center, int amount,
            int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var sizes = new List<Size>();
            while (amount-- > 0)
                sizes.Add(GetRandomSize(minWidth, maxWidth, minHeight, maxHeight));
            return LayoutRectangles(center, sizes);
        }

        public static List<Rectangle> LayoutRectangles(Point center, List<Size> sizes)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            foreach (var size in sizes)
                rectangles.Add(layouter.PutNextRectangle(size));

            return rectangles;
        }

        public static Size GetRandomSize(int minWidth, int maxWidth,
                                  int minHeight, int maxHeight)
        {
            return new Size(rnd.Next(minWidth, maxWidth + 1),
                                rnd.Next(minHeight, maxHeight + 1));
        }
    }
}
