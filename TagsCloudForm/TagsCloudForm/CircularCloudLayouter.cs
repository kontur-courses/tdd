using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{

    public class PositionSearchResult
    {
        public double MinDistance { get; private set; }
        public Segment MinSegment { get; private set; }
        public Point ClosestRectCoord { get; private set; }
        public PositionSearchResult(double minDistance, Segment minSegment, Point closestRectCoord)
        {
            this.MinDistance = minDistance;
            this.MinSegment = minSegment;
            this.ClosestRectCoord = closestRectCoord;
        }
        public void Update(double minDistance, Segment minSegment, Point closestRectCoord)
        {
            if (minDistance<this.MinDistance)
            {
                this.MinDistance = minDistance;
                this.MinSegment = minSegment;
                this.ClosestRectCoord = closestRectCoord;
            }
        }
    }

    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point CloudCenter;
        private readonly HashSet<Segment> BorderSegments;
        private readonly HashSet<Segment> ProbablyBuggedSegments;
        private bool isFirstRectangle;
        private readonly List<Rectangle> addedRectangles;
        private readonly bool safeMode;
        public CircularCloudLayouter(Point center)
        {
            CloudCenter = center;
            BorderSegments = new HashSet<Segment>();
            ProbablyBuggedSegments = new HashSet<Segment>();
            isFirstRectangle = true;
            addedRectangles = new List<Rectangle>();
            safeMode = false;
        }

        public CircularCloudLayouter(Point center, bool mode)
        {
            CloudCenter = center;
            BorderSegments = new HashSet<Segment>();
            ProbablyBuggedSegments = new HashSet<Segment>();
            isFirstRectangle = true;
            addedRectangles = new List<Rectangle>();
            safeMode = mode;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (isFirstRectangle)
            {
                isFirstRectangle = false;
                InitializeFirstRectangle(rectangleSize);
                var firstRect = new Rectangle(new Point(CloudCenter.X - (int)Math.Floor(rectangleSize.Width / (double)2), CloudCenter.Y - (int)Math.Floor(rectangleSize.Height / (double)2)), rectangleSize);
                return firstRect;
            }
            var searchResult = new PositionSearchResult(double.MaxValue, null, new Point());
            foreach (var segment in BorderSegments.Except(ProbablyBuggedSegments))
            {
                if (segment.type == Segment.Type.Top)
                {
                    var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                    var rightBorderX = FindRightBorderX(segment, rectangleSize);
                    var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                    SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
                if (segment.type == Segment.Type.Bottom)
                {
                    var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                    var rightBorderX = FindRightBorderX(segment, rectangleSize);
                    var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                    SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
                if (segment.type == Segment.Type.Left)
                {
                    var topBorderY = FindTopBorderY(segment, rectangleSize);
                    var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                    var updatedSegment = new Segment(segment.start.X, topBorderY, segment.end.X, bottomBorderY, segment.type);
                    SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
                if (segment.type == Segment.Type.Right)
                {
                    var topBorderY = FindTopBorderY(segment, rectangleSize);
                    var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                    var updatedSegment = new Segment(segment.start.X, topBorderY, segment.end.X, bottomBorderY, segment.type);
                    SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
            }
            var outRectangle = new Rectangle(searchResult.ClosestRectCoord, rectangleSize);
            var toAdd = new List<Segment>();
            if (safeMode)
            {
                foreach (var rect in addedRectangles)
                    if (rect.IntersectsWith(outRectangle))
                    {
                        ProbablyBuggedSegments.Add(searchResult.MinSegment);//полагаем что тот сегмент к которому пытаемся присоединиться багованный
                        if (BorderSegments.Except(ProbablyBuggedSegments).Count()==0)
                        {//здесь должен быть выход из рекурсии, можно приделать прямоугольник в самую крайнюю точку
                            var closestRectCoord = FindClosestCloudBorder(rectangleSize);
                            outRectangle = new Rectangle(closestRectCoord, rectangleSize);
                            break;
                        }
                        return PutNextRectangle(rectangleSize);
                    }
            }
            else
            {
                if (searchResult.MinSegment == null)
                    throw new ArgumentException("Place for appending not found");
            }
            toAdd.Add(new Segment(searchResult.ClosestRectCoord.X, searchResult.ClosestRectCoord.Y, searchResult.ClosestRectCoord.X + rectangleSize.Width, searchResult.ClosestRectCoord.Y, Segment.Type.Top));
            toAdd.Add(new Segment(searchResult.ClosestRectCoord.X, searchResult.ClosestRectCoord.Y + rectangleSize.Height, searchResult.ClosestRectCoord.X + rectangleSize.Width, searchResult.ClosestRectCoord.Y + rectangleSize.Height, Segment.Type.Bottom));
            toAdd.Add(new Segment(searchResult.ClosestRectCoord.X, searchResult.ClosestRectCoord.Y, searchResult.ClosestRectCoord.X, searchResult.ClosestRectCoord.Y + rectangleSize.Height, Segment.Type.Left));
            toAdd.Add(new Segment(searchResult.ClosestRectCoord.X + rectangleSize.Width, searchResult.ClosestRectCoord.Y, searchResult.ClosestRectCoord.X + rectangleSize.Width, searchResult.ClosestRectCoord.Y + rectangleSize.Height, Segment.Type.Right));
            StackSegments(toAdd);
            addedRectangles.Add(outRectangle);
            return outRectangle;
        }


        private void SearchMinDistance(PositionSearchResult searchResult, Segment segment, Size rectangleSize, Segment extendedSegment)
        {
            if (segment.Horizontal())
            {
                if (extendedSegment.Length < rectangleSize.Width)
                    return;
                if (extendedSegment.start.X < CloudCenter.X
                && extendedSegment.end.X > CloudCenter.X
                && CloudCenter.X + (int)Math.Truncate(rectangleSize.Width / (double)2) + 1 <= extendedSegment.end.X
                && CloudCenter.X - (int)Math.Truncate(rectangleSize.Width / (double)2) - 1 >= extendedSegment.start.X)
                {
                    Point midRectCoordinates;
                    if (segment.type==Segment.Type.Top)
                        midRectCoordinates = new Point(CloudCenter.X - (int)Math.Truncate(rectangleSize.Width / (double)2), extendedSegment.start.Y - rectangleSize.Height);
                    else
                        midRectCoordinates = new Point(CloudCenter.X - (int)Math.Truncate(rectangleSize.Width / (double)2), extendedSegment.start.Y);
                    if (CheckOppositeBorder(midRectCoordinates, rectangleSize, Segment.Type.Top))
                        CheckDistance(searchResult, segment, midRectCoordinates, rectangleSize);
                }
                Point leftMostRectCoordinates;
                Point rightMostRectCoordinates;
                if (segment.type == Segment.Type.Top)
                {
                    leftMostRectCoordinates = new Point(extendedSegment.start.X, extendedSegment.start.Y - rectangleSize.Height);
                    rightMostRectCoordinates = new Point(extendedSegment.end.X - rectangleSize.Width, extendedSegment.start.Y - rectangleSize.Height);
                }
                else
                {
                    leftMostRectCoordinates = new Point(extendedSegment.start.X, extendedSegment.start.Y);
                    rightMostRectCoordinates = new Point(extendedSegment.end.X - rectangleSize.Width, extendedSegment.start.Y);
                }
                if (CheckOppositeBorder(leftMostRectCoordinates, rectangleSize, Segment.Type.Top))
                    CheckDistance(searchResult, segment, leftMostRectCoordinates, rectangleSize);
                if (CheckOppositeBorder(rightMostRectCoordinates, rectangleSize, Segment.Type.Top))
                    CheckDistance(searchResult, segment, rightMostRectCoordinates, rectangleSize);
            }
            else
            {
                if (extendedSegment.Length < rectangleSize.Height)
                    return;
                if (extendedSegment.start.Y < CloudCenter.Y
                && extendedSegment.end.Y > CloudCenter.Y
                && CloudCenter.Y + (int)Math.Truncate(rectangleSize.Height / (double)2) + 1 <= extendedSegment.end.Y
                && CloudCenter.Y - (int)Math.Truncate(rectangleSize.Height / (double)2) - 1 >= extendedSegment.start.Y)
                {
                    Point midRectCoordinates;
                    if (segment.type==Segment.Type.Left)
                        midRectCoordinates = new Point(extendedSegment.start.X - rectangleSize.Width, CloudCenter.Y - (int)Math.Truncate(rectangleSize.Height / (double)2));
                    else
                        midRectCoordinates = new Point(extendedSegment.start.X, CloudCenter.Y - (int)Math.Truncate(rectangleSize.Height / (double)2));
                    if (CheckOppositeBorder(midRectCoordinates, rectangleSize, Segment.Type.Right))
                        CheckDistance(searchResult, segment, midRectCoordinates, rectangleSize);
                }
                Point topMostRectCoordinates;
                Point botMostRectcoordinates;
                if (segment.type==Segment.Type.Left)
                {
                    topMostRectCoordinates = new Point(extendedSegment.start.X - rectangleSize.Width, extendedSegment.start.Y);
                    botMostRectcoordinates = new Point(extendedSegment.end.X - rectangleSize.Width, extendedSegment.end.Y - rectangleSize.Height);
                }
                else
                {
                    topMostRectCoordinates = new Point(extendedSegment.start.X, extendedSegment.start.Y);
                    botMostRectcoordinates = new Point(extendedSegment.end.X, extendedSegment.end.Y - rectangleSize.Height);
                }
                if (CheckOppositeBorder(topMostRectCoordinates, rectangleSize, Segment.Type.Right))
                    CheckDistance(searchResult, segment, topMostRectCoordinates, rectangleSize);
                if (CheckOppositeBorder(botMostRectcoordinates, rectangleSize, Segment.Type.Right))
                    CheckDistance(searchResult, segment, botMostRectcoordinates, rectangleSize);
            }

        }


        private bool CheckOppositeBorder(Point rectanglePos, Size rectangleSize, Segment.Type segmentType)
        {
            if (segmentType == Segment.Type.Top)
            {
                var TopBorderY = FindTopBorderY(
                    new Segment(
                        new Point(rectanglePos.X, rectanglePos.Y + rectangleSize.Height-1)
                        , new Point(rectanglePos.X, rectanglePos.Y + rectangleSize.Height)
                        , Segment.Type.Right)
                    , new Size(rectangleSize.Width, 1));
                if (TopBorderY <= rectanglePos.Y)
                    return true;
                return false;
            }
            if (segmentType == Segment.Type.Bottom)
            {
                var BotBorderY = FindBottomBorderY(
                    new Segment(
                        new Point(rectanglePos.X, rectanglePos.Y)
                        , new Point(rectanglePos.X, rectanglePos.Y + 1)
                        , Segment.Type.Right)
                    , new Size(rectangleSize.Width, 1));
                if (BotBorderY >= rectanglePos.Y + rectangleSize.Height)
                    return true;
                return false;
            }
            if (segmentType == Segment.Type.Left)
            {
                var LeftBorderX = FindLeftBorderX(
                    new Segment(
                        new Point(rectanglePos.X+rectangleSize.Width-1, rectanglePos.Y+rectangleSize.Height)
                        , new Point(rectanglePos.X + rectangleSize.Width, rectanglePos.Y + rectangleSize.Height)
                        , Segment.Type.Top)
                    , new Size(1, rectangleSize.Height));
                if (LeftBorderX <= rectanglePos.X)
                    return true;
                return false;
            }
            if (segmentType == Segment.Type.Right)
            {
                var RightBorderX = FindRightBorderX(
                    new Segment(
                        new Point(rectanglePos.X, rectanglePos.Y + rectangleSize.Height)
                        , new Point(rectanglePos.X + 1, rectanglePos.Y + rectangleSize.Height)
                        , Segment.Type.Top)
                    , new Size(1, rectangleSize.Height));
                if (RightBorderX >= rectanglePos.X + rectangleSize.Width)
                    return true;
                return false;
            }
            return false;
        }

        private int FindLeftBorderX(Segment segment, Size rectangleSize)
        {
            int topRectSide;
            if (segment.type == Segment.Type.Top)
                topRectSide = segment.end.Y - rectangleSize.Height;
            else
                topRectSide = segment.end.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var leftBorder = BorderSegments
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
            var rightBorder = BorderSegments
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
            var topBorder = BorderSegments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Bottom
                && b.start.Y <= segment.start.Y
                && ((b.start.X >= leftRectSide && b.start.X < rightRectSide)
                            || (b.end.X > leftRectSide && b.end.X <= rightRectSide)
                            || (b.start.X <= leftRectSide && b.end.X >= rightRectSide)
                            || (b.start.X >= leftRectSide && b.end.X <= rightRectSide)))
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
            var topBorder = BorderSegments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Top
                && b.end.Y >= segment.end.Y
                && ((b.start.X >= leftRectSide && b.start.X < rightRectSide)
                            || (b.end.X > leftRectSide && b.end.X <= rightRectSide)
                            || (b.start.X <= leftRectSide && b.end.X >= rightRectSide)
                            || (b.start.X >= leftRectSide && b.end.X <= rightRectSide)))
                .OrderBy(c => c.start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = 100000;
            else
                topBorderY = topBorder.start.Y;
            return topBorderY;
        }


        private void CheckDistance(PositionSearchResult currentSearchRes, Segment segment, Point coord, Size rectangleSize)
        {
            var dist = Distance(GetRectangleCenter(new Rectangle(coord, rectangleSize)), CloudCenter);
            if (dist < currentSearchRes.MinDistance)
            {
                currentSearchRes.Update(dist, segment, coord);
            }
        }


        private Point GetRectangleCenter(Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                     rect.Top + rect.Height / 2);
        }

        private double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private void StackSegments(List<Segment> added)
        {
            var toDelete = new HashSet<Segment>();
            var toAdd = new HashSet<Segment>();
            BorderSegments.UnionWith(added);//добавляем все элементы, какие не нужны - удалим в процессе проверки
            foreach (var segment1 in added)
            {
                foreach (var segment2 in BorderSegments)
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
            BorderSegments.UnionWith(toAdd);
            BorderSegments.ExceptWith(toDelete);
            BorderSegments.RemoveWhere(a => a.Length == 0);
        }


        private void StackCheckPartialCrossingHorizontal(Segment segment1, Segment segment2, ref HashSet<Segment> toDelete, ref HashSet<Segment> toAdd)
        {
            if (segment1.type == Segment.Type.Bottom
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
            var firstRectCoord = new Point(
                    CloudCenter.X - (int)Math.Floor(rectangleSize.Width / (double)2)
                    , CloudCenter.Y - (int)Math.Floor(rectangleSize.Height / (double)2));
            BorderSegments.Add(new Segment(
                    firstRectCoord.X
                    , firstRectCoord.Y
                    , firstRectCoord.X+rectangleSize.Width
                    , firstRectCoord.Y
                    , Segment.Type.Top));
            BorderSegments.Add(new Segment(
                firstRectCoord.X 
                , firstRectCoord.Y + rectangleSize.Height
                , firstRectCoord.X + rectangleSize.Width
                , firstRectCoord.Y + rectangleSize.Height
                , Segment.Type.Bottom));
            BorderSegments.Add(new Segment(
                firstRectCoord.X
                , firstRectCoord.Y
                , firstRectCoord.X
                , firstRectCoord.Y+rectangleSize.Height
                , Segment.Type.Left));
            BorderSegments.Add(new Segment(
                firstRectCoord.X + rectangleSize.Width
                , firstRectCoord.Y
                , firstRectCoord.X + rectangleSize.Width
                , firstRectCoord.Y + rectangleSize.Height
                , Segment.Type.Right));
            addedRectangles.Add(new Rectangle(firstRectCoord, rectangleSize));
        }

        private Point FindClosestCloudBorder(Size rectangleSize)
        {
            var topBorder = BorderSegments
                                .Select(a => a.start.Y)
                                .OrderBy(a => a)
                                .First();
            var botBorder = BorderSegments
                    .Select(a => a.start.Y)
                    .OrderByDescending(a => a)
                    .First();
            var leftBorder = BorderSegments
                    .Select(a => a.start.X)
                    .OrderByDescending(a => a)
                    .First();
            var rightBorder = BorderSegments
                    .Select(a => a.start.X)
                    .OrderBy(a => a)
                    .First();
            var minDist = new[] { Math.Abs(topBorder), Math.Abs(botBorder), Math.Abs(leftBorder), Math.Abs(rightBorder) }.Min();
            if (minDist == Math.Abs(topBorder))
                return new Point(CloudCenter.X - (int)Math.Floor(rectangleSize.Width / (double)2), topBorder - rectangleSize.Height);
            if (minDist == Math.Abs(botBorder))
                return new Point(CloudCenter.X - (int)Math.Ceiling(rectangleSize.Width / (double)2), botBorder);
            if (minDist == Math.Abs(leftBorder))
                return new Point(CloudCenter.Y - (int)Math.Floor(rectangleSize.Height / (double)2), leftBorder - rectangleSize.Width);
            return new Point(CloudCenter.Y - (int)Math.Ceiling(rectangleSize.Height / (double)2), rightBorder);

        }
    }

}
