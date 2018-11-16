using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }
            
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle dimensions must be positive");

            var rectangle = new Rectangle(Center, rectangleSize);
            var spiralStrategy = new SpiralStrategy(Rectangles, Center);
            var centerMoveStrategy = new CenterMoveStrategy(Rectangles, Center);

            rectangle = spiralStrategy.PlaceRectangle(rectangle);
            rectangle = centerMoveStrategy.PlaceRectangle(rectangle);
            Rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
