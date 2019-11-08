using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<IFigure> figures;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            figures = new List<IFigure>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentOutOfRangeException();

            var rectangle = new Figures.Rectangle { Size = rectangleSize };
            rectangle.Location = FindFreeLocationForFigure(rectangle);
            figures.Add(rectangle);

            return rectangle.BaseRectangle;
        }

        private (int radius, double angle, double step) parameters = (0, 0, 1);
        private Point FindNextPoint()
        {
            const double maxAngle = 2 * Math.PI;

            if ((parameters.angle += parameters.step) >= maxAngle)
            {
                parameters.radius++;
                parameters.angle = 0;
                parameters.step = Math.PI / Math.Pow(parameters.radius + 1, 0.7);
            }
            return new Point(
                x: center.X + (int)(parameters.radius * Math.Cos(parameters.angle)),
                y: center.Y + (int)(parameters.radius * Math.Sin(parameters.angle)));
        }

        private Point FindFreeLocationForFigure(IFigure figure)
        {
            do figure.Center = FindNextPoint();
            while (figures.Any(f => f.IntersectsWith(figure)));
            return figure.Location;
        }
    }
}