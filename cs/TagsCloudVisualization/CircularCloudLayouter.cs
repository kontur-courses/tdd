﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Rectangle PutNextRectangle(Point center, List<Rectangle> rectangles, Size nextRectangleSize)
        {
            Rectangle nextRectangle = default;
            var shiftX = -nextRectangleSize.Width / 2;
            var shiftY = -nextRectangleSize.Height / 2;

            if (rectangles.Count == 0)
                nextRectangle = new Rectangle(new Point(center.X + shiftX, center.Y + shiftY), nextRectangleSize);
            else
            {
                foreach (var nextRectanglePosition in GetNextRectanglePosition(center))
                {
                    if (rectangles.Any(rectangle =>
                            rectangle.IntersectsWith(new Rectangle(nextRectanglePosition, nextRectangleSize)))) 
                        continue;
                
                    nextRectangle = new Rectangle(nextRectanglePosition, nextRectangleSize);
                    break;
                }
            }
            
            rectangles.Add(nextRectangle);

            return nextRectangle;
        }

        public List<Rectangle> GenerateCloud(Point center, List<Size> rectangleSizes)
        {
            var rectangles = new List<Rectangle>();
            
            foreach (var rectangleSize in rectangleSizes)
                PutNextRectangle(center, rectangles, rectangleSize);

            return rectangles;
        }

        private static IEnumerable<Point> GetNextRectanglePosition(
            Point startPoint, float shiftAngle = 0.1f, float shiftX = 5.0f, float shiftY = 2.5f)
        {
            var angle = 0.0f;
            
            while (true)
            {
                angle += shiftAngle;
                var x = startPoint.X + shiftX * angle * Math.Cos(angle);
                var y = startPoint.Y + shiftY * angle * Math.Sin(angle);

                yield return new Point((int)x, (int)y);
            }
        }
    }
}