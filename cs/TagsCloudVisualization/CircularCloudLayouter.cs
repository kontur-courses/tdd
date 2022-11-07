using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; }
        private readonly List<Rectangle> rectangles;
        private readonly Spiral spiral;
        private const int NumberOfPoints = 10_000;

        public CircularCloudLayouter(Point center, Spiral spiral)
        {
            if (spiral.Equals(null))
                throw new NullReferenceException();

            Center = center;
            rectangles = new List<Rectangle>();
            this.spiral = spiral;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException();

            var i = 0;
            var points = spiral.GetPoints(NumberOfPoints);
            var point = points[i];

            while (!RectangleCanBePlaced(point, rectangleSize))
            {
                point = points[++i];
            }

            point = points[i];
            var rectangle = new Rectangle(point, rectangleSize);
            rectangles.Add(rectangle);

            return rectangle;
        }

        public List<Rectangle> GetRectangles(List<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                rectangles.Add(PutNextRectangle(size));

            return rectangles;
        }

        public bool RectangleCanBePlaced(Point position, Size rectangleSize)
        {
            return !rectangles.Any(rectangle =>
                rectangle.IntersectsWith(new Rectangle(position, rectangleSize)));
        }
    }
}