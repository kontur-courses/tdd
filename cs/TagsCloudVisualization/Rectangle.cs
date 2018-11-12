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

        public Point BottomRightPoint => new Point(Pos.X + Size.Width, Pos.Y + Size.Height);
        public Point CenterPoint => new Point(Pos.X + Size.Width / 2, Pos.Y + Size.Height / 2);

        public bool OverlapsWith(Rectangle rect)
        {
            var aBottomRight = this.BottomRightPoint;
            var bBottomRight = rect.BottomRightPoint;

            return this.Pos.X < bBottomRight.X &&
                   this.Pos.Y < bBottomRight.Y &&
                   aBottomRight.X > rect.Pos.X &&
                   aBottomRight.Y > rect.Pos.Y;
        }

        public bool ContainsRect(Rectangle rect)
        {
            var aBottomRight = this.BottomRightPoint;
            var bBottomRight = rect.BottomRightPoint;
            
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

                if (rect.BottomRightPoint.Y > bottomRect.BottomRightPoint.Y)
                {
                    bottomRect = rect;
                }

                if (rect.BottomRightPoint.X > rightRect.BottomRightPoint.X)
                {
                    rightRect = rect;
                }
            }

            var topLeftPoint = new Point(leftRect.Pos.X, topRect.Pos.Y);
            var totalSize = new Size(
                Math.Abs(rightRect.BottomRightPoint.X - topLeftPoint.X), 
                Math.Abs(bottomRect.BottomRightPoint.Y - topLeftPoint.Y)
            );
            
            return new Rectangle(topLeftPoint, totalSize);
        }
    }
}