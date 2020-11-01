using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point _center;
        public CircularCloudLayouter(Point center)
        {
            _center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var startRectangle = new Point(_center.X - rectangleSize.Width / 2, _center.Y - rectangleSize.Height / 2);
            return new Rectangle(startRectangle, rectangleSize);
        }
    }
}
