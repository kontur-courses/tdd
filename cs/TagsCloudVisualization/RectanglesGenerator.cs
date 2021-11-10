using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class RectanglesGenerator
    {
        internal static Rectangle[] GenerateSameRectanglesWithSize(int count, int minSize, int maxSize)
        {
            if (count <= 0 || minSize <= 0 || maxSize <= 0) throw new ArgumentException("all parameters should be positive");
            var layouter = new CircularCloudLayouter(new Point());
            var rnd = new Random();
            var size = new Size(rnd.Next(minSize, maxSize), rnd.Next(minSize, maxSize));
            for (var i = 0; i < count; i++) layouter.PutNextRectangle(size);
            return layouter.Rectangles.ToArray();
        }

        internal static Rectangle[] GenerateDifferentRectangles(int count, int minSize, int maxSize)
        {
            if (count <= 0 || minSize <= 0 || maxSize <= 0) throw new ArgumentException("all parameters should be positive");
            var layouter = new CircularCloudLayouter(new Point());
            var rnd = new Random();
            for (var i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(minSize, maxSize), rnd.Next(minSize, maxSize));
                layouter.PutNextRectangle(size);
            }
            return layouter.Rectangles.ToArray();
        }

    }
}