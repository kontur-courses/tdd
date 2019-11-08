using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; }
        public Point Center { get; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            if (Rectangles.Count == 0)
            {
                var x = Center.X - rectangleSize.Width / 2;
                var y = Center.Y - rectangleSize.Height / 2;
                var rectangle = new Rectangle(new Point(x, y), rectangleSize);
                Rectangles.Add(rectangle);
                return rectangle;
            }
            var spiral = new ArchimedeanSprial(0, Math.PI / 12, Math.PI / 12, 0.4, Center, rectangleSize);
            while (true)
            {
                var rectangle = new Rectangle(spiral.GetNextCoordinate(), rectangleSize);
                if (!Rectangles.Any(rec => rec.IntersectsWith(rectangle)))
                {
                    Rectangles.Add(rectangle);
                    return rectangle;
                }
            }
        }
    }

    public class ArchimedeanSprial
    {
        public double A { get; }
        public double Alpha { get; private set; }
        public double Step { get; }
        public double DensityCoefficient { get; }
        public Point Center { get; }
        private double RadiusVector { get; set; }
        private Size RectangleSize { get; }

        public ArchimedeanSprial(double a, double alpha, double step, double densityCoefficient, Point center, Size rectangleSize)
        {
            A = a;
            Alpha = alpha;
            Step = step;
            DensityCoefficient = densityCoefficient;
            Center = center;
            RectangleSize = rectangleSize;
        }

        public Point GetNextCoordinate()
        {
            Alpha += Step;
            RadiusVector = Alpha * DensityCoefficient;
            var x = (int)(RadiusVector * Math.Cos(Alpha)) - RectangleSize.Width / 2 + Center.X;
            var y = (int)(RadiusVector * Math.Sin(Alpha)) - RectangleSize.Height / 2 + Center.Y;
            return new Point(x, y);
        }
    }
}
