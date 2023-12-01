using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class SpiralGenerator : IEnumerable<Point>
    {
        private readonly Point center;
        public readonly int RadiusLambda;
        public readonly double AngleLambda;

        public Point Center => new(center.X, center.Y);
        public int Radius { get; private set; }
        public double Angle { get; private set; }

        public SpiralGenerator(int radiusLambda = 0, double angleLambda = Math.PI / 60)
        {
            this.center = new Point(0, 0);
            this.RadiusLambda = radiusLambda < 0 ? throw new ArgumentException("radiusLambda cant be negative") : radiusLambda;
            this.AngleLambda = angleLambda;
        }
        public SpiralGenerator(Point center, int radiusLambda = 0, double angleLambda = Math.PI / 60)
        {
            this.center = center;
            this.RadiusLambda = radiusLambda < 0 ? throw new ArgumentException("radiusLambda cant be negative") : radiusLambda;
            this.AngleLambda = angleLambda;
        }

        public Point GetNextPoint()
        {
            var x = (int)Math.Round(center.X + Radius * Math.Cos(Angle));
            var y = (int)Math.Round(center.Y + Radius * Math.Sin(Angle));

            var nextAngle = Angle + AngleLambda;
            var angleMoreThan2Pi = Math.Abs(nextAngle) >= Math.PI * 2;

            Radius = angleMoreThan2Pi ? Radius + RadiusLambda : Radius;
            Angle = angleMoreThan2Pi ? 0 : nextAngle;

            return new Point(x, y);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
