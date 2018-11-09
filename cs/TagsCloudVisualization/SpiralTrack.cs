using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralTrack
    {
        private Point center;
        private int angle;
        private readonly double step;
        private Point? lastPoint;

        public SpiralTrack(Point center, double step)
        {
            this.center = center;
            this.step = step;
        }

        public Point GetNextPoint()
        {
            var nextPoint = lastPoint;

            if (lastPoint == null)
            {
                lastPoint = center;
                return center;
            }

            while (nextPoint == lastPoint)
            {
                var vectorLength = angle * (step / (2 * Math.PI));
                var vector = new Point(
                    (int)Math.Ceiling(vectorLength * Math.Cos(angle)),
                    (int)Math.Ceiling(vectorLength * Math.Sin(angle))
                );

                nextPoint = new Point(center.X + vector.X, center.Y + vector.Y);
                angle += 1;
            }

            return (Point) nextPoint;
        }
    }
}
