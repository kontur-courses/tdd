using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class СoncentricCircles: ISpiralLayouter
    {
        private double ro;
        private double phi;

        private double deltaRo;

        public СoncentricCircles(double deltaRo = 0.2)
        {
            this.deltaRo = deltaRo;
        }

        public Rectangle PutNextRectangle(Size rectangleSize, Predicate<Rectangle> isIntersect)
        {
            var centerShift = new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            while (true)
            {
                var sectors = Math.Max(Math.Round(ro * 18), 18);
                var deltaPhi = Math.PI / sectors;
                for (var i = 0; i < 2 * sectors; ++i)
                {
                    var rect = GetCenteredRectangleFromPolar(rectangleSize, centerShift);
                    if (!isIntersect(rect))
                    {
                        ro /= 2;
                        return rect;
                    }

                    phi += deltaPhi;
                }
                ro += deltaRo;
            }
        }


        private Rectangle GetCenteredRectangleFromPolar(Size rectSize, Point centerShift)
        {
            var cartesianCoord = FromPolar(phi, ro);
            return new Rectangle(new Point(cartesianCoord.X + centerShift.X, cartesianCoord.Y + centerShift.Y), rectSize);
        }
        
        private static Point FromPolar(double phi, double ro)
        {
            return new Point(
                (int)Math.Round(ro * Math.Cos(phi)), 
                (int)Math.Round(ro * Math.Sin(phi)));
        }
    }
}