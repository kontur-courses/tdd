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
        private readonly Spiral spiral;

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
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height must be positive");
            var spiralPoints = spiral.GetNextPoint();
            var newTag = new Rectangle(new Point(), rectangleSize);
            foreach (var nextPoint in spiralPoints)
            {
                newTag = new Rectangle(nextPoint, rectangleSize);
                
                if (!IsThereIntersectionWithAnyAnotherRectangle(newTag))
                {       
                    tags.Add(MoveRectangleCloserToCenter(newTag));
                    ChangeCloudBoundaries(newTag);
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

        private void ChangeCloudBoundaries(Rectangle newTag)
        {
            left = (newTag.Left < left) ? newTag.Left : left;
            right = (newTag.Right > right) ? newTag.Right : right;
            top = (newTag.Top < top) ? newTag.Top : top;
            bottom = (newTag.Bottom > bottom) ? newTag.Bottom : bottom;
        }
    }
}