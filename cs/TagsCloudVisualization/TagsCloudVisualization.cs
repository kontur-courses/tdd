using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualization
    {
        public Point Center;
        public List<Rectangle> WordPositions;
        public double Radius;
        public double Angle;

        public TagsCloudVisualization(Point center)
        {
            Center = center;
            Radius = 0;
            Angle = 0;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
             throw new ArgumentException();
        }

    }

    /*public class WordPosition
    {
        public Point Position;
        public double Width;
        public double Height;

        public WordPosition(Point position, double width, double height)
        {
            Position = position;
            Width = width;
            Height = height;
        }
    }*/
}