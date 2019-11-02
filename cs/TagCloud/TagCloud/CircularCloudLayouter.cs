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
        
        // increase values
        private static double roEpsilon = 0.2;
        private static int phiRounds = 18;
        private static readonly double PhiEpsilon = Math.PI / phiRounds;

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
            var res = new Rectangle(new Point(nonShiftedRect.X + _center.X, nonShiftedRect.Y + _center.Y), nonShiftedRect.Size);
            _rectangleMap.Add(res);
            return res;
        }

        public static Point FromPolar(double phi, double ro)
        {
            return new Point(
                (int)Math.Round(ro * Math.Cos(phi)), 
                (int)Math.Round(ro * Math.Sin(phi)));
        }

        private Rectangle CreateRectangleOnUnrolledSpirals(Size rectangleSize)
        {
            var shift = new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            while (true) {
                for (var i = 0; i < phiRounds; ++i)
                {
                    // current spirals end locate to rectangle center
                    var center = FromPolar(phi, ro);
                    var leftTop = new Point(center.X + shift.X, center.Y + shift.Y);
                    var rect = new Rectangle(leftTop, rectangleSize);
                    if (IsNonIntersects(rect))
                        return rect;
                    phi += PhiEpsilon;
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