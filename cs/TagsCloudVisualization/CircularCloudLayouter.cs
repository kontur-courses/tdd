using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> rectanglesList;
        private Point сenter;        

        public CircularCloudLayouter(Point currentCenter)
        {
            if (currentCenter.X < 0 || currentCenter.Y < 0)
                throw new ArgumentException("Center coordinates should be greater than null");
            rectanglesList = new List<Rectangle>();
            сenter = currentCenter;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectanglesList.Count == 0)
            {
                var newCoordinates = new Point(сenter.X - (rectangleSize.Width / 2), сenter.Y - (rectangleSize.Height / 2));
                rectanglesList.Add(new Rectangle(newCoordinates, rectangleSize));               
                return new Rectangle(newCoordinates, rectangleSize);
            }

            var rectangle = new Rectangle(сenter, rectangleSize);
            var min = int.MaxValue;
            var suitablePointsList = GetSuitablePointsList(rectangle);
            var result = сenter;

            for (var k = 0; k < suitablePointsList.Count; k++)
            {
                if (min > GetSquaredDistanceFromCenterToRectangle(suitablePointsList[k]))
                {
                    min = GetSquaredDistanceFromCenterToRectangle(suitablePointsList[k]);
                    result = suitablePointsList[k];
                }
            }
            rectangle.Location = result;
            rectanglesList.Add(rectangle);
            return rectanglesList[rectanglesList.Count - 1];
        }

        private bool CheckRectangleDoesNotIntersectWithAnyAnother(Rectangle rectangle)
        {
            return !rectanglesList.Any(t => t.IntersectsWith(rectangle));
        }

        private List<Point> GetSuitablePointsList(Rectangle rectangle)
        {
            var pointsList = new List<Point>();
            for (var i = 0; i < rectanglesList.Count; i++)
            {
                var rectangleSides = GetRectangleSides(rectanglesList[i]);
                for (var j = 0; j < rectangleSides.Count; j++)
                {
                    rectangle.Y = rectangleSides[2];
                    if (j == 2)
                        rectangle.Y -= rectangle.Height;
                    else if (j == 3)
                        rectangle.Y = rectangleSides[3];

                    rectangle.X = rectangleSides[0];
                    if (j == 0)
                        rectangle.X -= rectangle.Width;
                    else if (j == 1)
                        rectangle.X = rectangleSides[1];
                    var side = (j == 0 || j == 1) ? rectanglesList[i].Height : rectanglesList[i].Width;
                    for (var k = 0; k < side; k = k + 5)
                    {
                        if (j == 0 || j == 1)
                            rectangle.Y -= k;
                        else
                            rectangle.X += k;
                        if (CheckRectangleDoesNotIntersectWithAnyAnother(rectangle))
                            pointsList.Add(rectangle.Location);
                    }
                }
            }
            return pointsList;
        }

        private List<int> GetRectangleSides(Rectangle rectangle)
        {
            var rectangleSides = new List<int>();
            rectangleSides.Add(rectangle.Left);
            rectangleSides.Add(rectangle.Right);
            rectangleSides.Add(rectangle.Top);
            rectangleSides.Add(rectangle.Bottom);
            return rectangleSides;
        }

        private int GetSquaredDistanceFromCenterToRectangle(Point point)
        {
            return (int)(Math.Pow(сenter.X - point.X, 2) + Math.Pow(сenter.Y - point.Y, 2));
        }
    }
}
