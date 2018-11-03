using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Rectangle
    {
        public Point Pos { get; set; }
        public Size Size { get; }

        public Rectangle(Point pos, Size size)
        {
            Pos = pos;
            Size = size;
        }

        public Point bottmRightPoint => new Point(Pos.X + Size.Width, Pos.Y + Size.Height);

        public static bool IsOverlap(Rectangle a, Rectangle b)
        {
            var aBottomRight = a.bottmRightPoint;
            var bBottomRight = b.bottmRightPoint;

            return a.Pos.X < bBottomRight.X &&
                   aBottomRight.X > b.Pos.X &&
                   a.Pos.Y < bBottomRight.Y &&
                   aBottomRight.Y > b.Pos.Y;
        }
        
        public static Rectangle GetOuterRect(IEnumerable<Rectangle> rects)
        {
            Rectangle topRect = rects.First();
            Rectangle leftRect = rects.First();
            Rectangle bottomRect = rects.First();
            Rectangle rightRect = rects.First();
            
            foreach (var rect in rects)
            {
                if (rect.Pos.Y < topRect.Pos.Y)
                {
                    topRect = rect;
                }
                
                if (rect.Pos.X < leftRect.Pos.X)
                {
                    leftRect = rect;
                }

                if (rect.bottmRightPoint.Y > bottomRect.bottmRightPoint.Y)
                {
                    bottomRect = rect;
                }

                if (rect.bottmRightPoint.X > rightRect.bottmRightPoint.X)
                {
                    rightRect = rect;
                }
            }

            var topLeftPoint = new Point(leftRect.Pos.X, topRect.Pos.Y);
            var totalSize = new Size(
                Math.Abs(rightRect.bottmRightPoint.X - topLeftPoint.X), 
                Math.Abs(bottomRect.bottmRightPoint.Y - topLeftPoint.Y)
            );
            
            return new Rectangle(topLeftPoint, totalSize);
        }
    }
}