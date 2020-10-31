using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return new Rectangle(center, rectangleSize);
        }
    }
}