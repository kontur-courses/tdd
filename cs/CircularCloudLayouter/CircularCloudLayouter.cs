using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions.Equivalency;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; }
        public Point Center { get; }
        private const double destinyCOfficient = 0.2; 
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and Height should be greater than zero");
            if (Rectangles.Count == 0)
            {
                var x = Center.X - rectangleSize.Width / 2;
                var y = Center.Y - rectangleSize.Height / 2;
                var rectangle = new Rectangle(new Point(x, y), rectangleSize);
                Rectangles.Add(rectangle);
                return rectangle;
            }
            var spiral = new ArchimedeanSprial(Math.PI / 40, Math.PI / 40, destinyCOfficient, Center);
            while (true)
            {
                var point = new Point(spiral.GetNextCoordinate().X - rectangleSize.Width / 2,
                        spiral.GetNextCoordinate().Y - rectangleSize.Height / 2);
                var rectangle = new Rectangle(point, rectangleSize);
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
        public double Alpha { get; private set; }
        public double Step { get; }
        public double DensityCoefficient { get; }
        public Point Center { get; }
        private double RadiusVector { get; set; }

        public ArchimedeanSprial(double alpha, double step, double densityCoefficient, Point center)
        {
            Alpha = alpha;
            Step = step;
            DensityCoefficient = densityCoefficient;
            Center = center;
        }

        public Point GetNextCoordinate()
        {
            Alpha += Step;
            RadiusVector = Alpha * DensityCoefficient;
            var x = (int)(RadiusVector * Math.Cos(Alpha)) + Center.X;
            var y = (int)(RadiusVector * Math.Sin(Alpha)) + Center.Y;
            return new Point(x, y);
        }
    }
}
