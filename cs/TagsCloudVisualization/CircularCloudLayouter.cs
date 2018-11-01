using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public Size(int w, int h)
        {
            Width = w;
            Height = h;
        }
    }

    public class Rectangle
    {
        public Point Pos { get; }
        public Size Size { get; }

        public Rectangle(Point pos, Size size)
        {
            Pos = pos;
            Size = size;
        }
    }
    
    public class CircularCloudLayouter
    {
        private Point layoutCenter;
        private List<Rectangle> rects = new List<Rectangle>();
            
        public CircularCloudLayouter(Point center)
        {
            layoutCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return AddRectangleToLayout(rectangleSize);
            
        }

        private Rectangle AddRectangleToLayout(Size size)
        {
            var rect = new Rectangle(layoutCenter, size);
            rects.Add(rect);
            return rect;
        }
    }
}