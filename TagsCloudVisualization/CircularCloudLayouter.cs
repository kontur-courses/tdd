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
        private List<Rectangle> rectangles = new List<Rectangle>();
        //private HashSet<Angle> angles;
        private HashSet<Segment> Segments;
        private bool isFirstRectangle;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            this.Segments = new HashSet<Segment>();
            this.isFirstRectangle = true;
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
                return new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize);
            }
            double minDistance = double.MaxValue;
            Segment minSegment = null;
            Point minCoord = new Point();
            foreach (var segment in Segments)
            {
                if (segment.Length < rectangleSize.Width)// на самом деле есть и другие варианты но пока так
                    continue;
                if (segment.type == Segment.Type.Top)
                {
                    Point startAngleCoord = new Point(segment.start.X, segment.start.Y - rectangleSize.Height);
                    double startDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (startDist < minDistance)
                    {
                        minDistance = startDist;
                        minSegment = segment;
                        minCoord = startAngleCoord;
                    }
                    Point endAngleCoord = new Point(segment.end.X - rectangleSize.Width, segment.start.Y - rectangleSize.Height);
                    double endDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (endDist < minDistance)
                    {
                        minDistance = endDist;
                        minSegment = segment;
                        minCoord = endAngleCoord;
                    }
                }
                if (segment.type == Segment.Type.Bottom)
                {
                    Point startAngleCoord = new Point(segment.start.X, segment.start.Y);
                    double startDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (startDist < minDistance)
                    {
                        minDistance = startDist;
                        minSegment = segment;
                        minCoord = startAngleCoord;
                    }
                    Point endAngleCoord = new Point(segment.end.X - rectangleSize.Width, segment.start.Y);
                    double endDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (endDist < minDistance)
                    {
                        minDistance = endDist;
                        minSegment = segment;
                        minCoord = endAngleCoord;
                    }
                }
                if (segment.type == Segment.Type.Left)
                {
                    Point startAngleCoord = new Point(segment.start.X-rectangleSize.Width, segment.start.Y);
                    double startDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (startDist < minDistance)
                    {
                        minDistance = startDist;
                        minSegment = segment;
                        minCoord = startAngleCoord;
                    }
                    Point endAngleCoord = new Point(segment.end.X - rectangleSize.Width, segment.start.Y-rectangleSize.Height);
                    double endDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (endDist < minDistance)
                    {
                        minDistance = endDist;
                        minSegment = segment;
                        minCoord = endAngleCoord;
                    }
                }
                if (segment.type == Segment.Type.Right)
                {
                    Point startAngleCoord = new Point(segment.start.X, segment.start.Y);
                    double startDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (startDist < minDistance)
                    {
                        minDistance = startDist;
                        minSegment = segment;
                        minCoord = startAngleCoord;
                    }
                    Point endAngleCoord = new Point(segment.end.X, segment.start.Y - rectangleSize.Height);
                    double endDist = Distance(GetRectangleCenter(new Rectangle(startAngleCoord, rectangleSize)), center);
                    if (endDist < minDistance)
                    {
                        minDistance = endDist;
                        minSegment = segment;
                        minCoord = endAngleCoord;
                    }
                }
            }
            if (minSegment.type == Segment.Type.Top)
            {
                Segments.Remove(minSegment);
                //adding rectangle top side
                Segments.Add(new Segment(minCoord.X, minCoord.Y, minCoord.X + rectangleSize.Width, minCoord.Y, Segment.Type.Top));
                //adding rest part of previous segment
                if (minCoord.X==minSegment.start.X)  //this case:  |_|_____
                    Segments.Add(new Segment(minCoord.X + rectangleSize.Width, minSegment.end.Y, minSegment.end.X, minSegment.end.Y, Segment.Type.Top));
                else  //this case:  _____|_|
                    Segments.Add(new Segment(minSegment.start.X, minSegment.start.Y, minSegment.end.X-rectangleSize.Width, minSegment.end.Y, Segment.Type.Top));
                //adding rectangle left side
                Segments.Add(new Segment(minCoord, minSegment.start, Segment.Type.Left));
                //adding rectangle right side
                Segments.Add(new Segment(minCoord.X + rectangleSize.Width, minCoord.Y, minSegment.end.X, minCoord.Y, Segment.Type.Right));
                return new Rectangle(minCoord, rectangleSize);
            }
            if (minSegment.type == Segment.Type.Bottom)
            {
                Segments.Remove(minSegment);
                //adding rectangle bottom side
                Segments.Add(new Segment(minCoord.X, minCoord.Y+rectangleSize.Height, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Bottom));
                //adding rest part of previous segment
                if (minCoord.X == minSegment.start.X)  //this case:  |‾|‾‾‾‾‾‾‾
                    Segments.Add(new Segment(minCoord.X + rectangleSize.Width, minSegment.end.Y, minSegment.end.X, minSegment.end.Y, Segment.Type.Bottom));
                else  //this case:  ‾‾‾‾‾‾‾|‾|
                    Segments.Add(new Segment(minSegment.start.X, minSegment.start.Y, minSegment.end.X - rectangleSize.Width, minSegment.end.Y, Segment.Type.Bottom));
                //adding rectangle left side
                Segments.Add(new Segment(minCoord, new Point(minCoord.X, minCoord.Y+rectangleSize.Height), Segment.Type.Left));
                //adding rectangle right side
                Segments.Add(new Segment(minCoord.X + rectangleSize.Width, minCoord.Y, minCoord.X+rectangleSize.Width, minCoord.Y+rectangleSize.Height, Segment.Type.Right));
                return new Rectangle(minCoord, rectangleSize);
            }
            if (minSegment.type == Segment.Type.Left)
            {
                Segments.Remove(minSegment);
                //adding rectangle left side
                Segments.Add(new Segment(minCoord.X, minCoord.Y, minCoord.X, minCoord.Y + rectangleSize.Height, Segment.Type.Left));
                //adding rest part of previous segment
                if (minCoord.Y == minSegment.start.Y)  //this case:  ‾|  возможно здесь косяк
                    Segments.Add(new Segment(minCoord.X + rectangleSize.Width, minSegment.start.Y+rectangleSize.Height, minSegment.end.X, minSegment.end.Y, Segment.Type.Left));
                else  //this case: _|
                    Segments.Add(new Segment(minSegment.start.X, minSegment.start.Y, minSegment.end.X - rectangleSize.Width, minSegment.end.Y, Segment.Type.Left));
                //adding rectangle top side
                Segments.Add(new Segment(minCoord, new Point(minCoord.X+rectangleSize.Width, minCoord.Y), Segment.Type.Top));
                //adding rectangle bottom side
                Segments.Add(new Segment(minCoord.X, minCoord.Y+rectangleSize.Height, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Bottom));
                return new Rectangle(minCoord, rectangleSize);
            }
            if (minSegment.type == Segment.Type.Right)
            {
                Segments.Remove(minSegment);
                //adding rectangle right side
                Segments.Add(new Segment(minCoord.X+rectangleSize.Width, minCoord.Y, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Right));
                //adding rest part of previous segment
                if (minCoord.Y == minSegment.start.Y)  //this case:  |‾
                    Segments.Add(new Segment( minCoord.X, minCoord.Y+rectangleSize.Height, minSegment.end.X, minSegment.end.Y, Segment.Type.Right));
                else  //this case: |_
                    Segments.Add(new Segment( minSegment.start, minCoord, Segment.Type.Right));
                //adding rectangle top side
                Segments.Add(new Segment(minCoord, new Point(minCoord.X + rectangleSize.Width, minCoord.Y), Segment.Type.Top));
                //adding rectangle bottom side
                Segments.Add(new Segment(minCoord.X, minCoord.Y + rectangleSize.Height, minCoord.X + rectangleSize.Width, minCoord.Y + rectangleSize.Height, Segment.Type.Bottom));
                return new Rectangle(minCoord, rectangleSize);
            }
            return new Rectangle(center, rectangleSize);
        }

        private Point GetSizeCenter(Size size)
        {
            return new Point(size.Width / 2, size.Height / 2);
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


    }
}
