using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public Size CanvasSize { get => GetCanvasSize(); }
        private List<Rectangle> Rectangles;
        private ArchimedeanSpiral _spiral;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            _spiral = new ArchimedeanSpiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException($"Отрицательный размер прямоугольника: " +
                    $"{rectangleSize.Width} x {rectangleSize.Height}");

            var rectangle = Rectangles.Any()
                ? GetNextRectangleWithLocation(rectangleSize)
                : new Rectangle(Center.GetRectangleLocationByCenter(rectangleSize), rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangleWithLocation(Size rectSize)
        {
            var dryRect = new Rectangle(Point.Empty, rectSize);
            var pointEnumerator = _spiral.GetDiscretePoints().GetEnumerator();
            while (pointEnumerator.MoveNext())
            {
                dryRect.Location = pointEnumerator.Current;
                if (!CheckIntersects(dryRect))             
                    break;                
            }
            return dryRect;
        }

        private bool CheckIntersects(Rectangle rectangle) => Rectangles.Any(r => r.IntersectsWith(rectangle));

        private Rectangle UnionAll()
        {
            var union = Rectangles.First();
            foreach (var r in Rectangles)
                union = Rectangle.Union(union, r);
            return union;
        }

        private Size GetCanvasSize()
        {
            var union = UnionAll();
            var distances = union.GetDistancesToPoint(Center);
            var horizontalIncrement = Math.Abs(distances[0] - distances[2]);
            var verticalIncrement = Math.Abs(distances[1] - distances[3]);
            union.Inflate(horizontalIncrement, verticalIncrement);
            return union.Size;
        }

        public IEnumerable<Rectangle> GetLaidRectangles()
        {
            foreach (var r in Rectangles)
                yield return r;
        }
    }
}
