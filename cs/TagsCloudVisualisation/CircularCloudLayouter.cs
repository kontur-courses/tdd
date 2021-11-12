﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualisation
{
    class CircularCloudLayouter
    {
        private readonly ArchimedeanSpiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(ArchimedeanSpiral spiral)
        {
            this.spiral = spiral;
        }
        
        Rectangle GetRectangleWithCenterInPoint(Size rectangleSize, Point center)
        {
            var locationX = center.X - rectangleSize.Width / 2;
            var locationY = center.Y - rectangleSize.Height / 2;
            return new Rectangle(new Point(locationX, locationY), rectangleSize);
        }

        Rectangle CreateRectangleFromSpiral(Size rectangleSize)
        {
            var currentPoint = spiral.GetNextPoint();
            return GetRectangleWithCenterInPoint(rectangleSize, currentPoint);
        }

        public Rectangle PutNewRectangle(Size rectangleSize)
        {
            var rectangle = CreateRectangleFromSpiral(rectangleSize);
            while (rectangle.IntersectsWith(rectangles))
            {
                rectangle = CreateRectangleFromSpiral(rectangleSize);
            }
            rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
