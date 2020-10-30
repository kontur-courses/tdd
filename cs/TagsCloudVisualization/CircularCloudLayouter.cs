using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Rectangle rectangle;
        private Rectangle lastRectangle;
        private Point center;
        private int position =1;
        private int top = int.MaxValue;
        private int bottom = 0;
        private int left = 0;
        private int right = 0;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangle.IsEmpty)
            {
                rectangle= new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize);
                return rectangle;
            }

            if (position == 1)
            {
                if (lastRectangle.IsEmpty)
                {
                    lastRectangle = new Rectangle(new Point(rectangle.Left, rectangle.Top - rectangleSize.Height), rectangleSize);
                }
                else
                {
                    lastRectangle = new Rectangle(new Point(lastRectangle.Right, rectangle.Top - rectangleSize.Height), rectangleSize);
                }

                if (top > lastRectangle.Top)
                    top = lastRectangle.Top;

                if (lastRectangle.Right > rectangle.Right)
                {
                    right = lastRectangle.Right;
                    position = 2;
                    rectangle = new Rectangle(new Point(rectangle.Left, top), new Size(rectangle.Width, rectangle.Bottom - top));
                }

                return lastRectangle;
            }
            else if (position == 2)
            {
                lastRectangle = new Rectangle(new Point(rectangle.Right, lastRectangle.Bottom), rectangleSize);

                if (right < lastRectangle.Right)
                    right = lastRectangle.Right;

                if (lastRectangle.Bottom > rectangle.Bottom)
                {
                    bottom = lastRectangle.Bottom;
                    position = 3;
                    rectangle = new Rectangle(new Point(rectangle.Left, rectangle.Top), new Size(right-rectangle.Left , rectangle.Height));
                }

                return lastRectangle;
            }
            else if (position == 3)
            {
                lastRectangle = new Rectangle(new Point(lastRectangle.Left-rectangleSize.Width, rectangle.Bottom), rectangleSize);

                if (bottom < lastRectangle.Bottom)
                    bottom = lastRectangle.Bottom;

                if (lastRectangle.Left < rectangle.Left) 
                {
                    left = lastRectangle.Left;
                    position = 4;
                    rectangle = new Rectangle(new Point(rectangle.Left, rectangle.Top), new Size(rectangle.Width, bottom-rectangle.Top));
                }

                return lastRectangle;
            }
            else
            {
                lastRectangle = new Rectangle(new Point(rectangle.Left-rectangleSize.Width, lastRectangle.Top-rectangleSize.Height), rectangleSize);

                if (left > lastRectangle.Left)
                    left = lastRectangle.Left;

                if (lastRectangle.Top < rectangle.Top) 
                {
                    top = lastRectangle.Top;
                    position = 1;
                    rectangle = new Rectangle(new Point(left, rectangle.Top), new Size(rectangle.Right-left, rectangle.Height));
                }
                
                return lastRectangle;
            }
        }
    }
}