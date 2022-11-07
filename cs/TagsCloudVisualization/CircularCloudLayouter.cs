using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point _center;
        private List<Rectangle> _rectanles;
        
        public CircularCloudLayouter(Point center)
        {
            _center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize == Size.Empty)
                return Rectangle.Empty;
            return new Rectangle(_center, rectangleSize);
        }
    }
}