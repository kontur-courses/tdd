using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ILayouter
    {
        private List<Rectangle> puttedRectangles = new List<Rectangle>();
        private readonly IntegerSpiral spiral;
        private readonly IEnumerator<Point> spiralEnumerator;

        public CircularCloudLayouter(Point center)
        {
            this.spiral = new IntegerSpiral(center);
            this.spiralEnumerator = spiral.GetEnumerator();
            spiralEnumerator.MoveNext();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Point potencialCenter;
            do
            {
                potencialCenter = spiralEnumerator.Current;
                var newRectangle = new Rectangle(potencialCenter - rectangleSize / 2, rectangleSize);
                if (puttedRectangles.All(x => !x.IntersectsWith(newRectangle)))
                {
                    puttedRectangles.Add(newRectangle);
                    return newRectangle;
                }
            } while (spiralEnumerator.MoveNext());

            //Какую ошибку принято выкидывать, если был достигнут недостигаемый код?
            throw new Exception("Circular Cloud Layouter worked incorrect");
        }
    }
}