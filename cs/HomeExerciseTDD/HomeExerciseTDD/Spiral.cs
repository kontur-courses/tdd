using System;
using System.Collections.Generic;
using System.Drawing;

namespace HomeExerciseTDD
{
    public class Spiral
    {
        private Point center;
        private const float step = 0.0005f;
        private Point previousPoint;
        private float previousAngle;

        public Spiral(Point center)
        {
            previousPoint = center;
            this.center = center;
        }

        public Point GetNextPoint()
        {
            while (true)
            {
                if (previousPoint == center)
                {
                    previousPoint = new Point(Size.Empty);
                    return center;
                }
                var angle = previousAngle;
                angle++;

                var distance = angle * step;
                var currentX = distance * (float) Math.Cos(angle) + center.X;
                var currentY = distance * (float) Math.Sin(angle) + center.Y;

                var currentPoint = new Point((int) currentX, (int) currentY);
                previousAngle = angle;
                if (previousPoint.Equals(currentPoint))
                    continue;
                
                previousPoint = currentPoint;
                return currentPoint;
            }
        }
    }
}