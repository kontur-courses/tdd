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
        public readonly Point Center;
        public readonly Size Size;
        private readonly IEnumerable<Point> _points;
        public List<Rectangle> Rectangles { get; set; }


        public CircularCloudLayouter(Point center, IPointGenerator pointGenerator) 
        {
            if (center.X <=0 || center.Y <=0)
                throw new ArgumentException("Center coordinates values have to be greater than Zero");
            Center = center;
            Size = CountSize(center);
            Rectangles = [];
            _points = pointGenerator.GeneratePoints(Center);
        }


        private Size CountSize(Point center)
        {
            var width = (center.X % 2 == 0) ? center.X * 2 + 1 : Center.X * 2;
            var height = (center.Y % 2 == 0) ? center.Y * 2 + 1 : center.Y * 2;
            return new Size(width, height);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            foreach (var point in _points)
            {
                var supposed = new Rectangle(new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2),
                          rectangleSize);
                if (IntersectsWithAnyOther(supposed, Rectangles))
                    continue;
                Rectangles.Add(supposed);
                return supposed;
            }
            throw new ArgumentException("Not Enough Points Generated");
        }

        public static bool IntersectsWithAnyOther(Rectangle supposed, List<Rectangle> others)
        {
            return others.Any(x => x.IntersectsWith(supposed));
        }
    }
}
