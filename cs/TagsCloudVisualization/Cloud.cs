using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public abstract class Cloud
    {
        public readonly Point Center;
        public IReadOnlyCollection<Rectangle> Rectangles => rectangles;
        protected readonly List<Rectangle> rectangles;

        public Cloud(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
        }

        public abstract Rectangle PutNextRectangle(Size rectangleSize);

        public void PutManyRectangles(int count, Random random,
            int minSize, int maxSize)
        {
            for (var i = 0; i < count; i++)
                PutNextRectangle(new Size(
                        random.Next(minSize, maxSize),
                        random.Next(minSize, maxSize)));
        }
    }
}
