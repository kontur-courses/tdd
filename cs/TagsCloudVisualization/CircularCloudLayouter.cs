using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Point center;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return new Rectangle(
                new Point(
                    center.X - rectangleSize.Width / 2, 
                    center.Y - rectangleSize.Height / 2), 
                rectangleSize);
        }
    }
}
