using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Helpers
    {
        public static Rectangle GetOuterRectOfRects(IEnumerable<Rectangle> rects)
        {
            Rectangle topRect = rects.First();
            Rectangle leftRect = rects.First();
            Rectangle bottomRect = rects.First();
            Rectangle rightRect = rects.First();
            
            foreach (var rect in rects)
            {
                if (rect.Y < topRect.Y)
                {
                    topRect = rect;
                }
                
                if (rect.X < leftRect.X)
                {
                    leftRect = rect;
                }

                if (rect.Bottom > bottomRect.Bottom)
                {
                    bottomRect = rect;
                }

                if (rect.Right > rightRect.Right)
                {
                    rightRect = rect;
                }
            }

            var topLeftPoint = new Point(leftRect.X, topRect.Y);
            var totalSize = new Size(
                Math.Abs(rightRect.Right - topLeftPoint.X), 
                Math.Abs(bottomRect.Bottom - topLeftPoint.Y)
            );
            
            return new Rectangle(topLeftPoint, totalSize);
        }

        public static int PowDistanceBetweenPoints(Point a, Point b)
        {
            var dx = (b.X - a.X);
            var dy = (b.Y - a.Y);
            return dx * dx + dy * dy;
        }
    }
}
