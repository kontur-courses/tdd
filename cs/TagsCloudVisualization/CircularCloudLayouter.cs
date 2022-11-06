using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0) throw new ArgumentException();
            this.center = center;
        }

        Rectangle PutNextRectangle(Size rectangleSize)
        {
            return new Rectangle();
        }
    }
}
