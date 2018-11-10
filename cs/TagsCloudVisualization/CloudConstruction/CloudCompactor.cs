using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.CloudConstruction
{
    public class CloudCompactor
    {
        public CircularCloudLayouter Cloud { get; set; }

        public CloudCompactor(CircularCloudLayouter cloud)
        {
            Cloud = cloud;
        }

        public Rectangle ShiftRectangleToTheNearest(Rectangle rectangle)
        {
            if (Cloud.Rectangles.Count == 0)
                return rectangle;
            var yLevelRectangles = Cloud.Rectangles.Where(rect => !(rectangle.Y > rect.Y + rect.Height
                                                                  || rectangle.Y + rectangle.Height < rect.Y)).ToList();
            rectangle = FindNearestRectangleHorizontally(rectangle, yLevelRectangles);
            var xLevelRectangles = Cloud.Rectangles.Where(rect => !(rectangle.X > rect.X + rect.Width
                                                                  || rectangle.X + rectangle.Width < rect.X)).ToList();
            rectangle = FindNearestRectangleVertically(rectangle, xLevelRectangles);

            return rectangle;
        }

        private Rectangle FindNearestRectangleHorizontally(Rectangle rectangle, List<Rectangle> yLevelRectangles)
        {
            if (yLevelRectangles.Count == 0) return rectangle;
            int distanceToNearestRectangle;
            if (rectangle.X <= Cloud.Center.X)
            {
                var listCorrectRectangles = yLevelRectangles.Where(rec => rec.X >= rectangle.X + rectangle.Width).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : listCorrectRectangles
                        .Min(rec => Math.Abs(rec.X - (rectangle.X + rectangle.Width)));
            }
            else
            {
                var listCorrectRectangles = yLevelRectangles.Where(rec => rec.X + rec.Width <= rectangle.X).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : -listCorrectRectangles
                        .Min(rec => Math.Abs(rec.X + rec.Width - rectangle.X));
            }
            var newLocation = new Point(rectangle.X + distanceToNearestRectangle, rectangle.Y);
            rectangle = new Rectangle(newLocation, rectangle.Size);

            return rectangle;
        }

        private Rectangle FindNearestRectangleVertically(Rectangle rectangle, List<Rectangle> xLevelRectangles)
        {
            if (xLevelRectangles.Count == 0) return rectangle;
            int distanceToNearestRectangle;
            if (rectangle.Y >= Cloud.Center.Y)
            {
                var listCorrectRectangles = xLevelRectangles.Where(rec => rec.Y + rec.Height <= rectangle.Y).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : -listCorrectRectangles
                        .Min(rec => Math.Abs(rec.Y + rec.Height - rectangle.Y));
            }
            else
            {
                var listCorrectRectangles =
                    xLevelRectangles.Where(rec => rec.Y >= rectangle.Y + rectangle.Height).ToList();
                distanceToNearestRectangle = listCorrectRectangles.Count == 0
                    ? 0
                    : listCorrectRectangles
                        .Min(rec => Math.Abs(rec.Y - rectangle.Y - rectangle.Height));
            }
            var newLocation = new Point(rectangle.X, rectangle.Y + distanceToNearestRectangle);
            rectangle = new Rectangle(newLocation, rectangle.Size);
            return rectangle;
        }
    }
}