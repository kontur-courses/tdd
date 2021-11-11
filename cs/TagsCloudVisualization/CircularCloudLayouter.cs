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
            squareEnumerator.MoveNext();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Point potencialCenter;
            do
            {
                potencialCenter = squareEnumerator.Current;
                var newRectangle = new Rectangle(potencialCenter - rectangleSize / 2, rectangleSize);
                if (puttedRectangles.All(x => !x.IntersectsWith(newRectangle)))
                {
                    puttedRectangles.Add(newRectangle);
                    return newRectangle;
                }
            } while (squareEnumerator.MoveNext());

            //Какую ошибку принято выкидывать, если был достигнут недостигаемый код?
            throw new Exception("Circular Cloud Layouter worked incorrect");
        }
    }
}