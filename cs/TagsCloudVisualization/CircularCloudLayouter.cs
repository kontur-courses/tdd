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

            resultRect = ShiftRectangleToTheNearest(resultRect);
            ListRectangles.Add(resultRect);
            return resultRect;
        }

        private Rectangle ShiftRectangleToTheNearest(Rectangle rectangle)
        {
            if (ListRectangles.Count == 0)
                return rectangle;
            var yLevelRectangles = ListRectangles.Where(rect => !(rectangle.Y >= rect.Y + rect.Height
                                                                  || rectangle.Y + rectangle.Height <= rect.Y)).ToList();
            rectangle = FindNearestRectangleHorizontally(rectangle, yLevelRectangles);
            var xLevelRectangles = ListRectangles.Where(rect => !(rectangle.X >= rect.X + rect.Width
                                                                  || rectangle.X + rectangle.Width <= rect.X)).ToList();
            rectangle = FindNearestRectangleVertically(rectangle, xLevelRectangles);

            return rectangle;
        }

        private Rectangle FindNearestRectangleHorizontally(Rectangle rectangle, List<Rectangle> yLevelRectangles)
        {
            if (yLevelRectangles.Count == 0) return rectangle;
            int distanceToNearestRectangle;
            if (rectangle.X <= Center.X)
            {
                var listCorrectRectangles = yLevelRectangles.Where(rec => rec.X >= rectangle.X + rectangle.Width).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : listCorrectRectangles
                        .Min(rec => Math.Abs(rec.X - (rectangle.X + rectangle.Width)));
            }
            else
            {
                var listCorrectRectangles = yLevelRectangles.Where(rec => rec.X + rec.Width <= rectangle.X).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : -listCorrectRectangles
                        .Min(rec => Math.Abs(rec.X + rec.Width - rectangle.X));
            }
            var newLocation = new Point(rectangle.X + distanceToNearestRectangle, rectangle.Y);
            rectangle = new Rectangle(newLocation, rectangle.Size);

            return rectangle;
        }

        private Rectangle FindNearestRectangleVertically(Rectangle rectangle, List<Rectangle> xLevelRectangles)
        {
            if (xLevelRectangles.Count == 0) return rectangle;
            int distanceToNearestRectangle;
            if (rectangle.Y >= Center.Y)
            {
                var listCorrectRectangles = xLevelRectangles.Where(rec => rec.Y + rec.Height <= rectangle.Y).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : -listCorrectRectangles
                        .Min(rec => Math.Abs(rec.Y + rec.Height - rectangle.Y));
            }
            else
            {
                var listCorrectRectangles =
                    xLevelRectangles.Where(rec => rec.Y >= rectangle.Y + rectangle.Height).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : listCorrectRectangles
                        .Min(rec => Math.Abs(rec.Y - rectangle.Y - rectangle.Height));
            }
            var newLocation = new Point(rectangle.X, rectangle.Y + distanceToNearestRectangle);
            rectangle = new Rectangle(newLocation, rectangle.Size);
            return rectangle;
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
