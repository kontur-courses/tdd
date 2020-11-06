using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        // Идея такова
        // При добавлении первого прямоугольника делю плоскость на 4 части: верхнюю, правую, нижнюю и левую
        // Делю ее для удобства (ничего лучше не придумал)
        // В каждой из частей пытаюсь размместить другие прямоугольники так, чтобы расстояние до центра было минимальным
        // Для каждой плоскости получается 4 расстояния, и уже среди них беру лучшее и добавляю прямоугольник с
        // получившимися координатами
        // Долго думал над спиралью, но так и не надумал ничего((( Поэтому написал то, что написал
        private readonly int rectangleMargin;
        public Point Center { get; }
        private readonly List<PlanePart> parts;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangleMargin = 2;
            parts = new List<PlanePart>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle {Size = rectangleSize, Location = CalculateLocation(rectangleSize)};
            return rectangle;
        }

        private Point CalculateLocation(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle size should be positive.");
            
            if (parts.Count == 0)
            {
                var location = new Point(Center.X - rectangleSize.Width / 2, Center.Y - rectangleSize.Height / 2);
                InitializePlaneParts(new Rectangle{Size = rectangleSize, Location = location});
                return location;
            }

            var bestDistance = double.MaxValue;
            var bestLocation = new Point();
            var bestPartIndex = 0;

            for (var i = 0; i < 4; i++)
            {
                var location = parts[i].GetBestLocation(rectangleSize);
                var distance = CalculateDistance(location.X + rectangleSize.Width / 2.0,
                    location.Y + rectangleSize.Height / 2.0, Center.X, Center.Y);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestLocation =location;
                    bestPartIndex = i;
                }
            }

            parts[bestPartIndex].AddRectangle(new Rectangle{Size = rectangleSize, Location = bestLocation});

            return bestLocation;
        }

        private void InitializePlaneParts(Rectangle central)
        {
            InitializePlanePart(PartType.Top,
                new Point(central.X - rectangleMargin, central.Y + central.Height + rectangleMargin));
            InitializePlanePart(PartType.Right, new Point(central.X + central.Width + rectangleMargin,
                central.Y + central.Height + rectangleMargin));
            InitializePlanePart(PartType.Bottom, new Point(central.X + central.Width + rectangleMargin,
                central.Y - rectangleMargin));
            InitializePlanePart(PartType.Left, new Point(central.X - rectangleMargin,
                central.Y - rectangleMargin));
        }

        private void InitializePlanePart(PartType partType, Point rectangleLocation)
        {
            var part = new PlanePart(Center, partType, rectangleMargin);
            // Т.к. вначале в каждой из частей плоскости нет прямоугольникв, но отталкиваться от чего-то нужно, добавляю
            // в нее пустой прямоугольник
            part.AddRectangle(new Rectangle{Size = new Size(0, 0), Location = rectangleLocation});
            parts.Add(part);
        }
        
        public static double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}