using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    { 
        public readonly List<Rectangle> Rectangles;
        public readonly Point CloudCenter;

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            CloudCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckRectangleSizeCorrectness(rectangleSize);
            return new Rectangle();
        }

        private void CheckRectangleSizeCorrectness(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
        }
    }
}
