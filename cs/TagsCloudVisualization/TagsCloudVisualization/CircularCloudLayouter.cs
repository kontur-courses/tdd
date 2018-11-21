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
            foreach (var nextPoint in location)
            {
                newTag = new Rectangle(nextPoint, rectangleSize);
                
                if (!IsThereIntersectionWithAnyAnotherRectangle(newTag))
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

        private bool IsThereIntersectionWithAnyAnotherRectangle(Rectangle newTag)
        {
            return tags.Any(newTag.IntersectsWith);
        }

        private Rectangle MoveRectangleCloserToCenter(Rectangle tag)
        {
            while (tag.X != Center.X)
            {
                var oldX = tag.X;
                tag.X += Center.X < tag.X ? -1 : 1;
                if (IsThereIntersectionWithAnyAnotherRectangle(tag))
                {
                    tag.X = oldX;
                    break;
                }
            }
        
            while (tag.Y != Center.Y)
            {
                var oldY = tag.Y;
                tag.Y += Center.Y < tag.Y ? -1 : 1;
                if (IsThereIntersectionWithAnyAnotherRectangle(tag))
                {
                    tag.Y = oldY;
                    break;
                }
            }

            return tag;
        }
    }
}