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
            var rectangle = new Rectangle(Point.Empty, rectangleSize);
            Point targetPoint;
            if (Rectangles.Count > 0)
                targetPoint = FindBestPointMinRadius(rectangleSize);
            else
                targetPoint = rectangle.GetLocationFromCenter(Center);
            rectangle = new Rectangle(targetPoint, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Point FindBestPointMinRadius(Size rectSize)
        {
            var pointWithMinRadius = new Point(int.MaxValue, int.MaxValue);
            double minRadius = int.MaxValue;

            var treshold = GetTreshold();
            foreach (var spiralPoint in _spiral.Slide())
            {
                var dryRectangle = new Rectangle(spiralPoint, rectSize);
                if (Center == dryRectangle.GetCenter())
                    continue;
                var radius = new Radius(Center, dryRectangle.GetCenter());

                foreach (var point in radius.LinSpaceFromCenter(treshold))
                {
                    dryRectangle = new Rectangle(dryRectangle.GetLocationFromCenter(point), dryRectangle.Size);
                    if (!CheckIntersects(dryRectangle))
                    {
                        if (Radius.Length(Center, point) < minRadius)
                        {
                            minRadius = Radius.Length(point, Center);
                            pointWithMinRadius = dryRectangle.Location;
                        }
                        break;
                    }
                }
            }
            return pointWithMinRadius;
        }

        private double GetTreshold()
        {
            var sizes = Rectangles.Select(r => (r.Width + r.Height) / 2d).ToList();
            sizes.Sort();
            var meidanSize = sizes[sizes.Count / 2];
            var f = (Math.Sqrt(Rectangles.Count) + 1) / 2 - 1;
            return meidanSize * f;
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
