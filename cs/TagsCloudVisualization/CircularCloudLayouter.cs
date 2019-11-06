using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        Point center;
        List<Rectangle> rectangles;
        ISpiral spiral;
        IEnumerator<Point> spiralPoints;

        public CircularCloudLayouter(Point center) => new CircularCloudLayouter(center, new ArchimedeanSpiral());

        public CircularCloudLayouter(Point center, ISpiral spiral)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            this.spiral = spiral;
            spiralPoints = spiral.GetPoints().GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException(
                    $"Unable to create a rectangle with width = {rectangleSize.Width} and height = {rectangleSize.Height}");

            while(true)
            {
                var curentSpiralPoint = spiralPoints.Current;
                var curentPosition = new Point(
                    center.X + curentSpiralPoint.X, 
                    center.Y + curentSpiralPoint.Y);

                var rectangle = new Rectangle(curentPosition, rectangleSize);

                if (TryAddRectangle(rectangle)) return rectangle;
                else spiralPoints.MoveNext();
            }
        }

        bool TryAddRectangle(Rectangle rectangle)
        {
            if (rectangles.Exists(rec => rec.IntersectsWith(rectangle))) 
                return false;

            rectangles.Add(rectangle);
            return true;
        }
    }
}
