using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{

    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point center;
        private readonly HashSet<Segment> Segments;
        private readonly HashSet<Segment> ProbablyBuggedSegments;
        private bool isFirstRectangle;
        private readonly List<Rectangle> rectangles;
        private readonly bool safeMode = true;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            this.Segments = new HashSet<Segment>();
            this.ProbablyBuggedSegments = new HashSet<Segment>();
            this.isFirstRectangle = true;
            this.rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (isFirstRectangle)
            {
                isFirstRectangle = false;
                InitializeFirstRectangle(rectangleSize);
                return new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize);
            }
            double minDistance = double.MaxValue;
            Segment minSegment = null;
            Point minCoord = new Point();
            foreach (var segment in Segments.Except(ProbablyBuggedSegments))
            {
                if (segment.type == Segment.Type.Top)
                {
                    var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                    var rightBorderX = FindRightBorderX(segment, rectangleSize);
                    var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                    var topBorderY = FindTopBorderY(new Segment(updatedSegment.start, updatedSegment.start, Segment.Type.Right), new Size((int)updatedSegment.Length, 1));
                    if (topBorderY > segment.start.Y - rectangleSize.Height)
                        continue;
                    SearchMinDistanceTop(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize, updatedSegment);
                }
                if (segment.type == Segment.Type.Bottom)
                {
                    var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                    var rightBorderX = FindRightBorderX(segment, rectangleSize);
                    var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                    var bottomBorderY = FindBottomBorderY(new Segment(updatedSegment.start, updatedSegment.start, Segment.Type.Right), new Size((int)updatedSegment.Length, 1));
                    if (bottomBorderY < segment.start.Y + rectangleSize.Height)//здесь нужно быть повнимательнее с типом фейкового сегмента
                        continue;                                              //возможно стоит сделать с обоими типами и выбрать минимум
                    SearchMinDistanceBottom(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize, updatedSegment);
                }
                if (segment.type == Segment.Type.Left)
                {
                    var topBorderY = FindTopBorderY(segment, rectangleSize);
                    var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                    var leftBorderX = FindLeftBorderX(new Segment(segment.start, segment.start, Segment.Type.Top), new Size(1, (int)segment.Length));
                    if (leftBorderX > segment.start.X - rectangleSize.Width)//здесь нужно быть повнимательнее с типом фейкового сегмента
                        continue;
                    var updatedSegment = new Segment(segment.start.X, topBorderY, segment.end.X, bottomBorderY, segment.type);
                    SearchMinDistanceLeft(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize, updatedSegment);
                }
                if (segment.type == Segment.Type.Right)
                {
                    var topBorderY = FindTopBorderY(segment, rectangleSize);
                    var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                    var rightBorderX = FindRightBorderX(new Segment(segment.start, segment.start, Segment.Type.Top), new Size(1, (int)segment.Length));
                    if (rightBorderX < segment.start.X + rectangleSize.Width)
                        continue;
                    var updatedSegment = new Segment(segment.start.X, topBorderY, segment.end.X, bottomBorderY, segment.type);
                    SearchMinDistanceRight(ref minSegment, ref minDistance, ref minCoord, segment, rectangleSize, updatedSegment);
                }
            }
            var outRectangle = new Rectangle(minCoord, rectangleSize);
            var toAdd = new List<Segment>();
            if (safeMode)
            {
                foreach (var rect in rectangles)
                    if (rect.IntersectsWith(outRectangle))
                    {
                        ProbablyBuggedSegments.Add(minSegment);//полагаем что тот сегмент к которому пытаемся присоединиться багованный
                        if (Segments.Except(ProbablyBuggedSegments).Count()==0)
                        {//здесь должен быть выход из рекурсии, можно приделать прямоугольник в самую верхнюю точку
                            var topBorder = Segments
                                .Select(a => a.start.Y)
                                .OrderBy(a => a)
                                .First();
                            var botBorder = Segments
                                    .Select(a => a.start.Y)
                                    .OrderByDescending(a => a)
                                    .First();
                            var leftBorder = Segments
                                    .Select(a => a.start.X)
                                    .OrderByDescending(a => a)
                                    .First();
                            var rightBorder = Segments
                                    .Select(a => a.start.X)
                                    .OrderBy(a => a)
                                    .First();
                            var minDist = new[] { Math.Abs(topBorder), Math.Abs(botBorder), Math.Abs(leftBorder), Math.Abs(rightBorder) }.Min();
                            if (minDist == Math.Abs(topBorder))
                                minCoord = new Point(center.X - rectangleSize.Width / 2, topBorder - rectangleSize.Height);
                            if (minDist == Math.Abs(botBorder))
                                minCoord = new Point(center.X - rectangleSize.Width / 2, botBorder);
                            if (minDist == Math.Abs(leftBorder))
                                minCoord = new Point(center.Y - rectangleSize.Height / 2, leftBorder - rectangleSize.Width);
                            if (minDist == Math.Abs(rightBorder))
                                minCoord = new Point(center.Y - rectangleSize.Height / 2, rightBorder);
                            outRectangle = new Rectangle(minCoord, rectangleSize);
                            break;
                        }
                        return PutNextRectangle(rectangleSize);
                    }
            }
            else
            {
                if (minSegment == null)
                    throw new ArgumentException("Place for appending not found");
            }
            toAdd.Add(new Segment(minCoord.X, minCoord.Y, minCoord.X + rectangleSize.Width, minCoord.Y, Segment.Type.Top));
            toAdd.Add(new Segment(minCoord.X, minCoord.Y + rectangleSize.Height, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Bottom));
            toAdd.Add(new Segment(minCoord.X, minCoord.Y, minCoord.X, minCoord.Y + rectangleSize.Height, Segment.Type.Left));
            toAdd.Add(new Segment(minCoord.X + rectangleSize.Width, minCoord.Y, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Right));
            StackSegments(toAdd);
            rectangles.Add(outRectangle);
            return outRectangle;
        }


        private void SearchMinDistanceTop(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize, Segment extendedSegment)
        {
            if (extendedSegment.Length < rectangleSize.Width)
                return;
            if (extendedSegment.start.X < center.X && extendedSegment.end.X > center.X)
            {
                Point midAngleCoord = new Point(center.X - rectangleSize.Width / 2, extendedSegment.start.Y - rectangleSize.Height);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }
            Point startAngleCoord = new Point(extendedSegment.start.X, extendedSegment.start.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(extendedSegment.end.X - rectangleSize.Width, extendedSegment.start.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private void SearchMinDistanceBottom(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize, Segment extendedSegment)
        {
            if (extendedSegment.Length < rectangleSize.Width)
                return;
            if (extendedSegment.start.X < center.X && extendedSegment.end.X > center.X)
            {
                Point midAngleCoord = new Point(center.X - rectangleSize.Width / 2, extendedSegment.start.Y);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }

            Point startAngleCoord = new Point(extendedSegment.start.X, extendedSegment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(extendedSegment.end.X - rectangleSize.Width, extendedSegment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private void SearchMinDistanceLeft(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize, Segment extendedSegment)
        {
            if (extendedSegment.Length < rectangleSize.Height)
                return;
            if (extendedSegment.start.Y < center.Y && extendedSegment.end.Y > center.Y)
            {
                Point midAngleCoord = new Point(extendedSegment.start.X - rectangleSize.Width, center.Y - rectangleSize.Height / 2);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }
            Point startAngleCoord = new Point(extendedSegment.start.X - rectangleSize.Width, extendedSegment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(extendedSegment.end.X - rectangleSize.Width, extendedSegment.end.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private void SearchMinDistanceRight(ref Segment minSegment, ref double minDistance, ref Point minCoord, Segment segment, Size rectangleSize, Segment extendedSegment)
        {
            if (extendedSegment.Length < rectangleSize.Height)
                return;
            if (extendedSegment.start.Y < center.Y && extendedSegment.end.Y > center.Y)
            {
                Point midAngleCoord = new Point(extendedSegment.start.X, center.Y - rectangleSize.Height / 2);
                CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, midAngleCoord, rectangleSize);
            }

            Point startAngleCoord = new Point(extendedSegment.start.X, extendedSegment.start.Y);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, startAngleCoord, rectangleSize);
            Point endAngleCoord = new Point(extendedSegment.end.X, extendedSegment.end.Y - rectangleSize.Height);
            CheckDistance(ref minSegment, ref minDistance, segment, ref minCoord, endAngleCoord, rectangleSize);
        }

        private int FindLeftBorderX(Segment segment, Size rectangleSize)
        {
            int topRectSide;
            if (segment.type == Segment.Type.Top)
                topRectSide = segment.end.Y - rectangleSize.Height;
            else
                topRectSide = segment.end.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var leftBorder = Segments
                            .Select(a => a)
                            .Where(b =>
                            b.type == Segment.Type.Right
                            && b.start.X <= segment.start.X
                            && ((b.start.Y >= topRectSide && b.start.Y < bottomRectSide)
                            || (b.end.Y > topRectSide && b.end.Y <= bottomRectSide)
                            || (b.start.Y <= topRectSide && b.end.Y >= bottomRectSide
                            || (b.start.Y >= topRectSide && b.end.Y <= bottomRectSide))))
                            .OrderByDescending(c => c.start.X)
                            .FirstOrDefault();
            int leftBorderX;
            if (leftBorder == null)
                leftBorderX = -100000;
            else
                leftBorderX = leftBorder.start.X;
            return leftBorderX;
        }

        private int FindRightBorderX(Segment segment, Size rectangleSize)
        {
            int topRectSide;
            if (segment.type == Segment.Type.Top)
                topRectSide = segment.end.Y - rectangleSize.Height;
            else
                topRectSide = segment.end.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var rightBorder = Segments
                            .Select(a => a)
                            .Where(b =>
                            b.type == Segment.Type.Left
                            && b.start.X >= segment.end.X
                            && ((b.start.Y >= topRectSide && b.start.Y < bottomRectSide)
                            || (b.end.Y > topRectSide && b.end.Y <= bottomRectSide)
                            || (b.start.Y <= topRectSide && b.end.Y >= bottomRectSide
                            || (b.start.Y >= topRectSide && b.end.Y <= bottomRectSide))))
                            .OrderBy(c => c.start.X)
                            .FirstOrDefault();
            int rightBorderX;
            if (rightBorder == null)
                rightBorderX = 100000;
            else
                rightBorderX = rightBorder.start.X;
            return rightBorderX;
        }

        private int FindTopBorderY(Segment segment, Size rectangleSize)
        {
            int leftRectSide;
            if (segment.type == Segment.Type.Left)
                leftRectSide = segment.end.X - rectangleSize.Width;
            else
                leftRectSide = segment.end.X;
            int rightRectSide = leftRectSide + rectangleSize.Width;
            var topBorder = Segments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Bottom
                && b.start.Y <= segment.start.Y
                && ((b.start.X >= leftRectSide && b.start.X < rightRectSide)
                            || (b.end.X > leftRectSide && b.end.X <= rightRectSide)
                            || (b.start.X <= leftRectSide && b.end.X >= rightRectSide)))
                .OrderByDescending(c => c.start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = -100000;
            else
                topBorderY = topBorder.start.Y;
            return topBorderY;
        }

        private int FindBottomBorderY(Segment segment, Size rectangleSize)
        {
            int leftRectSide;
            if (segment.type == Segment.Type.Left)
                leftRectSide = segment.end.X - rectangleSize.Width;
            else
                leftRectSide = segment.end.X;
            int rightRectSide = leftRectSide + rectangleSize.Width;
            var topBorder = Segments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Top
                && b.end.Y >= segment.end.Y
                && ((b.start.X >= leftRectSide && b.start.X < rightRectSide)
                            || (b.end.X > leftRectSide && b.end.X <= rightRectSide)
                            || (b.start.X <= leftRectSide && b.end.X >= rightRectSide)))
                .OrderBy(c => c.start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = 100000;
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
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
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
                    if (segment1.start == segment2.start// здесь возможно не нужны такие проверки
                        && segment1.end == segment2.end
                        && segment1.Opposite(segment2))
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                    }
                    if (segment1.Opposite(segment2)
                        && !segment1.Horizontal()
                        && segment1.start.X == segment2.start.X
                        && segment1.start.Y <= segment2.start.Y
                        && segment1.end.Y >= segment2.end.Y)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment1.start, segment2.start, segment1.type));
                        toAdd.Add(new Segment(segment2.end, segment1.end, segment1.type));
                    }
                    if (segment1.Opposite(segment2)
                        && !segment1.Horizontal()
                        && segment1.start.X == segment2.start.X
                        && segment2.start.Y <= segment1.start.Y
                        && segment2.end.Y >= segment1.end.Y)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment2.start, segment1.start, segment2.type));
                        toAdd.Add(new Segment(segment1.end, segment2.end, segment2.type));
                    }
                    if (segment1.Opposite(segment2)
                        && segment1.Horizontal()
                        && segment1.start.Y == segment2.start.Y
                        && segment1.start.X <= segment2.start.X
                        && segment1.end.X >= segment2.end.X)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment1.start, segment2.start, segment1.type));
                        toAdd.Add(new Segment(segment2.end, segment1.end, segment1.type));
                    }
                    if (segment1.Opposite(segment2)
                        && segment1.Horizontal()
                        && segment1.start.Y == segment2.start.Y
                        && segment2.start.X <= segment1.start.X
                        && segment2.end.X >= segment1.end.X)
                    {
                        toDelete.Add(segment1);
                        toDelete.Add(segment2);
                        toAdd.Add(new Segment(segment2.start, segment1.start, segment2.type));
                        toAdd.Add(new Segment(segment1.end, segment2.end, segment2.type));
                    }
                    StackCheckPartialCrossingHorizontal(segment1, segment2, ref toDelete, ref toAdd);
                    StackCheckPartialCrossingVertical(segment1, segment2, ref toDelete, ref toAdd);
                }
            }
            Segments.UnionWith(toAdd);
            Segments.ExceptWith(toDelete);
            Segments.RemoveWhere(a => a.Length == 0);
        }


        private void StackCheckPartialCrossingHorizontal(Segment segment1, Segment segment2, ref HashSet<Segment> toDelete, ref HashSet<Segment> toAdd)
        {
            if (segment1.type == Segment.Type.Bottom// еще нужна перестановка top/bottom и самих элементов
                && segment1.type == Segment.Type.Top
                && segment1.start.X < segment2.start.X
                && segment1.end.X > segment2.start.X
                && segment1.end.X < segment2.end.X)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment1.start, segment2.start, Segment.Type.Bottom));
                toAdd.Add(new Segment(segment1.end, segment2.end, Segment.Type.Top));
            }
            if (segment1.type == Segment.Type.Top
                && segment1.type == Segment.Type.Bottom
                && segment1.start.X < segment2.start.X
                && segment1.end.X > segment2.start.X
                && segment1.end.X < segment2.end.X)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment1.start, segment2.start, Segment.Type.Top));
                toAdd.Add(new Segment(segment1.end, segment2.end, Segment.Type.Bottom));
            }
            if (segment1.type == Segment.Type.Bottom
                && segment1.type == Segment.Type.Top
                && segment2.start.X < segment1.start.X
                && segment2.end.X > segment1.start.X
                && segment2.end.X < segment1.end.X)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment2.start, segment1.start, Segment.Type.Bottom));
                toAdd.Add(new Segment(segment2.end, segment1.end, Segment.Type.Top));
            }
            if (segment1.type == Segment.Type.Top
                && segment1.type == Segment.Type.Bottom
                && segment2.start.X < segment1.start.X
                && segment2.end.X > segment1.start.X
                && segment2.end.X < segment1.end.X)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment2.start, segment1.start, Segment.Type.Top));
                toAdd.Add(new Segment(segment2.end, segment1.end, Segment.Type.Bottom));
            }
        }

        private void StackCheckPartialCrossingVertical(Segment segment1, Segment segment2, ref HashSet<Segment> toDelete, ref HashSet<Segment> toAdd)
        {
            if (segment1.type == Segment.Type.Left
                && segment2.type == Segment.Type.Right
                && segment1.start.X == segment2.start.X
                && segment1.start.Y < segment2.start.Y
                && segment1.end.Y > segment2.start.Y
                && segment1.end.Y < segment2.end.Y)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment1.start, segment2.start, Segment.Type.Left));
                toAdd.Add(new Segment(segment1.end, segment2.end, Segment.Type.Right));
            }
            if (segment1.type == Segment.Type.Right
                && segment2.type == Segment.Type.Left
                && segment1.start.X == segment2.start.X
                && segment1.start.Y < segment2.start.Y
                && segment1.end.Y > segment2.start.Y
                && segment1.end.Y < segment2.end.Y)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment1.start, segment2.start, Segment.Type.Right));
                toAdd.Add(new Segment(segment1.end, segment2.end, Segment.Type.Left));
            }
            if (segment1.type == Segment.Type.Left
                && segment2.type == Segment.Type.Right
                && segment1.start.X == segment2.start.X
                && segment2.start.Y < segment1.start.Y
                && segment2.end.Y > segment1.start.Y
                && segment2.end.Y < segment1.end.Y)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment2.start, segment1.start, Segment.Type.Right));
                toAdd.Add(new Segment(segment2.end, segment1.end, Segment.Type.Left));
            }
            if (segment1.type == Segment.Type.Right
                && segment2.type == Segment.Type.Left
                && segment1.start.X == segment2.start.X
                && segment2.start.Y < segment1.start.Y
                && segment2.end.Y > segment1.start.Y
                && segment2.end.Y < segment1.end.Y)
            {
                toDelete.Add(segment1);
                toDelete.Add(segment2);
                toAdd.Add(new Segment(segment2.start, segment1.start, Segment.Type.Left));
                toAdd.Add(new Segment(segment2.end, segment1.end, Segment.Type.Right));
            }
        }
        private void InitializeFirstRectangle(Size rectangleSize)
        {
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
        }
    }

}
