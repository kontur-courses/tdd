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
        private readonly IEnumerator<Point> pointMaker;
        private List<Rectangle> Rectangles;
        public List<Rectangle> rectangles
        {
            get { return Rectangles.ToList(); }
            private set { Rectangles = value; }
        }

        private void AddRectangle(Rectangle newRectangle)
        {
            Rectangles.Add(newRectangle);
        }

        public CircularCloudLayouter(Point center, double spiraleStep)
        {
            this.center = center;
            pointMaker = ArchimedesSpiralePointsMaker
                .GenerateNextPoint(center, spiraleStep)
                .GetEnumerator();
            rectangles = new List<Rectangle>();
        }

        private bool AreRectanglesIntersectWith(Rectangle newRectangle)
        {
            return rectangles.Any((rectangle) => rectangle.IntersectsWith(newRectangle));
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                pointMaker.MoveNext();
                var location = pointMaker.Current;
                var newRectangle = new Rectangle(location, rectangleSize);
                if (!AreRectanglesIntersectWith(newRectangle))
                {
                    AddRectangle(newRectangle);
                    return newRectangle;
                }
            }
        }
    }
}
