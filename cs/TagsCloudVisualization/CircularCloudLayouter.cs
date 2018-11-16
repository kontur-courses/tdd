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
        private readonly List<Rectangle> rectangles;
        public IReadOnlyList<Rectangle> Rectangles
        {
            get { return rectangles.AsReadOnly(); }
        }

        private void AddRectangle(Rectangle newRectangle)
        {
            rectangles.Add(newRectangle);
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
            return Rectangles.Any((rectangle) => rectangle.IntersectsWith(newRectangle));
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
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
