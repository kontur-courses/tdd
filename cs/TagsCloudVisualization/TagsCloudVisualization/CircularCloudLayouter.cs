using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private int left;
        private int right;
        private int top;
        private int bottom;

        public Rectangle GetRectangle => new Rectangle(left, top, right - left, bottom - top);

        public List<Rectangle> tags = new List<Rectangle>();
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            left = right = center.X;
            top = bottom = center.Y;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = new Point(right, bottom);
            if (left == right && top == bottom)
            {
                location = new Point(Center.X - rectangleSize.Width / 2, Center.Y - rectangleSize.Height / 2);
            }
    
            var newTag = new Rectangle(location, rectangleSize);
            tags.Add(newTag);
            left = (newTag.Left < left) ? newTag.Left : left;
            right = (newTag.Right > right ) ? newTag.Right : right;
            top = (newTag.Top < top) ? newTag.Top : top;
            bottom = (newTag.Bottom > bottom) ? newTag.Bottom : bottom;
            return newTag;
        }    
    }
}