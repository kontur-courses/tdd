using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class СoncentricCircles: ISpiral
    {
        private double ro;
        private double phi;

        private double deltaRo;

        public СoncentricCircles(double deltaRo = 0.2)
        {
            this.deltaRo = deltaRo;
        }

        public IEnumerable<Point> IterateBySpiralPoints()
        {
            ro /= 2;
            while (true)
            {
                var sectors = Math.Max(Math.Round(ro * 18), 18);
                var deltaPhi = Math.PI / sectors;
                for (var i = 0; i < 2 * sectors; ++i)
                {
                    yield return FromPolar(phi, ro);
                    phi += deltaPhi;
                }
                ro += deltaRo;
            }
        }
        
        private static Point FromPolar(double phi, double ro)
        {
            return new Point(
                (int)Math.Round(ro * Math.Cos(phi)), 
                (int)Math.Round(ro * Math.Sin(phi)));
        }
    }
}