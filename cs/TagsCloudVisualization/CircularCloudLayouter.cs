using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public HashSet<Rectangle> Centreings()
        {
            var centreingsCloudLayout = new CircularCloudLayouter(center);
            var newRectangles = new HashSet<Rectangle>();
            foreach (var rectangle in rectangles.OrderBy(x =>  -x.Width * x.Height))
                newRectangles.Add(centreingsCloudLayout.PutNextRectangle(rectangle.Size));

            return newRectangles;
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

        public Bitmap Visualization(Bitmap bitmap, HashSet<Rectangle> rectangles)
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

        public Bitmap Visualization(Bitmap bitmap) =>
            Visualization(bitmap, rectangles);
    }
}