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
                if (segment.Horizontal())
                {
                    var leftBorderX = FindLeftBorderX(segment, rectangleSize);
                    var rightBorderX = FindRightBorderX(segment, rectangleSize);
                    var updatedSegment = new Segment(leftBorderX, segment.start.Y, rightBorderX, segment.end.Y, segment.type);
                    SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
                else
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
                        {
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
                    if (CheckOppositeBorder(midRectCoordinates, rectangleSize, segment.type))
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
                if (CheckOppositeBorder(leftMostRectCoordinates, rectangleSize, segment.type))
                    CheckDistance(searchResult, segment, leftMostRectCoordinates, rectangleSize);
                if (CheckOppositeBorder(rightMostRectCoordinates, rectangleSize, segment.type))
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
                    if (CheckOppositeBorder(midRectCoordinates, rectangleSize, segment.type))
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
                if (CheckOppositeBorder(topMostRectCoordinates, rectangleSize, segment.type))
                    CheckDistance(searchResult, segment, topMostRectCoordinates, rectangleSize);
                if (CheckOppositeBorder(botMostRectcoordinates, rectangleSize, segment.type))
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
            if (segmentType == Segment.Type.Right)// кажется здесь должно быть FindLeftBorderX
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
            int topRectSide = segment.type == Segment.Type.Top ? segment.end.Y - rectangleSize.Height : segment.end.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var leftRectSide = new Segment(0, topRectSide, 0, bottomRectSide, Segment.Type.Left);
            var leftBorder = BorderSegments
                            .Select(a => a)
                            .Where(b =>
                            b.type == Segment.Type.Right
                            && b.start.X <= segment.start.X
                            && b.Intersects(leftRectSide))
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
            int topRectSide = segment.type == Segment.Type.Top ? segment.end.Y - rectangleSize.Height : segment.end.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var rightRectSide = new Segment(0, topRectSide, 0, bottomRectSide, Segment.Type.Right);
            var rightBorder = BorderSegments
                            .Select(a => a)
                            .Where(b =>
                            b.type == Segment.Type.Left
                            && b.start.X >= segment.end.X
                            && b.Intersects(rightRectSide))
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
            int leftRectSide = segment.type == Segment.Type.Left ? segment.end.X - rectangleSize.Width : segment.end.X;
            int rightRectSide = leftRectSide + rectangleSize.Width;
            var topRectSide = new Segment(leftRectSide, 0, rightRectSide, 0, Segment.Type.Top);
            var topBorder = BorderSegments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Bottom
                && b.start.Y <= segment.start.Y
                && b.Intersects(topRectSide))
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
            int leftRectSide = segment.type == Segment.Type.Left ? segment.end.X - rectangleSize.Width : segment.end.X;
            int rightRectSide = leftRectSide + rectangleSize.Width;
            var bottomRectSide = new Segment(leftRectSide, 0, rightRectSide, 0, Segment.Type.Bottom);
            var topBorder = BorderSegments
                .Select(a => a)
                .Where(b =>
                b.type == Segment.Type.Top
                && b.end.Y >= segment.end.Y
                && b.Intersects(bottomRectSide))
                .OrderBy(c => c.start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = 100000;
            else
                topBorderY = topBorder.start.Y;
            return topBorderY;
        }


        private void CheckDistance(PositionSearchResult currentSearchRes, Segment segment, Point rectangleCoord, Size rectangleSize)
        {
            var dist = Distance(GetRectangleCenter(new Rectangle(rectangleCoord, rectangleSize)), CloudCenter);
            if (dist < currentSearchRes.MinDistance)
            {
                currentSearchRes.Update(dist, segment, rectangleCoord);
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

        private void StackSegments(List<Segment> added)
        {
            foreach (var segment in added)
            {
                if (segment.Horizontal())
                {
                    List<Segment> parallelSegments = BorderSegments
                        .Where(a => a.start.Y == segment.start.Y && a.Horizontal()).ToList();
                    parallelSegments.Add(segment);
                    var stacked = FindIntersectionsHorizontal(parallelSegments, segment.start.Y);
                    BorderSegments.ExceptWith(parallelSegments);
                    BorderSegments.UnionWith(stacked);
                }
                else
                {
                    List<Segment> parallelSegments = BorderSegments
                        .Where(a => a.start.X == segment.start.X && !a.Horizontal()).ToList();
                    parallelSegments.Add(segment);
                    var stacked = FindIntersectionsVertical(parallelSegments, segment.start.X);
                    BorderSegments.ExceptWith(parallelSegments);
                    BorderSegments.UnionWith(stacked);
                }
            }
        }

        public static List<Segment> FindIntersectionsVertical(List<Segment> segments, int Xcoord)
        {
            List<CustomPoint> points = new List<CustomPoint>();
            List<Segment> outSegments = new List<Segment>();
            foreach (var segment in segments)
            {
                points.Add(new CustomPoint(segment.start.Y, segment.type, CustomPoint.PointType.Start));
                points.Add(new CustomPoint(segment.end.Y, segment.type, CustomPoint.PointType.End));
            }
            Segment.Type currentType = points[0].SegmentType;
            var currentPoint = points[0];
            var startFound = true;
            bool isIntersection = false;
            bool isFirst = true;
            int lastAdded = int.MinValue;
            var sortedPoints = points.OrderBy(a => a.Coord).ToList();
            for (int i = 0; i < sortedPoints.Count - 1; i++)
            {
                if (sortedPoints[i].Coord == sortedPoints[i + 1].Coord 
                    && sortedPoints[i].SegmentType == sortedPoints[i + 1].SegmentType)
                {
                    if (sortedPoints[i].Type == CustomPoint.PointType.End)
                        sortedPoints[i].Type = CustomPoint.PointType.Start;
                    if (sortedPoints[i + 1].Type == CustomPoint.PointType.End)
                        sortedPoints[i + 1].Type = CustomPoint.PointType.Start;
                }
            }
            foreach (var point in sortedPoints)
            {
                if (isFirst)
                {
                    currentType = point.SegmentType;
                    currentPoint = point;
                    startFound = true;
                    isFirst = false;
                    continue;
                }
                if (isIntersection)
                {
                    startFound = true;
                    currentPoint = point;
                    currentType = Segment.OppositeType(point.SegmentType);
                    isIntersection = false;
                    continue;
                }
                if (!startFound && point.Type==CustomPoint.PointType.Start)
                {
                    if (lastAdded==point.Coord && outSegments.Last().type==point.SegmentType)
                    {
                        var lastSegment = outSegments.Last();
                        outSegments.RemoveAt(outSegments.Count - 1);
                        startFound = true;
                        currentPoint = new CustomPoint(lastSegment.start.Y, point.SegmentType, CustomPoint.PointType.Start);
                        continue;
                    }
                    currentPoint = point;
                    startFound = true;
                    currentType = point.SegmentType;
                    continue;
                }
                if (startFound && point.SegmentType == currentType && point.Type==CustomPoint.PointType.End)
                {
                    outSegments.Add(new Segment(Xcoord, currentPoint.Coord, Xcoord, point.Coord, currentType));
                    lastAdded = point.Coord;
                    startFound = false;
                    continue;
                }
                if (startFound && point.SegmentType!=currentType)
                {
                    outSegments.Add(new Segment(Xcoord, currentPoint.Coord, Xcoord, point.Coord, currentType));
                    lastAdded = point.Coord;
                    startFound = false;
                    isIntersection = true;
                    continue;
                }
            }
            return outSegments.Where(a=>a.Length>0).ToList();
        }

        public static List<Segment> FindIntersectionsHorizontal(List<Segment> segments, int Ycoord)
        {
            List<CustomPoint> points = new List<CustomPoint>();
            List<Segment> outSegments = new List<Segment>();
            foreach (var segment in segments)
            {
                points.Add(new CustomPoint(segment.start.X, segment.type, CustomPoint.PointType.Start));
                points.Add(new CustomPoint(segment.end.X, segment.type, CustomPoint.PointType.End));
            }
            Segment.Type currentType = points[0].SegmentType;
            var currentPoint = points[0];
            var startFound = true;
            bool isIntersection = false;
            bool isFirst = true;
            int lastAdded = int.MinValue;
            var sortedPoints = points.OrderBy(a => a.Coord).ToList();
            for(int i=0; i<sortedPoints.Count-1;i++)
            {
                if (sortedPoints[i].Coord == sortedPoints[i + 1].Coord && sortedPoints[i].SegmentType == sortedPoints[i + 1].SegmentType)
                {
                    sortedPoints[i].Type = CustomPoint.PointType.Start;
                    sortedPoints[i + 1].Type = CustomPoint.PointType.Start;
                    //if (sortedPoints[i].Type == CustomPoint.PointType.End)
                    //    sortedPoints[i].Type = CustomPoint.PointType.Start;
                    //if (sortedPoints[i + 1].Type == CustomPoint.PointType.End)
                    //    sortedPoints[i + 1].Type = CustomPoint.PointType.Start;
                }
            }
            foreach (var point in sortedPoints)
            {
                if (isFirst)
                {
                    currentType = point.SegmentType;
                    currentPoint = point;
                    startFound = true;
                    isFirst = false;
                    continue;
                }
                if (isIntersection)
                {
                    startFound = true;
                    currentPoint = point;
                    currentType = Segment.OppositeType(point.SegmentType);
                    isIntersection = false;
                    continue;
                }
                if (!startFound && point.Type == CustomPoint.PointType.Start)
                {
                    if (lastAdded == point.Coord && outSegments.Last().type == point.SegmentType)
                    {
                        var lastSegment = outSegments.Last();
                        outSegments.RemoveAt(outSegments.Count - 1);
                        startFound = true;
                        currentPoint = new CustomPoint(lastSegment.start.X, point.SegmentType, CustomPoint.PointType.Start);
                        continue;
                    }
                    else
                    {
                        currentPoint = point;
                        startFound = true;
                        currentType = point.SegmentType;
                    }
                    continue;
                }
                if (startFound && point.SegmentType == currentType && point.Type == CustomPoint.PointType.End)
                {
                    outSegments.Add(new Segment(currentPoint.Coord, Ycoord, point.Coord, Ycoord, currentType));
                    lastAdded = point.Coord;
                    startFound = false;
                    continue;
                }
                if (startFound && point.SegmentType != currentType)
                {
                    outSegments.Add(new Segment(currentPoint.Coord, Ycoord, point.Coord, Ycoord, currentType));
                    lastAdded = point.Coord;
                    startFound = false;
                    isIntersection = true;
                    continue;
                }
            }
            return outSegments.Where(a => a.Length > 0).ToList();
        }

        private class CustomPoint
        {
            public int Coord;
            public Segment.Type SegmentType;
            public PointType Type;
            public CustomPoint(int coord, Segment.Type segmentType, PointType type)
            {
                Coord = coord;
                SegmentType = segmentType;
                Type = type;
            }
            public enum PointType
            {
                Start,
                End
            }
        }
    }

}
