using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral : ISpiral
    {
        public ArchimedeanSpiral(Point center,double distanceBetweenTurns, double angleStep)
        {
            AngleStep = angleStep;
            DistanceBetweenTurns = distanceBetweenTurns;
            Center = center;
        }

        public Point Center { get; }
        public double AngleStep { get; }
        public double DistanceBetweenTurns { get; }
        public double Angle { get; private set; }

        public Point GetNextPosition()
        {
            var newPoint = new Point
                (
                    Center.X+(int)(DistanceBetweenTurns / (2 * Math.PI) *
                    ConvertFromDegreesToRadians(Angle) * Math.Cos(ConvertFromDegreesToRadians(Angle))),

                    Center.Y+(int)(DistanceBetweenTurns / (2 * Math.PI) * 
                    ConvertFromDegreesToRadians(Angle) * Math.Sin(ConvertFromDegreesToRadians(Angle)))
                );

            Angle += AngleStep;

            return newPoint;
        }

        private double ConvertFromDegreesToRadians(double degrees)
        {
            return Math.PI / (180 / degrees);
        }

    }
}
