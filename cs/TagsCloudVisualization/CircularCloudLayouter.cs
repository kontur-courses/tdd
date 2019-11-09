﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedesSpiral archimedesSpiral;

        public List<Rectangle> Rectangles { get; }
        private Point center;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            archimedesSpiral = new ArchimedesSpiral(center, 0.1);
            Rectangles = new List<Rectangle>();
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.IsEmpty)
                throw new ArgumentException("Empty rectangle size");
            var rectangle = GetNextRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var rectangleToPlace = default(Rectangle);
            foreach (var point in archimedesSpiral.GetSpiralPoints())
            {
                var possibleRectangle = new Rectangle(point, rectangleSize).GetCentered(point);
                if (possibleRectangle.IntersectsWithAny(Rectangles)) continue;
                rectangleToPlace = GetRectanglePushedCloserToCenter(possibleRectangle);
                break;
            }
            return rectangleToPlace;
        }

        private Rectangle GetRectanglePushedCloserToCenter(Rectangle rectangle)
        {
            var rectangleCenter = rectangle.GetCenter();
            var xDirection = 0;
            var yDirection = 0;
            while (!rectangle.IntersectsWithAny(Rectangles) && center.GetDistanceTo(rectangleCenter) > 10)
            {
                xDirection = rectangleCenter.X >= center.X ? rectangleCenter.X == center.X ? 0 : -1 : 1;
                yDirection = rectangleCenter.Y >= center.Y ? rectangleCenter.Y == center.Y ? 0 : -1 : 1;
                rectangle.Offset(xDirection, yDirection);
            }
            rectangle.Offset(-xDirection, -yDirection);
            return rectangle;
        }
    }
}