using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private Point center;
        private double radius;
        private double angle;
        private double deltaRadius;
        private double deltaAngle;

        public Spiral(Point center, double deltaRadius = 0.1, double deltaAngle = 0.1) 
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("the coordinates of the center must be positive numbers");
            this.center = center;
            this.deltaRadius = deltaRadius;
            this.deltaAngle = deltaAngle;
            radius = 0;
            angle = 0;
        }

        public Rectangle GetPossibleNextRectangle(List<Rectangle> cloudRectangles, Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("the width and height of the rectangle must be positive numbers");

            while (true)
            {
                Point point = new Point(
                    (int)(center.X + radius * Math.Cos(angle)),
                    (int)(center.Y + radius * Math.Sin(angle))
                    );
                Rectangle possibleRectangle = new Rectangle(point, rectangleSize);

                if (!cloudRectangles.Any(rectangle => rectangle.IntersectsWith(possibleRectangle)))
                {
                    return possibleRectangle;
                }

                angle += deltaAngle;
                radius += deltaRadius;
            }
        }

    }
}
