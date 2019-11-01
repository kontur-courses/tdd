using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private static readonly Size _degenerateSize = new Size(0, 0);
        
        // polar coordinates 
        private double phi = 0;
        private double ro = 0;
        
        // increase values
        private static double roEpsilon = 0.2;
        private static int phiRounds = 18;
        private static double phiEpsilon = Math.PI / phiRounds;

        private readonly List<Rectangle> _rectangleMap;
//        private readonly Point _center;

        public CircularCloudLayouter(Point center)
        {
//            _center = center;
            _rectangleMap = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize == _degenerateSize)
                throw new ArgumentException("rectangleSize is degenerate");
            // finding pos
            OrientToPossibleCenter(rectangleSize);
            var res = new Rectangle(FromPolar(phi, ro), rectangleSize);
            _rectangleMap.Add(res);
            return res;
        }

        public static Point FromPolar(double phi, double ro)
        {
            return new Point(
                (int)Math.Round(ro * Math.Cos(phi)), 
                (int)Math.Round(ro * Math.Sin(phi)));
        }

        private void OrientToPossibleCenter(Size rectangleSize)
        {
            while (true) {
                for (var i = 0; i < phiRounds; ++i)
                {
                    var checkRes = IsNonIntersects(new Rectangle(FromPolar(phi, ro), rectangleSize));
                    if (checkRes)
                        return;
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