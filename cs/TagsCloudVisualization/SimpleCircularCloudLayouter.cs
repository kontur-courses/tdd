using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class SimpleCircularCloudLayouter : ILayouter
    {
        private Point center;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private IntegerSpiral spiral;
        private IEnumerator<Point> spiralEnumerator;

        public SimpleCircularCloudLayouter(Point center)
        {
            this.center = center;
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
                var rectangle = new Rectangle(potencialCenter - rectangleSize / 2, rectangleSize);
                if (RectangleIsCorrect(rectangle))
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                }
            } while (spiralEnumerator.MoveNext());

            //Какую ошибку принято выкидывать, если был достигнут недостигаемый код?
            throw new Exception("Circular Cloud Layouter worked incorrect");
        }

        private bool RectangleIsCorrect(Rectangle rectangle)
        {
            return rectangles.All(x => !x.IntersectsWith(rectangle));
        }
    }
}