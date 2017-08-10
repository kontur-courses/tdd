using System;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Dictionary<Point, Rectangle> Rectangles;

        public Dictionary<Point, Rectangle> GetCloud()
        {
            return Rectangles;
        }

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new Dictionary<Point, Rectangle>();
        }

        Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }
    }

  }