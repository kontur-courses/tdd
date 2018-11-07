using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly IEnumerator<Point> spiralGenerator;

        public CircularCloudLayouter(Point center)
        {
            spiralGenerator = new RoundSpiralGenerator(center, 3.6).GetEnumerator();
            spiralGenerator.MoveNext();
        }

        public IEnumerable<Rectangle> Rectangles => rectangles.AsEnumerable();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("size has non positive parts");

            var nextPosition = spiralGenerator.Current;
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (rectangles.Exists(rect => rectangle.IntersectsWith(rect)))
            {
                //var step = (rectangleSize.Height + rectangleSize.Width) / 20; TODO: remove this or rework
                for (var i = 0; i < 1; i++)
                    spiralGenerator.MoveNext();
                nextPosition = spiralGenerator.Current;
                rectangle = new Rectangle(nextPosition, rectangleSize);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        public void PutNextRectangles(IEnumerable<Size> rectangles)
        {
            foreach (var rectangle in rectangles) PutNextRectangle(rectangle);
        }
    }
}
