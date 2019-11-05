using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    class RoundSpiralPositionGenerator
    {
        private Point center;
        private double angle = 0;
        public readonly double DeltaRadiusBetweenTurns = 10;
        public readonly double Delta = 0.5;

        public RoundSpiralPositionGenerator(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException();
            }
            this.center = center;
        }

        public Point Next()
        {
            angle += Delta;
            var dist = DeltaRadiusBetweenTurns * angle / 2 / Math.PI;
            var X = (int)(center.X + (dist * Math.Cos(angle)));
            var Y = (int)(center.Y + (dist * Math.Sin(angle)));
            return new Point(X, Y);
        }
    }
}
