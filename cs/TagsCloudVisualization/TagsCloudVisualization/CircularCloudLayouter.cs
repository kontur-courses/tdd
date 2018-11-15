using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private int left;
        private int right;
        private int top;
        private int bottom;
        private Spiral spiral;

        public int GetWidth => right - left;
        public int GetHeight => bottom - top;

        public List<Rectangle> tags = new List<Rectangle>();
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            left = right = center.X;
            top = bottom = center.Y;
            spiral = new Spiral(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = spiral.GetNextPoint();
            var newTag = new Rectangle(new Point(), rectangleSize);
            foreach (var point in location)
            {
                newTag = new Rectangle(point, rectangleSize);
                
                if (!IsThereIntersection(newTag))
                {       
                    tags.Add(MoveRectangleCloserToCenter(newTag));
                    left = (newTag.Left < left) ? newTag.Left : left;
                    right = (newTag.Right > right) ? newTag.Right : right;
                    top = (newTag.Top < top) ? newTag.Top : top;
                    bottom = (newTag.Bottom > bottom) ? newTag.Bottom : bottom;
                    break;
                }
            }
            return newTag;
        }

        private bool IsThereIntersection(Rectangle newTag)
        {
            return tags.Any(newTag.IntersectsWith);
        }

        private Rectangle MoveRectangleCloserToCenter(Rectangle newTag)
        {
            while (newTag.X != Center.X)
            {
                var oldX = newTag.X;
                newTag.X += Center.X < newTag.X ? -1 : 1;
                if (IsThereIntersection(newTag))
                {
                    newTag.X = oldX;
                    break;
                }
            }
        
            while (newTag.Y != Center.Y)
            {
                var oldY = newTag.Y;
                newTag.Y += Center.Y < newTag.Y ? -1 : 1;
                if (IsThereIntersection(newTag))
                {
                    newTag.Y = oldY;
                    break;
                }
            }

            return newTag;
        }
    }
}