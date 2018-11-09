using System;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace CloudConstruction
{
    public class RectangleGenerator
    {
        public CircularCloudLayouter Cloud { get; set; }

        public RectangleGenerator(CircularCloudLayouter cloud)
        {
            Cloud = cloud;
        }

        public Rectangle GetNextRectangle(Size size)
        {
            var parameterArchimedesSpiral = CircularCloudLayouter.ParameterArchimedesSpiral;
            var stepAngle = CircularCloudLayouter.StepAngle;
            Point location;
            var rectangle = new Rectangle();
            if (Cloud.Rectangles.Count == 0)
            {
                location = new Point(Cloud.Center.X - size.Width / 2, Cloud.Center.Y - size.Height / 2);
                Cloud.Angle += stepAngle;
                return new Rectangle(location, size);
            }
            while (!CheckLocation(rectangle))
            {
                var distance = parameterArchimedesSpiral * Cloud.Angle;
                location = new Point((int)(Cloud.Center.X + distance * Math.Cos(Cloud.Angle)),
                    (int)(Cloud.Center.Y - distance * Math.Sin(Cloud.Angle)));
                rectangle = new Rectangle(location,size);
                Cloud.Angle += stepAngle;
            }
            return rectangle;
        }
        private bool CheckLocation(Rectangle rec)
        {
            if (rec == new Rectangle())
                return false;
            return Cloud.Rectangles.All(rec1 => rec1.Y >= rec.Y + rec.Height || rec.Y >= rec1.Y + rec1.Height ||
                                          rec.X >= rec1.X + rec1.Width || rec1.X >= rec.X + rec.Width);
        }
    }
}