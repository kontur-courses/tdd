using System;
using System.Drawing;
using System.Collections.Generic;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Spiral _spiral;
        private List<Rectangle> _rectangles;
        private Random _random;

        public IReadOnlyList<Rectangle> Rectangles => _rectangles;
        
        public CircularCloudLayouter(Point center, double step = 10)
        {
            _rectangles = new List<Rectangle>();
            _spiral = new Spiral(center, step);
            _random = new Random();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize == Size.Empty)
                throw new ArgumentException("Size must not be equal to 0");
            
            Rectangle rectangle;
            do
            { 
                rectangle = new Rectangle(_spiral.NextPoint(), rectangleSize);
            } while (IsIntersects(rectangle)); 
            
            _rectangles.Add(rectangle);
            return rectangle;
        }
        
        private bool IsIntersects(Rectangle newRectangle)
        {
            foreach (var rectangle in Rectangles)
            {
                if (rectangle.IntersectsWith(newRectangle))
                    return true;
            }
            return false;
        }

        public void AddRandomRectangles(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException();
            for (int i = 0; i < amount; i++)
            {
                PutNextRectangle(new Size(_random.Next() % 50, _random.Next() % 50));
            }
        }

    }
}