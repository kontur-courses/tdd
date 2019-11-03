using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleGeometry
    {
        public IEnumerable<Rectangle> GetCornerRectangles(Size size, HashSet<Point> points)
        {
            Point corner;
            while (points.Count != 0)
            {
                corner = points.First();
                points.Remove(corner);
                yield return new Rectangle(corner, size);
                yield return new Rectangle(new Point(corner.X - size.Width, corner.Y), size);
                yield return new Rectangle(new Point(corner.X - size.Width, corner.Y - size.Height), size);
                yield return new Rectangle(new Point(corner.X, corner.Y - size.Height), size);
            }
        }

        public IEnumerable<Point> GetConers(Rectangle rectangle)
        {
            yield return rectangle.Location;
            yield return new Point(rectangle.X + rectangle.Width, rectangle.Y);
            yield return new Point(rectangle.X, rectangle.Y + rectangle.Height);
            yield return new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
        }

        public double GetDistanceBetweenpPoints(Point p1, Point p2) =>
            Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

        public PointF CheckCircleForm(List<Rectangle> rectangles, Point center)
        {
            /* 
             * Метод вернет точку центра масс полученной фигуры.
             * Исходя из построения итоговой фигуры(наложение прямоугольнигов друг на друга), можно примерно 
             * понять, где находится окружности "описывающей" итоговую фигуру и
             * куда вытянута окружность с центром в точке center
             */
            float all_squares = 0;
            float all_x = 0;
            float all_y = 0;
            foreach (var rectangle in rectangles)
            {
                all_squares += rectangle.Width * rectangle.Height;
                all_x += (rectangle.X + rectangle.Width / 2) * rectangle.Width * rectangle.Height;
                all_y += (rectangle.Y + rectangle.Height / 2) * rectangle.Width* rectangle.Height;
            }
            return new PointF(all_x/all_squares, all_y/all_squares);
        }
    }

    public class CircularCloudLayouter
    {
        private Point center;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private SortedList<double, HashSet<Point>> corners = new SortedList<double, HashSet<Point>>();
        RectangleGeometry RectangleGeometry = new RectangleGeometry();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            corners.Add(0, new HashSet<Point> { center });
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            double distance;
            foreach (var corner in corners.Values)
                foreach (var rectangle in RectangleGeometry.GetCornerRectangles(rectangleSize, corner))
                {
                    if (rectangles.Any(rec => rec.IntersectsWith(rectangle))) continue; 
                    rectangles.Add(rectangle);
                    foreach (var point in RectangleGeometry.GetConers(rectangle))
                    {
                        distance = RectangleGeometry.GetDistanceBetweenpPoints(point, center);
                        if (corners.ContainsKey(distance)) corners[distance].Add(point);
                        else corners.Add(distance, new HashSet<Point>() { point });
                    }
                    return rectangle;
                }
            throw new Exception();
        }
    }
}   
