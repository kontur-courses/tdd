using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point center;
        private IEnumerator<Point> pointMaker;
        public List<Rectangle> rectangles;
         
        public CircularCloudLayouter(Point center, double spiraleStep)
        {
            this.center = center;
            pointMaker = ArchimedesSpiralePointsMaker
                .PointsMaker(center, spiraleStep)
                .GetEnumerator();
            rectangles = new List<Rectangle>();
        }

        private bool AreRectanglesIntersectWith(Rectangle newRectangle)
        {
            foreach (var rectangle in rectangles)
            {
                if (rectangle.IntersectsWith(newRectangle))
                    return true;
            }
            return false;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            pointMaker.MoveNext();
            var location = pointMaker.Current;
            var newRectangle = new Rectangle(location, rectangleSize);
            while (AreRectanglesIntersectWith(newRectangle))
            {
                location = pointMaker.Current;
                pointMaker.MoveNext();
                newRectangle = new Rectangle(location, rectangleSize);
            }
            rectangles.Add(newRectangle);
            return newRectangle;
        }
    }
}
