using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public sealed class CircularCloudLayouter
    {
        private readonly ICollection<Rectangle> rectangles;
        private readonly RectanglePlacer rectanglePlacer;
        
        public IReadOnlyCollection<Rectangle> Rectangles => (IReadOnlyCollection<Rectangle>)rectangles;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("the coordinates of the center must be positive numbers");
            rectanglePlacer = new RectanglePlacer(center);
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0  || rectangleSize.Height <= 0)
                throw new ArgumentException("the width and height of the rectangle must be positive numbers");
            
            var nextRengtanle = rectanglePlacer.GetPossibleNextRectangle((IReadOnlyCollection<Rectangle>)rectangles, rectangleSize);
            rectangles.Add(nextRengtanle);
            return nextRengtanle;
        }

    }
}
