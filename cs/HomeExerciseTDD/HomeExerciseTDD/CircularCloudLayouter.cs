using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace HomeExerciseTDD
{
    public class CircularCloudLayouter:ICircularCloudLayouter
    {
        private Point center;
        private readonly List<Rectangle> rectanglesInCloud = new List<Rectangle>();
        private readonly Spiral spiral;


        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new Spiral(this.center);
        }
        private bool IsIntersect(Rectangle currentRectangle)
        {
            return rectanglesInCloud.Any(currentRectangle.IntersectsWith);
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var rectangleLocation = spiral.GetNextPoint();
                var currentRectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (!IsIntersect(currentRectangle))
                {
                    rectanglesInCloud.Add(currentRectangle);
                    return currentRectangle;
                }
            }
        }
    }
}