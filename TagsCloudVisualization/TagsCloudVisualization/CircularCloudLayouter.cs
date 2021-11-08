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
        public List<Rectangle> Rectangles { get; private set; }
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
            var target = Rectangles.Any() ? FindNextPoint(rectangleSize) : Center.GetRectangleLocationByCenter(rectangleSize);

            var rectangle = new Rectangle(target, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Point FindNextPoint(Size rectSize)
        {
            Point? point = null;
            foreach (var spiralPoint in _spiral.Slide())
            {
                if (!CheckIntersects(new Rectangle(spiralPoint, rectSize)))
                {
                    point = spiralPoint;
                    break;
                }
            }
            return point.Value;
        }

        private bool CheckIntersects(Rectangle rectangle)
        {
            foreach (var r in Rectangles)
                if (rectangle.IntersectsWith(r))
                    return true;
            return false;
        }

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
    }
}
