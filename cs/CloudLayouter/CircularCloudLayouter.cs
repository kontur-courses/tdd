using System;
using System.Drawing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;

namespace CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public int Count{ get { return rectangles.Count; }}
        private SortedSet<Point> anchorpoints;
        private HashSet<Rectangle> rectangles;
        private Point center;

        public void ReformTagCloud()
        {
            var sizeList = rectangles.Select(rectangle => rectangle.Size)
                                     .OrderBy(size => -size.Height * size.Width)
                                     .ToList();
            rectangles = new HashSet<Rectangle>();
            anchorpoints.Clear();
            anchorpoints.Add(center);
            foreach (var size in sizeList)
            {
                PutNextRectangle(size);
            }
        }

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            anchorpoints = new SortedSet<Point>(new PointDistanceComparer(this.center));
            rectangles = new HashSet<Rectangle>();
            
            anchorpoints.Add(center);
        }
        
        public Rectangle PutNextRectangle(Size size)
        {
            if(size.IsEmpty)
                throw new ArgumentException("Size of rectangle can't be Empty.");

            var newRectangle = SearchBestValidPlace(size);
            AddAnchorPoints(newRectangle);
            rectangles.Add(newRectangle);
            return newRectangle;
        }

        private Rectangle SearchBestValidPlace(Size size)
        {
            var newRectangle = new Rectangle(new Point(), size);
            
            foreach (var point in anchorpoints)
            {
                foreach (var possibleLocationPoint in point.GetPossibleTagLocation(size))
                {
                    newRectangle.Location = possibleLocationPoint;
                    if (!newRectangle.IntersectsWith(rectangles))
                        return newRectangle;

                }       
            }
            return newRectangle;
        }
        
        private void AddAnchorPoints(Rectangle rectangle)
        {
            foreach (var point in rectangle.GetCornerPoints())
            {
                anchorpoints.Add(point);
            }
        }
       
        public Bitmap Draw(Bitmap bitmap)
        {
            var graphics = Graphics.FromImage(bitmap);
            var random = new Random();
            foreach (var rectangle in rectangles)
            {
                var randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                graphics.FillRectangle(new HatchBrush(HatchStyle.Percent90,randomColor),rectangle);
            }
            return bitmap;
        }


    }

    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle basicRectangle, IEnumerable<Rectangle> rectEnum)
        {
            return rectEnum.Any(rectangle => rectangle.IntersectsWith(basicRectangle));
        }
        
        public static IEnumerable<Point> GetCornerPoints(this Rectangle rectangle)
        {
            yield return new Point(rectangle.Left,rectangle.Top);
            yield return new Point(rectangle.Left,rectangle.Bottom);
            yield return new Point(rectangle.Right,rectangle.Top);
            yield return new Point(rectangle.Right,rectangle.Bottom);
        }    
    }
    
    public static class PointExtensions
    {
        public static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static IEnumerable<Point> GetPossibleTagLocation(this Point point, Size size)
        {
            yield return point;
            yield return new Point(point.X - size.Width, point.Y);
            yield return new Point(point.X, point.Y - size.Height);
            yield return new Point(point.X - size.Width, point.Y - size.Height);
        }
    }

    public class PointDistanceComparer : IComparer<Point>
    {
        private Point anchorPoint;
        
        public PointDistanceComparer(Point anchorPoint)
        {
            this.anchorPoint = anchorPoint;
        }
        
        public int Compare(Point x, Point y)
        {
            if (PointExtensions.GetDistance(x, anchorPoint) > PointExtensions.GetDistance(y, anchorPoint))
                return 1;
            else
                return -1;
        }
    }
}

