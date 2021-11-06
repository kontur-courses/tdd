using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ILayouter<Rectangle>
    {
        // private Point center;
        private readonly IEnumerator<Point> pointSpiral;
        private readonly HashSet<Rectangle> issuedRectangles;

        public CircularCloudLayouter(Point center)
        {
            issuedRectangles = new HashSet<Rectangle>();
            
            // this.center = center;
            pointSpiral = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(center)
                // .WithDegreesParameter(20)
                // .WithDensityParameter(15)
                .WithDegreesParameter(1)
                .WithDensityParameter(1)
                .Build()
                .GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
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