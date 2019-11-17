using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{

    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point CloudCenter;
        private readonly HashSet<Segment> BorderSegments;
        private readonly HashSet<Segment> ProbablyBuggedSegments;
        private bool isFirstRectangle;
        private readonly List<Rectangle> addedRectangles;
        public CircularCloudLayouter(Point center)
        {
            CloudCenter = center;
            BorderSegments = new HashSet<Segment>();
            ProbablyBuggedSegments = new HashSet<Segment>();
            isFirstRectangle = true;
            addedRectangles = new List<Rectangle>();
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
                    var updatedSegment = new Segment(leftBorderX, segment.Start.Y, rightBorderX, segment.End.Y, segment.SegmentType);
                    searchResult = SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
                else
                {
                    var topBorderY = FindTopBorderY(segment, rectangleSize);
                    var bottomBorderY = FindBottomBorderY(segment, rectangleSize);
                    var updatedSegment = new Segment(segment.Start.X, topBorderY, segment.End.X, bottomBorderY, segment.SegmentType);
                    searchResult = SearchMinDistance(searchResult, segment, rectangleSize, updatedSegment);
                }
            }
            var outRectangle = new Rectangle(searchResult.ClosestRectCoordinates, rectangleSize);
            SegmentStacker.StackSegments(Segment.GetSegmentsFromRectangle(outRectangle), BorderSegments);
            addedRectangles.Add(outRectangle);
            return outRectangle;
        }


        private PositionSearchResult SearchMinDistance(PositionSearchResult searchResult, Segment segment, Size rectangleSize, Segment extendedSegment)
        {
            if (segment.Horizontal())
            {
                if (extendedSegment.Length < rectangleSize.Width)
                    return searchResult;
                if (extendedSegment.Start.X < CloudCenter.X
                && extendedSegment.End.X > CloudCenter.X
                && CloudCenter.X + (int)Math.Truncate(rectangleSize.Width / (double)2) + 1 <= extendedSegment.End.X
                && CloudCenter.X - (int)Math.Truncate(rectangleSize.Width / (double)2) - 1 >= extendedSegment.Start.X)
                {
                    Point midRectCoordinates;
                    if (segment.SegmentType == Segment.Type.Top)
                        midRectCoordinates = new Point(CloudCenter.X - (int)Math.Truncate(rectangleSize.Width / (double)2), extendedSegment.Start.Y - rectangleSize.Height);
                    else
                        midRectCoordinates = new Point(CloudCenter.X - (int)Math.Truncate(rectangleSize.Width / (double)2), extendedSegment.Start.Y);
                    if (CheckOppositeBorder(midRectCoordinates, rectangleSize, segment.SegmentType))
                        searchResult = CheckDistance(searchResult, segment, midRectCoordinates, rectangleSize);
                }
                Point leftMostRectCoordinates;
                Point rightMostRectCoordinates;
                if (segment.SegmentType == Segment.Type.Top)
                {
                    leftMostRectCoordinates = new Point(extendedSegment.Start.X, extendedSegment.Start.Y - rectangleSize.Height);
                    rightMostRectCoordinates = new Point(extendedSegment.End.X - rectangleSize.Width, extendedSegment.Start.Y - rectangleSize.Height);
                }
                else
                {
                    leftMostRectCoordinates = new Point(extendedSegment.Start.X, extendedSegment.Start.Y);
                    rightMostRectCoordinates = new Point(extendedSegment.End.X - rectangleSize.Width, extendedSegment.Start.Y);
                }
                if (CheckOppositeBorder(leftMostRectCoordinates, rectangleSize, segment.SegmentType))
                    searchResult = CheckDistance(searchResult, segment, leftMostRectCoordinates, rectangleSize);
                if (CheckOppositeBorder(rightMostRectCoordinates, rectangleSize, segment.SegmentType))
                    searchResult = CheckDistance(searchResult, segment, rightMostRectCoordinates, rectangleSize);
            }
            else
            {
                if (extendedSegment.Length < rectangleSize.Height)
                    return searchResult;
                if (extendedSegment.Start.Y < CloudCenter.Y
                && extendedSegment.End.Y > CloudCenter.Y
                && CloudCenter.Y + (int)Math.Truncate(rectangleSize.Height / (double)2) + 1 <= extendedSegment.End.Y
                && CloudCenter.Y - (int)Math.Truncate(rectangleSize.Height / (double)2) - 1 >= extendedSegment.Start.Y)
                {
                    Point midRectCoordinates;
                    if (segment.SegmentType == Segment.Type.Left)
                        midRectCoordinates = new Point(extendedSegment.Start.X - rectangleSize.Width, CloudCenter.Y - (int)Math.Truncate(rectangleSize.Height / (double)2));
                    else
                        midRectCoordinates = new Point(extendedSegment.Start.X, CloudCenter.Y - (int)Math.Truncate(rectangleSize.Height / (double)2));
                    if (CheckOppositeBorder(midRectCoordinates, rectangleSize, segment.SegmentType))
                        searchResult = CheckDistance(searchResult, segment, midRectCoordinates, rectangleSize);
                }
                Point topMostRectCoordinates;
                Point botMostRectcoordinates;
                if (segment.SegmentType == Segment.Type.Left)
                {
                    topMostRectCoordinates = new Point(extendedSegment.Start.X - rectangleSize.Width, extendedSegment.Start.Y);
                    botMostRectcoordinates = new Point(extendedSegment.End.X - rectangleSize.Width, extendedSegment.End.Y - rectangleSize.Height);
                }
                else
                {
                    topMostRectCoordinates = new Point(extendedSegment.Start.X, extendedSegment.Start.Y);
                    botMostRectcoordinates = new Point(extendedSegment.End.X, extendedSegment.End.Y - rectangleSize.Height);
                }
                if (CheckOppositeBorder(topMostRectCoordinates, rectangleSize, segment.SegmentType))
                    searchResult = CheckDistance(searchResult, segment, topMostRectCoordinates, rectangleSize);
                if (CheckOppositeBorder(botMostRectcoordinates, rectangleSize, segment.SegmentType))
                    searchResult = CheckDistance(searchResult, segment, botMostRectcoordinates, rectangleSize);
            }
            return searchResult;

        }


        private bool CheckOppositeBorder(Point rectanglePos, Size rectangleSize, Segment.Type segmentType)
        {
            if (segmentType == Segment.Type.Top)
            {
                var TopBorderY = FindTopBorderY(
                    new Segment(
                        new Point(rectanglePos.X, rectanglePos.Y + rectangleSize.Height - 1)
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
                        new Point(rectanglePos.X + rectangleSize.Width - 1, rectanglePos.Y + rectangleSize.Height)
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
            int topRectSide = segment.SegmentType == Segment.Type.Top ? segment.End.Y - rectangleSize.Height : segment.End.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var leftRectSide = new Segment(0, topRectSide, 0, bottomRectSide, Segment.Type.Left);
            var leftBorder = BorderSegments
                            .Select(a => a)
                            .Where(b =>
                            b.SegmentType == Segment.Type.Right
                            && b.Start.X <= segment.Start.X
                            && b.Intersects(leftRectSide))
                            .OrderByDescending(c => c.Start.X)
                            .FirstOrDefault();
            int leftBorderX;
            if (leftBorder == null)
                leftBorderX = -100000;
            else
                leftBorderX = leftBorder.Start.X;
            return leftBorderX;
        }

        private int FindRightBorderX(Segment segment, Size rectangleSize)
        {
            int topRectSide = segment.SegmentType == Segment.Type.Top ? segment.End.Y - rectangleSize.Height : segment.End.Y;
            int bottomRectSide = topRectSide + rectangleSize.Height;
            var rightRectSide = new Segment(0, topRectSide, 0, bottomRectSide, Segment.Type.Right);
            var rightBorder = BorderSegments
                            .Select(a => a)
                            .Where(b =>
                            b.SegmentType == Segment.Type.Left
                            && b.Start.X >= segment.End.X
                            && b.Intersects(rightRectSide))
                            .OrderBy(c => c.Start.X)
                            .FirstOrDefault();
            int rightBorderX;
            if (rightBorder == null)
                rightBorderX = 100000;
            else
                rightBorderX = rightBorder.Start.X;
            return rightBorderX;
        }

        private int FindTopBorderY(Segment segment, Size rectangleSize)
        {
            int leftRectSide = segment.SegmentType == Segment.Type.Left ? segment.End.X - rectangleSize.Width : segment.End.X;
            int rightRectSide = leftRectSide + rectangleSize.Width;
            var topRectSide = new Segment(leftRectSide, 0, rightRectSide, 0, Segment.Type.Top);
            var topBorder = BorderSegments
                .Select(a => a)
                .Where(b =>
                b.SegmentType == Segment.Type.Bottom
                && b.Start.Y <= segment.Start.Y
                && b.Intersects(topRectSide))
                .OrderByDescending(c => c.Start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = -100000;
            else
                topBorderY = topBorder.Start.Y;
            return topBorderY;
        }

        private int FindBottomBorderY(Segment segment, Size rectangleSize)
        {
            int leftRectSide = segment.SegmentType == Segment.Type.Left ? segment.End.X - rectangleSize.Width : segment.End.X;
            int rightRectSide = leftRectSide + rectangleSize.Width;
            var bottomRectSide = new Segment(leftRectSide, 0, rightRectSide, 0, Segment.Type.Bottom);
            var topBorder = BorderSegments
                .Select(a => a)
                .Where(b =>
                b.SegmentType == Segment.Type.Top
                && b.End.Y >= segment.End.Y
                && b.Intersects(bottomRectSide))
                .OrderBy(c => c.Start.Y)
                .FirstOrDefault();
            int topBorderY;
            if (topBorder == null)
                topBorderY = 100000;
            else
                topBorderY = topBorder.Start.Y;
            return topBorderY;
        }


        private PositionSearchResult CheckDistance(PositionSearchResult currentSearchRes, Segment segment, Point rectangleCoord, Size rectangleSize)
        {
            var dist = Distance(GetRectangleCenter(new Rectangle(rectangleCoord, rectangleSize)), CloudCenter);
            if (dist < currentSearchRes.MinDistance)
            {
                return currentSearchRes.Update(dist, segment, rectangleCoord);
            }
            return currentSearchRes;
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
            var firstRect = new Rectangle(firstRectCoord, rectangleSize);
            BorderSegments.UnionWith(Segment.GetSegmentsFromRectangle(firstRect));
            addedRectangles.Add(firstRect);
        }
    }

}
