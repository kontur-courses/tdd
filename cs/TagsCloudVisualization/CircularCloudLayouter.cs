using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public readonly Point Center = new Point(0,0);
        public List<Rectangle> Rectangles;
        public int currentSpiralAngle;

        public CircularCloudLayouter()
        {
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GenerateNewRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private void IncreaseSpiralAngle()
        {
            currentSpiralAngle++;
        }

        private Rectangle GenerateNewRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            if (Rectangles.Count == 0)
                return new Rectangle(Center.ShiftToLeftRectangleCorner(rectangleSize), rectangleSize);
            while (true)
            {
                var rectangleCenterPointLocation = GenerateRectangleLocation();
                var rectangleLocation = rectangleCenterPointLocation.ShiftToLeftRectangleCorner(rectangleSize);
                rectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (RectanglesDoNotIntersect(rectangle))
                    break;
                IncreaseSpiralAngle();
            }
            return rectangle;
        }


        private Point GenerateRectangleLocation()
        {
            //For generating rectangle location (left-upper corner)
            //I'm using Archimedean Spiral

            var distanceBetweenTurnings = 1;
            var radius = distanceBetweenTurnings * currentSpiralAngle;

            var x = Center.X + (int)(radius * Math.Cos(currentSpiralAngle));
            var y = Center.Y + (int)(radius * Math.Sin(currentSpiralAngle));

            return new Point(x,y);
        }

        public bool RectanglesDoNotIntersect(Rectangle newRectangle)
        {
            return Rectangles.All(rect => newRectangle.IntersectsWith(rect) == false);
        }
    }

}
