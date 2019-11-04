using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        // polar coordinates 
        private double phi;
        private double ro;
        
//        // increase values
        private double roEpsilon = 0.2;
//        private int phiRounds = 18;

        private readonly Point _center;
        private readonly List<Rectangle> _rectangleMap;

        public CircularCloudLayouter(Point center)
        {
            _center = center;
            _rectangleMap = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException("rectangleSize is degenerate");
            // finding pos
            var nonShiftedRect = CreateRectangleOnUnrolledSpirals(rectangleSize);
            // remember rectangle
            _rectangleMap.Add(nonShiftedRect);
            // lower ro
            ro /= 2;
            var res = ShiftRectangle(nonShiftedRect, _center);
            return res;
        }

        public IEnumerable<Rectangle> GetAllRectangles()
        {
            return _rectangleMap.Select(rect => ShiftRectangle(rect, _center));
        }

        private static Rectangle ShiftRectangle(Rectangle rect, Point shift)
        {
            return new Rectangle(new Point(rect.X + shift.X, rect.Y + shift.Y), rect.Size);
        }

        private static Point FromPolar(double phi, double ro)
        {
            return new Point(
                (int)Math.Round(ro * Math.Cos(phi)), 
                (int)Math.Round(ro * Math.Sin(phi)));
        }

        private Rectangle CreateRectangleOnUnrolledSpirals(Size rectangleSize)
        {
            var shift = new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            while (true)
            {
                var sectors = Math.Max(Math.Round(ro * 18), 18);
                var phiEpsilon = Math.PI / sectors;
                for (var i = 0; i < 2 * sectors; ++i)
                {
                    // current spirals end locate to rectangle center
                    var center = FromPolar(phi, ro);
                    var leftTop = new Point(center.X + shift.X, center.Y + shift.Y);
                    var rect = new Rectangle(leftTop, rectangleSize);
                    if (IsNonIntersects(rect))
                        return rect;
                    phi += phiEpsilon;
                }
                ro += roEpsilon;
            }
        }

        private bool IsNonIntersects(Rectangle target)
        {
            return _rectangleMap.All(rectangle => !rectangle.IntersectsWith(target));
        }

    }
}