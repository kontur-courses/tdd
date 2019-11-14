using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        readonly public Point cloudCenter;
        private List<Rectangle> RectanglesList { get; } = new List<Rectangle>();
        private const double spiralCoefficientK = 1;
        private double spiralAngle = 0;
        private const double angleDelta = 0.01;


        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Sizes of rectangle must be positive");
            var rectangle = Rectangle.Empty;
            int possibleX = 0;
            int possibleY = 0;
            do
            {
                possibleX = (int)(spiralCoefficientK * spiralAngle * Math.Cos(spiralAngle) + cloudCenter.X -
                                  rectangleSize.Width / 2.0);
                possibleY = (int)(spiralCoefficientK * spiralAngle * Math.Sin(spiralAngle) + cloudCenter.Y -
                                  rectangleSize.Height / 2.0);
                rectangle = new Rectangle(new Point(possibleX, possibleY), rectangleSize);
                spiralAngle += angleDelta;
            } while (RectanglesList.Any(r => r.IntersectsWith(rectangle)));
            RectanglesList.Add(rectangle);
            return rectangle;
        }
    }
}