using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ILayouter<Rectangle>
    {
        private readonly IEnumerator<Point> pointSpiral;
        private readonly HashSet<Rectangle> issuedRectangles;

        public CircularCloudLayouter(Point center)
        {
            issuedRectangles = new HashSet<Rectangle>();
            
            pointSpiral = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(center)
                .WithDegreesParameter(5)
                .WithDensityParameter(5)
                .Build()
                .GetEnumerator();
        }
        
        public CircularCloudLayouter(IEnumerable<Point> spotPoints)
        {
            if (spotPoints is null)
                throw new ArgumentException("spotPoints should be not null");
            
            issuedRectangles = new HashSet<Rectangle>();
            pointSpiral = spotPoints.GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and Height should be positive");
            
            Rectangle rectangle;
            
            do
            {
                pointSpiral.MoveNext();
                var point = pointSpiral.Current;
                rectangle = new Rectangle(
                    point.X - rectangleSize.Width / 2, 
                    point.Y - rectangleSize.Height / 2,
                    rectangleSize.Width,
                    rectangleSize.Height);
            } while (issuedRectangles.Any(x => x.IntersectsWith(rectangle)));

            issuedRectangles.Add(rectangle);
            return rectangle;
        }
    }
}