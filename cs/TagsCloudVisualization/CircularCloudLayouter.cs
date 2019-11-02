using System.Drawing;
using System;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        private SortedSet<Point> cornerPoints;
        private HashSet<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            cornerPoints = new SortedSet<Point>(new PointRadiusComparer(center)) {center};
            rectangles = new HashSet<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty || rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Rectangle should exist in real world");
            foreach (var possibleLocation in cornerPoints)
            {
                foreach (var rectangle in RectanglesExtension.GetAllPossibleRectanglePosition(possibleLocation,
                    rectangleSize))
                {
                    if (RectanglesExtension.HaveRectangleIntersectWithAnother(rectangle, rectangles))
                        continue;
                    rectangles.Add(rectangle);
                    foreach (var corner in RectanglesExtension.CornersOfRectangles(rectangle))
                        cornerPoints.Add(corner);

                    return rectangle;
                }
            }

            throw new Exception("Something went wrong");
        }

        public Bitmap Visualization(Bitmap bitmap)
        {
            var graphics = Graphics.FromImage(bitmap);
            var rnd = new Random();
            foreach (var rectangle in rectangles)
            {
                var color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                graphics.FillRectangle(new SolidBrush(color), rectangle);
            }

            return bitmap;
        }
    }
}