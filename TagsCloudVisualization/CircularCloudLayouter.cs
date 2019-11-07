using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{

    public class CircularCloudLayouter: ICircularCloudLayouter
    {
        

        private Point center;
        private HashSet<Segment> Segments;
        private bool isFirstRectangle;
        private List<Rectangle> rectangles;
        private const bool safeMode = false;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            this.Segments = new HashSet<Segment>();
            this.isFirstRectangle = true;
            this.rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {

            if (isFirstRectangle)
            {
                isFirstRectangle = false;
                Segments.Add(new Segment(
                    center.X - rectangleSize.Width / 2
                    , center.Y - rectangleSize.Height / 2
                    , center.X + rectangleSize.Width / 2
                    , center.Y - rectangleSize.Height / 2
                    , Segment.Type.Top));
                Segments.Add(new Segment(
                    center.X - rectangleSize.Width / 2
                    , center.Y + rectangleSize.Height / 2
                    , center.X + rectangleSize.Width / 2
                    , center.Y + rectangleSize.Height / 2
                    , Segment.Type.Bottom));
                Segments.Add(new Segment(
                    center.X - rectangleSize.Width / 2
                    , center.Y - rectangleSize.Height / 2
                    , center.X - rectangleSize.Width / 2
                    , center.Y + rectangleSize.Height / 2
                    , Segment.Type.Left));
                Segments.Add(new Segment(
                    center.X + rectangleSize.Width / 2
                    , center.Y - rectangleSize.Height / 2
                    , center.X + rectangleSize.Width / 2
                    , center.Y + rectangleSize.Height / 2
                    , Segment.Type.Right));
                rectangles.Add(new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize));
                return new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize);
            }
            double minDistance = double.MaxValue;
            Segment minSegment = null;
            Point minCoord = new Point();
            foreach (var segment in Segments)
            {
                if (((segment.type == Segment.Type.Top || segment.type == Segment.Type.Bottom) && segment.Length < rectangleSize.Width)
                    || ((segment.type == Segment.Type.Left || segment.type == Segment.Type.Right) && segment.Length < rectangleSize.Height))
                {//здесь обрабатываются случаи когда прямоугольник вылезает за пределы отрезка, будет реализовано позднее
                    //continue;//stub
                    if (segment.type == Segment.Type.Top)
                    {
                        var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                        var rightBorderX = FindRightBorderX(segment, rectangleSize);
                        var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                        SearchMinDistanceTop(ref minSegment, ref minDistance, ref minCoord, updatedSegment, rectangleSize);
                    }
                    if (segment.type == Segment.Type.Bottom)
                    {
                        var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                        var rightBorderX = FindRightBorderX(segment, rectangleSize);
                        var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                        SearchMinDistanceBottom(ref minSegment, ref minDistance, ref minCoord, updatedSegment, rectangleSize);
                    }
                    if (segment.type == Segment.Type.Left)
                    {
                        var topBorderY= FindTopBorderY(segment, rectangleSize);
                        var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                        var updatedSegment = new Segment(segment.start.X, topBorderY, segment.end.X, bottomBorderY, segment.type);
                        SearchMinDistanceLeft(ref minSegment, ref minDistance, ref minCoord, updatedSegment, rectangleSize);
                    }
                    if (segment.type == Segment.Type.Right)
                    {
                        var topBorderY = FindTopBorderY(segment, rectangleSize);
                        var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                        var updatedSegment = new Segment(segment.start.X, topBorderY, segment.end.X, bottomBorderY, segment.type);
                        SearchMinDistanceRight(ref minSegment, ref minDistance, ref minCoord, updatedSegment, rectangleSize);
                    }
                }
                else
                {
                    if (segment.type == Segment.Type.Top)
                    {
                        SearchMinDistanceTop(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize);
                    }
                    if (segment.type == Segment.Type.Bottom)
                    {
                        SearchMinDistanceBottom(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize);
                    }
                    if (segment.type == Segment.Type.Left)
                    {
                        SearchMinDistanceLeft(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize);
                    }
                    if (segment.type == Segment.Type.Right)
                    {
                        SearchMinDistanceRight(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize);
                    }
                }
            }
            var outRectangle = new Rectangle(minCoord, rectangleSize);
            var toAdd = new List<Segment>();
            if (safeMode)
            {
                foreach (var rect in rectangles)
                    if(rect.IntersectsWith(outRectangle))
                    {
                        var topBorder = Segments
                                .Select(a => a.start.Y)
                                .OrderBy(a => a)
                                .First();
                        Segments.Remove(minSegment);//полагаем что тот сегмент к которому пытаемся присоединиться багованный
                        minCoord = new Point(center.X - rectangleSize.Width / 2, topBorder - rectangleSize.Height);
                        break;
                    }
            }
            toAdd.Add(new Segment(minCoord.X, minCoord.Y, minCoord.X + rectangleSize.Width, minCoord.Y, Segment.Type.Top));
            toAdd.Add(new Segment(minCoord.X, minCoord.Y+rectangleSize.Height, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Bottom));
            toAdd.Add(new Segment(minCoord.X, minCoord.Y, minCoord.X, minCoord.Y + rectangleSize.Height, Segment.Type.Left));
            toAdd.Add(new Segment(minCoord.X+rectangleSize.Width, minCoord.Y, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Right));
            StackSegments(toAdd);
            if (minSegment == null)
                throw new ArgumentException("Place for appending not found");
            rectangles.Add(outRectangle);
            return outRectangle;
        }


        private void SearchMinDistanceTop(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize)
        {
            if (segment.start.X < center.X && segment.end.X > center.X)
            {
                Point midAngleCoord = new Point(center.X - rectangleSize.Width / 2, segment.start.Y - rectangleSize.Height);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }
            Point startAngleCoord = new Point(segment.start.X, segment.start.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(segment.end.X - rectangleSize.Width, segment.start.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private void SearchMinDistanceBottom(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize)
        {
            if (segment.start.X < center.X && segment.end.X > center.X)
            {
                Point midAngleCoord = new Point(center.X - rectangleSize.Width / 2, segment.start.Y);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }

            Point startAngleCoord = new Point(segment.start.X, segment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(segment.end.X - rectangleSize.Width, segment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private void SearchMinDistanceLeft(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize)
        {
            if (segment.start.Y < center.Y && segment.end.Y > center.Y)
            {
                Point midAngleCoord = new Point(segment.start.X - rectangleSize.Width, center.Y - rectangleSize.Height/2);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }


            Point startAngleCoord = new Point(segment.start.X - rectangleSize.Width, segment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(segment.end.X - rectangleSize.Width, segment.end.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private void SearchMinDistanceRight(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize)
        {
            if (segment.start.Y < center.Y && segment.end.Y > center.Y)
            {
                Point midAngleCoord = new Point(segment.start.X, center.Y - rectangleSize.Height / 2);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }

            Point startAngleCoord = new Point(segment.start.X, segment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(segment.end.X, segment.end.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private int FindLeftBorderX(Segment segment, Size rectangleSize)
        {
            var leftBorder = Segments
                            .Select(a => a)
                            .Where(b =>
                            b.type == Segment.Type.Right
                            && b.start.X < segment.start.X
                            && ((b.start.Y > segment.start.Y - rectangleSize.Height && b.start.Y < segment.start.Y)
                            || (b.end.Y > segment.start.Y - rectangleSize.Height && b.end.Y < segment.start.Y)
                            || (b.start.Y < segment.start.Y - rectangleSize.Height && b.end.Y >= segment.start.Y)))
                            .OrderBy(c => c.start.X)
                            .FirstOrDefault();
            int leftBorderX;
            if (leftBorder == null)
                leftBorderX = int.MinValue;
            else
                leftBorderX = leftBorder.start.X;
            return leftBorderX;
        }

        private int FindRightBorderX(Segment segment, Size rectangleSize)
        {
            var rightBorder = Segments
                            .Select(a => a)
                            .Where(b =>
                            b.type == Segment.Type.Left
                            && b.start.X > segment.end.X
                            && ((b.start.Y > segment.end.Y - rectangleSize.Height && b.start.Y < segment.end.Y)
                            || (b.end.Y > segment.end.Y - rectangleSize.Height && b.end.Y < segment.end.Y)
                            || (b.start.Y < segment.end.Y - rectangleSize.Height && b.end.Y >= segment.start.Y)))
                            .OrderByDescending(c => c.start.X)
                            .FirstOrDefault();
            int rightBorderX;
            if (rightBorder == null)
                rightBorderX = int.MaxValue;
            else
                rightBorderX = rightBorder.start.X;
            return rightBorderX;
        }

        private int FindTopBorderY(Segment segment, Size rectangleSize)
        {
            var topBorder = Segments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Bottom
                && b.start.Y < segment.start.Y
                && ((b.start.X > segment.start.X - rectangleSize.Width && b.start.X < segment.start.X)
                            || (b.end.X > segment.start.X - rectangleSize.Width && b.end.X < segment.start.X)
                            || (b.start.X < segment.start.X - rectangleSize.Width && b.end.X >= segment.start.X)))
                .OrderByDescending(c => c.start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = int.MinValue;
            else
                topBorderY = topBorder.start.Y;
            return topBorderY;
        }

        private int FindBottomBorderY(Segment segment, Size rectangleSize)
        {
            var topBorder = Segments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Top
                && b.start.Y > segment.start.Y
                && ((b.start.X > segment.start.X - rectangleSize.Width && b.start.X < segment.start.X)
                            || (b.end.X > segment.start.X - rectangleSize.Width && b.end.X < segment.start.X)
                            || (b.start.X < segment.start.X - rectangleSize.Width && b.end.X >= segment.start.X)))
                .OrderBy(c => c.start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = int.MaxValue;
            else
                topBorderY = topBorder.start.Y;
            return topBorderY;
        }

        private void CheckDistance(ref Segment minSegment, ref double minDistance, Segment segment, ref Point minCoord, Point coord, Size rectangleSize)
        {
            double dist = Distance(GetRectangleCenter(new Rectangle(coord, rectangleSize)), center);
            if (dist < minDistance)
            {
                minDistance = dist;
                minSegment = segment;
                minCoord = coord;
            }
        }


        private Point GetRectangleCenter(Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                     rect.Top + rect.Height / 2);
        }

        private double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - center.Y), 2));
        }

        private void StackSegments(List<Segment> added)//экспериментальный метод, возможно работает косячно
        {
            var toDelete = new HashSet<Segment>();
            var toAdd = new HashSet<Segment>();
            Segments.UnionWith(added);//добавляем все элементы, какие не нужны - удалим в процессе проверки
            foreach (var segment1 in added)
            {
                foreach (var segment2 in Segments)
                {
                    if (segment1.end == segment2.start && segment1.type == segment2.type)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment1.start, segment2.end, segment1.type));
                    }
                    if (segment2.end == segment1.start && segment1.type == segment2.type)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment2.start, segment1.end, segment1.type));
                    }
                    if (segment1.start == segment2.start
                        && segment1.end == segment2.end
                        && ((segment1.type == Segment.Type.Top && segment2.type == Segment.Type.Bottom)
                        || (segment1.type == Segment.Type.Bottom && segment2.type == Segment.Type.Top)
                        || (segment1.type == Segment.Type.Left && segment2.type == Segment.Type.Right)
                        || (segment1.type == Segment.Type.Right && segment2.type == Segment.Type.Left)))
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                    }
                    if (((segment1.type == Segment.Type.Right && segment2.type == Segment.Type.Left)
                        || (segment1.type == Segment.Type.Left && segment2.type == Segment.Type.Right))
                        && segment1.start.X == segment2.start.X
                        && segment1.start.Y <= segment2.start.Y
                        && segment1.end.Y >= segment2.end.Y)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment1.start, segment2.start, segment1.type));
                        toAdd.Add(new Segment(segment2.end, segment1.end, segment1.type));
                    }
                    if (((segment1.type == Segment.Type.Right && segment2.type == Segment.Type.Left)
                        || (segment1.type == Segment.Type.Left && segment2.type == Segment.Type.Right))
                        && segment1.start.X == segment2.start.X
                        && segment2.start.Y <= segment1.start.Y
                        && segment2.end.Y >= segment1.end.Y)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment2.start, segment1.start, segment2.type));
                        toAdd.Add(new Segment(segment1.end, segment2.end, segment2.type));
                    }
                    if (((segment1.type == Segment.Type.Top && segment2.type == Segment.Type.Bottom)
                        || (segment1.type == Segment.Type.Bottom && segment2.type == Segment.Type.Top))
                        && segment1.start.Y == segment2.start.Y
                        && segment1.start.X <= segment2.start.X
                        && segment1.end.X >= segment2.end.X)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment1.start, segment2.start, segment1.type));
                        toAdd.Add(new Segment(segment2.end, segment1.end, segment1.type));
                    }
                    if (((segment1.type == Segment.Type.Top && segment2.type == Segment.Type.Bottom)
                        || (segment1.type == Segment.Type.Bottom && segment2.type == Segment.Type.Top))
                        && segment1.start.Y == segment2.start.Y
                        && segment2.start.X <= segment1.start.X
                        && segment2.end.X >= segment1.end.X)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment2.start, segment1.start, segment2.type));
                        toAdd.Add(new Segment(segment1.end, segment2.end, segment2.type));
                    }
                }
            }
            Segments.UnionWith(toAdd);
            Segments.ExceptWith(toDelete);
            Segments.RemoveWhere(a => a.Length == 0);
        }

    }
}
