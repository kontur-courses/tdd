using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private LinkedList<Rectangle> rectangles;
        private double angle = 0;
        private double radius = 0;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new LinkedList<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException();

            var rect = GetAddedRectangle(rectangleSize, center, angle, radius);
            while (IsIntersectsOthersRectangles(rect,rectangles))
            {
                angle += Math.PI / 12;
                radius += 0.25;
                rect = GetAddedRectangle(rectangleSize, center, angle, radius);
            }
            rectangles.AddLast(rect);
            return rect;
        }

        public bool IsIntersectsOthersRectangles(Rectangle rectangle,IEnumerable<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException();
            foreach (var rect in rectangles)
            {
                if (rect.IntersectsWith(rectangle)) 
                    return true;
            }
            return false;
        }

        private Rectangle GetAddedRectangle(Size rectangleSize, Point centerPoint, double angle, double radius)
        {
            var location = new Point(centerPoint.X + GetX(radius, angle), centerPoint.Y + GetY(radius, angle));
            return new Rectangle(location, rectangleSize);
        }
        private int GetX(double r, double a) => (int)(r * Math.Cos(a));
        private int GetY(double r, double a) => (int)(r * Math.Sin(a));
    }
}
