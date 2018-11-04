using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private readonly Point center;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var rectLocation = new Point(center.X,center.Y);
            rectLocation.Offset(-size.Width/2, - size.Height/2);

            return new Rectangle(rectLocation, size);
        }
    }
}
