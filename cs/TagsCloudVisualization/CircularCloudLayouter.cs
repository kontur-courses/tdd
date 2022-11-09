using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;

        private readonly List<Rectangle> rectangles;
        private readonly Func<int, Point> pointFinderFunc;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            pointFinderFunc = SpiralFunction.GetPointFinderFunction(center);
        }

        public List<Rectangle> GetRectangles()
        {
            return new List<Rectangle>(rectangles);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("only positive size");
            var rect = FindFreePlaceForNewRectangle(rectangleSize);
            TryMoveRectangleCloserToCenter(ref rect);
            rectangles.Add(rect);
            return rect;
        }

        private void TryMoveRectangleCloserToCenter(ref Rectangle rect)
        {
            var xStep = rect.GetCenter().X > Center.X ? new Point(-1, 0) : new Point(1, 0);
            TryMoveRectangleToTarget(rect.GetCenter().X, Center.X, xStep, ref rect);

            var yStep = rect.GetCenter().Y > Center.Y ? new Point(0, -1) : new Point(0, 1);
            TryMoveRectangleToTarget(rect.GetCenter().Y, Center.Y, yStep, ref rect);
        }
        
        private void TryMoveRectangleToTarget(int startPos, int targetPos, Point stepPoint, ref Rectangle rect)
        {
            var step = targetPos > startPos ? 1 : -1;
            while (NotIntersectOthers(rect) && startPos != targetPos)
            {
                startPos += step;
                rect.Location = rect.Location.Plus(stepPoint);
            }

            if (!NotIntersectOthers(rect))
                rect.Location = rect.Location.Minus(stepPoint);
        }

        private Rectangle FindFreePlaceForNewRectangle(Size rectangleSize)
        {
            var arg = 0;
            while (true)
            {
                var rectCenter = pointFinderFunc(arg);
                var x = rectCenter.X - rectangleSize.Width / 2;
                var y = rectCenter.Y - rectangleSize.Height / 2;
                var rect = new Rectangle(new Point(x, y), rectangleSize);
                if (NotIntersectOthers(rect))
                    return rect;
                arg++;
            }
        }

        private bool NotIntersectOthers(Rectangle rect)
        {
            return rectangles.All(r => !rect.IntersectsWith(r));
        }
    }
}
