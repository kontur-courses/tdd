using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public const double StepAngle = Math.PI / 36;
        public const double ParameterArchimedesSpiral = 10 / (2 * Math.PI);
        public Size WindowSize { get; set; }
        public Point Center { get; set; }
        public double Angle { get; set; }
        public List<Rectangle> ListRectangles { get; set; }

        public CircularCloudLayouter(Point center)
        {
            WindowSize = new Size(2000, 2000);
            if (center.X < 0 || center.Y < 0 || center.X > WindowSize.Width || center.Y > WindowSize.Height)
                throw new ArgumentException("Center coordinates must not exceed the window size");
            Center = center;
            ListRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size size)
        {
            Rectangle resultRect = new Rectangle();
            if (ListRectangles.Count == 0)
            {
                var location = new Point(Center.X - size.Width / 2, Center.Y - size.Height / 2);
                resultRect = new Rectangle(location,size);
                ListRectangles.Add(resultRect);
                Angle += StepAngle;
                return resultRect;
            }
            while (!CheckLocation(resultRect))
            {

                var distance = ParameterArchimedesSpiral * Angle;
                var location = new Point((int)(Center.X + distance * Math.Cos(Angle)),
                    (int)(Center.Y - distance * Math.Sin(Angle)));
                Angle += StepAngle;
                resultRect = new Rectangle(location, size);
            }

            var cloudConstrictor = new СloudСonstrictor(this);
            resultRect = cloudConstrictor.ShiftRectangleToTheNearest(resultRect);
            ListRectangles.Add(resultRect);
            return resultRect;
        }


        private bool CheckLocation(Rectangle rec)
        {
            if (rec == new Rectangle())
                return false;
            return ListRectangles.All(rec1 => rec1.Y >= rec.Y + rec.Height || rec.Y >= rec1.Y + rec1.Height ||
                                               rec.X >= rec1.X + rec1.Width || rec1.X >= rec.X + rec.Width);
        }
    }
}
