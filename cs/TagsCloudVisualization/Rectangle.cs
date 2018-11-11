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
        
        public Rectangle(int x, int y, int w, int h)
        {
            Pos = new Point(x, y);
            Size = new Size(w, h);
        }

        public Point BottmRightPoint => new Point(Pos.X + Size.Width, Pos.Y + Size.Height);
        public Point CenterPoint => new Point(Pos.X + Size.Width / 2, Pos.Y + Size.Height / 2);

        public bool OverlapsWith(Rectangle rect)
        {
            var a = this;
            var b = rect;
            var aBottomRight = a.BottmRightPoint;
            var bBottomRight = b.BottmRightPoint;

            return a.Pos.X < bBottomRight.X &&
                   aBottomRight.X > b.Pos.X &&
                   a.Pos.Y < bBottomRight.Y &&
                   aBottomRight.Y > b.Pos.Y;
        }

        public bool ContainsRect(Rectangle rect)
        {
            var aBottomRight = this.BottmRightPoint;
            var bBottomRight = rect.BottmRightPoint;
            
            return (this.Pos.X <= rect.Pos.X &&
                    this.Pos.Y <= rect.Pos.Y &&
                    aBottomRight.X >= bBottomRight.X &&
                    aBottomRight.Y >= bBottomRight.Y );
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

                if (rect.BottmRightPoint.Y > bottomRect.BottmRightPoint.Y)
                {
                    bottomRect = rect;
                }

                if (rect.BottmRightPoint.X > rightRect.BottmRightPoint.X)
                {
                    rightRect = rect;
                }
            }

            var topLeftPoint = new Point(leftRect.Pos.X, topRect.Pos.Y);
            var totalSize = new Size(
                Math.Abs(rightRect.BottmRightPoint.X - topLeftPoint.X), 
                Math.Abs(bottomRect.BottmRightPoint.Y - topLeftPoint.Y)
            );
            
            return new Rectangle(topLeftPoint, totalSize);
        }
    }
}