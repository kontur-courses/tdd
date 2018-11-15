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
        private IReadOnlyList<Rectangle> rectangles;
        public List<Rectangle> Rectangles
        {
            get { return rectangles.ToList(); }
        }

        private void AddRectangle(Rectangle newRectangle)
        {
            var list = rectangles.ToList();
            list.Add(newRectangle);
            rectangles = list.AsReadOnly();
        }

        public CircularCloudLayouter(Point center, double spiraleStep)
        {
            this.center = center;
            pointMaker = ArchimedesSpiralePointsMaker
                .GenerateNextPoint(center, spiraleStep)
                .GetEnumerator();
            rectangles = new List<Rectangle>().AsReadOnly();
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
