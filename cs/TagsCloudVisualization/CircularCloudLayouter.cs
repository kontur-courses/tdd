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
        private Point center;
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

            var rect = new Rectangle(new Point(center.X + GetX(radius,angle),center.Y+GetY(radius,angle)),rectangleSize);
            while (IsIntersectsOthersRectangles(rect))
            {
                angle += Math.PI / 12;
                radius += 0.25;
                rect = new Rectangle(new Point(center.X + GetX(radius, angle), center.Y + GetY(radius, angle)), rectangleSize);
            }
            rectangles.AddLast(rect);
            return rect;
        }

        private bool IsIntersectsOthersRectangles(Rectangle rectangle)
        {
            foreach (var rect in rectangles)
            {
                if (rect.IntersectsWith(rectangle)) 
                    return true;
            }
            return false;
        }

        private int GetX(double r, double a) => (int)(r * Math.Cos(a));
        private int GetY(double r, double a) => (int)(r * Math.Sin(a));
    }
}
