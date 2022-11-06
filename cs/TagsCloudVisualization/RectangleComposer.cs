using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleComposer
    {
        public Spiral Spiral { get; set; }
        public List<Rectangle> Rectangles { get; set; }
        public static readonly int stepToCenter = 5;
        public static readonly int centerAreaRadius = 10;

        public RectangleComposer(List<Rectangle> layouterRectangles, Point spiralCenter)
        {
            Spiral = new Spiral(spiralCenter);
            Rectangles = layouterRectangles;
        }

        public Rectangle GetNextRectangleInCloud(Size newRect)
        {
            var location = new Point(Spiral.Center.X, Spiral.Center.Y);
            var rectangle = new Rectangle(location, newRect);
            var rectOnSpiral = FindFreePlaceOnSpiral(rectangle);
            var centeredRect = MoveToCenter(rectOnSpiral);

            Rectangles.Add(centeredRect);
            return centeredRect;
        }

        public Rectangle FindFreePlaceOnSpiral(Rectangle newRectange)
        {
            while (true) // сомневаюсь, что это хорошая идея (наращиваем спираль до бесконечности)
            {
                foreach (var point in Spiral.FreePoints)
                {
                    var newLoc = new Point(point.X - newRectange.Width / 2, point.Y - newRectange.Height / 2);
                    newRectange.Location = newLoc;

                    if (IsRectangleNotIntersectOther(newRectange, Rectangles))
                    {
                        Spiral.FreePoints.Remove(point);
                        return newRectange;
                    }
                }

                Spiral.AddMorePointsInSpiral(10);
            }
        }

        public Rectangle MoveToCenter(Rectangle rect)
        {
            var rectCenter = new Point(
                rect.X + rect.Width / 2, 
                rect.Y + rect.Height / 2);

            var delX = rectCenter.X - Spiral.Center.X;
            var delY = rectCenter.Y - Spiral.Center.Y;
            var angleToCenter = Math.Atan2(delY, delX);

            while (!IsRectangleInCenter(rect, Spiral.Center))
            {
                Point nextPoint;

                if (delX >= 0 && delY <= 0) // 1 четверть
                {
                    nextPoint = GetNextPointToCenter(angleToCenter);
                    nextPoint = new Point(-nextPoint.X, nextPoint.Y);
                }
                else if (delX > 0 && delY > 0) // 4 четверть
                {
                    nextPoint = GetNextPointToCenter(angleToCenter);
                    nextPoint = new Point(-nextPoint.X, -nextPoint.Y);
                }
                else if (delX < 0 && delY > 0) // 3 четверть
                {
                    nextPoint = GetNextPointToCenter(Math.PI - angleToCenter);
                    nextPoint = new Point(nextPoint.X, -nextPoint.Y);
                }
                else //(delX < 0 && delY < 0) // 2 четверть
                {
                    nextPoint = GetNextPointToCenter(angleToCenter + Math.PI);
                }

                var newLoc = new Point(rect.Location.X + nextPoint.X, rect.Location.Y + nextPoint.Y);
                var newRect = new Rectangle(newLoc, rect.Size);

                if (!IsRectangleNotIntersectOther(newRect, Rectangles))
                {
                    return rect;
                }
                else
                {
                    rect.Location = newLoc;
                }
            }

            return rect;
        }

        public Point GetNextPointToCenter(double angleToCenter)
        {
            var angle = Math.Abs(angleToCenter);
            var angleSin = Math.Sin(angle);
            var yStep = angleSin * stepToCenter;
            var xStep = Math.Sqrt(stepToCenter * stepToCenter - yStep * yStep);
            return new Point((int)xStep, (int)yStep);
        }

        public static bool IsRectangleNotIntersectOther(Rectangle rect, List<Rectangle> other)
        {
            var flag = true;

            foreach (var rec in other)
            {
                if (rec.IntersectsWith(rect))
                {
                    flag = false;
                }
            }

            return flag;
        }

        public static bool IsRectangleInCenter(Rectangle rect, Point center)
        {
            var rectCenter = new Point(
                rect.X + rect.Width / 2 - center.X,
                rect.Y + rect.Height / 2 - center.Y);

            if (Math.Abs(rectCenter.X) < centerAreaRadius && 
                Math.Abs(rectCenter.Y) < centerAreaRadius)
            {
                return true;
            }
             
            return false;
        }
    }

}