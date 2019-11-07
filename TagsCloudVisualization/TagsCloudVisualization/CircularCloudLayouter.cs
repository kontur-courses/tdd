using System;
using System.Collections.Generic;
using GeometryObjects;

public class CircularCloudLayouter
{
    private Point cloudCenter;
    public List<Rectangle> RectanglesList { get; private set; } = new List<Rectangle>();
    public int RectangleCount => RectanglesList.Count;

    public Point CloudCenter
    {
        get { return cloudCenter; }
        private set
        {
            if (value.Y < 0 || value.X < 0)
                throw new ArgumentException("Coordinates of center must be not negative");
            cloudCenter = value;
        }
    }


    public CircularCloudLayouter(Point center)
    {
        CloudCenter = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = new Rectangle(new Point(0, 0), rectangleSize);
        if (RectanglesList.Count == 0)
        {
            rectangle.LeftBottomVertex.X = Math.Max(cloudCenter.X - rectangleSize.Width / 2,
                0);
            rectangle.LeftBottomVertex.Y = Math.Max(cloudCenter.Y - rectangleSize.Height / 2,
                0);
        }
        else
        {
            var ind = RectangleCount - 1;
            rectangle = GetNextRectangleByPrevious(RectanglesList[ind], rectangleSize);
            while (ind > 0 && rectangle == null)
            {
                ind--;
                rectangle = GetNextRectangleByPrevious(RectanglesList[ind], rectangleSize);
            }
            rectangle = MakeRectangleCloserToCenterAndGetIt(rectangle);
        }
        RectanglesList.Add(rectangle);
        return rectangle;
    }

    private Rectangle GetNextRectangleByPrevious(Rectangle prevRect, Size nextRectSize)
    {
        var possibleLeftBottomVertices = GetPossibleLeftBottomVertices(prevRect, nextRectSize);
        Rectangle resultRec = null;
        double minDist = Double.MaxValue;
        foreach (var curLeftBottomVertex in possibleLeftBottomVertices)
        {
            Rectangle possibleRec = new Rectangle(curLeftBottomVertex, nextRectSize);
            if (possibleRec.LeftBottomVertex.X >= 0 &&
                possibleRec.LeftBottomVertex.Y >= 0 && !IsRectIntersectedWithAny(possibleRec))
            {
                var curDist = DistancesCalculator.GetPointToRectangleDistance(cloudCenter,
                    possibleRec);
                if (curDist < minDist)
                {
                    minDist = curDist;
                    resultRec = possibleRec;
                }
            }
        }
        return resultRec;
    }

    private Point[] GetPossibleLeftBottomVertices(Rectangle prevRect, Size nextRectSize)
    {
        return new Point[]
        {
            prevRect.LeftTopVertex, prevRect.RightTopVertex,
            prevRect.RightBottomVertex,
            new Point(prevRect.RightBottomVertex.X,
                prevRect.RightBottomVertex.Y - nextRectSize.Height),
            new Point(prevRect.LeftBottomVertex.X,
                prevRect.LeftBottomVertex.Y - nextRectSize.Height),
            new Point(prevRect.LeftBottomVertex.X - nextRectSize.Width,
                prevRect.LeftBottomVertex.Y - nextRectSize.Height),
            new Point(prevRect.LeftBottomVertex.X - nextRectSize.Width,
                prevRect.LeftBottomVertex.Y),
            new Point(prevRect.LeftTopVertex.X - nextRectSize.Width,
                prevRect.LeftTopVertex.Y)
        };
    }

    private bool IsRectIntersectedWithAny(Rectangle rec)
    {
        foreach (var curRec in RectanglesList)
        {
            if (Rectangle.AreRectanglesIntersected(rec, curRec))
                return true;
        }
        return false;
    }

    private Rectangle MakeRectangleCloserToCenterAndGetIt(Rectangle rec) //TODO Refactor. Don't know how to simplify
    {
        while (true)
        {
            var isChanged = false;
            while (rec.LeftBottomVertex.Y + 1 <= cloudCenter.Y)
            {
                rec.LeftBottomVertex.Y++;
                if (IsRectIntersectedWithAny(rec))
                {
                    rec.LeftBottomVertex.Y--;
                    break;
                }
                isChanged = true;
            }
            while (rec.LeftBottomVertex.Y - 1 >= cloudCenter.Y)
            {
                rec.LeftBottomVertex.Y--;
                if (IsRectIntersectedWithAny(rec))
                {
                    rec.LeftBottomVertex.Y++;
                    break;
                }
                isChanged = true;
            }
            while (rec.LeftBottomVertex.X + 1 <= cloudCenter.X)
            {
                rec.LeftBottomVertex.X++;
                if (IsRectIntersectedWithAny(rec))
                {
                    rec.LeftBottomVertex.X--;
                    break;
                }
                isChanged = true;
            }
            while (rec.LeftBottomVertex.X - 1 >= cloudCenter.X)
            {
                rec.LeftBottomVertex.X--;
                if (IsRectIntersectedWithAny(rec))
                {
                    rec.LeftBottomVertex.X++;
                    break;
                }
                isChanged = true;
            }
            if (!isChanged)
                break;
        }
        return rec;
    }
}