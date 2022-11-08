﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.TagCloud
{
    /// <summary> Построитель облака тегов </summary>
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public List<Rectangle> rectangles { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize == null
                || rectangleSize.Width <= 0
                || rectangleSize.Height <= 0)
            {
                throw new AggregateException("Передан некорректный размер прямоугольника.");
            }

            var spiral = new Spiral(center);

            while (true)
            {
                var point = spiral.GetNextPoint();
                var coordinate = new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2);
                var rectangle = new Rectangle(coordinate, rectangleSize);

                if (TryPlaceRectangle(rectangle))
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                }
            }
        }

        private bool TryPlaceRectangle(Rectangle rectangle)
        {
            return !rectangles.Any(x => x.IntersectsWith(rectangle));
        }
    }
}
