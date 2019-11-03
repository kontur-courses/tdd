using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    class RoundSpiralPositionGenerator
    {
        private Point center;
        private double angel = 0;
        public readonly double RadiusBetweenTurns = 10;
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
            angel += Delta;
            var dist = RadiusBetweenTurns * angel / 2 / Math.PI;
            var X = (int)(center.X + (dist * Math.Cos(angel)));
            var Y = (int)(center.Y + (dist * Math.Sin(angel)));
            return new Point(X, Y);
        }
    }
}

