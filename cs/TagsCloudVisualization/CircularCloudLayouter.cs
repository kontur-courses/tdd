using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ILayouter
    {
        private List<Rectangle> puttedRectangles = new List<Rectangle>();
        private readonly ExpandingSquare square;
        private readonly IEnumerator<Point> squareEnumerator;

        public CircularCloudLayouter(Point center)
        {
            this.square = new ExpandingSquare(center);
            this.squareEnumerator = square.GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (squareEnumerator.MoveNext())
            {
                var potentialCenter = squareEnumerator.Current;
                var newRectangle = new Rectangle(potentialCenter - rectangleSize / 2, rectangleSize);
                if (puttedRectangles.All(x => !x.IntersectsWith(newRectangle)))
                {
                    puttedRectangles.Add(newRectangle);
                    return newRectangle;
                }
            }

            throw new Exception("Circular Cloud Layouter worked incorrect");
        }
    }
}