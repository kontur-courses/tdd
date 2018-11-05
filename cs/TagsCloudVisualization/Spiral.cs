using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private CircularCloudLayouter layouter;
        private int currentSpiralAngle;
        private readonly Point center = new Point(0, 0);

        public Spiral(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        private void IncreaseSpiralAngle()
        {
            currentSpiralAngle++;
        }

        private Point GenerateRectangleLocation()
        {
            //For generating rectangle location (left-upper corner)
            //I'm using Archimedean Spiral

            var distanceBetweenTurnings = 1;
            var radius = distanceBetweenTurnings * currentSpiralAngle;

            var x = (int)(radius * Math.Cos(currentSpiralAngle));
            var y = (int)(radius * Math.Sin(currentSpiralAngle));

            return new Point(x,y);
        }

        public Rectangle GenerateNewRectangle(List<Rectangle> rectangles, Size rectangleSize)
        {
            Rectangle rectangle;
            if (rectangles.Count == 0)
                return new Rectangle(center.ShiftToLeftRectangleCorner(rectangleSize), rectangleSize);
            while (true)
            {
                var rectangleCenterPointLocation = GenerateRectangleLocation();
                var rectangleLocation = rectangleCenterPointLocation.ShiftToLeftRectangleCorner(rectangleSize);
                rectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (RectanglesDoNotIntersect(rectangles, rectangle))
                    break;
                IncreaseSpiralAngle();
            }
            return rectangle;
        }


        private bool RectanglesDoNotIntersect(List<Rectangle> rectangles, Rectangle newRectangle)
        {
            return !(rectangles.Any(newRectangle.IntersectsWith));
        }


    }
}