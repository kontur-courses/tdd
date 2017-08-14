using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Dictionary<Point, Rectangle> Rectangles;
        private Point center;
        private Point firstPoint;
        public Dictionary<Point, Rectangle> GetCloud()
        {
            return Rectangles;
        }

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new Dictionary<Point, Rectangle>();
            this.center = center;
        }

        private Point GetClosestFreePoint(Size rectangleSize)
        {
            var lastRectangle = Rectangles.FirstOrDefault().Value;
            var x = lastRectangle.X + lastRectangle.Size.Width / 2 + rectangleSize.Width / 2;
            var y = lastRectangle.Y + lastRectangle.Size.Height / 2 + rectangleSize.Height / 2;
            var closestPoint = new Point(x, y);
            return closestPoint;

        }

        private IEnumerable<Point> GetOrderedPoints()
        {
            return Rectangles.Keys.OrderBy(p => p.DistanceFrom(center));
        }
        public void SaveBitmap(string fileName)
        {
            var minX = Rectangles.Values.Min(r => r.X);
            var minY = Rectangles.Values.Min(r => r.Y);
            var maxX = Rectangles.Values.Max(r => r.X+r.Width);
            var maxY = Rectangles.Values.Max(r => r.Y + r.Height);
            var bitmap = new Bitmap(maxX,maxY);
            SolidBrush b = new SolidBrush(Color.White);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(b,0,0,maxX,maxY);
            var pen = new Pen(Color.Red,1);
            foreach (var rectangle in Rectangles.Values)
            {
                g.DrawRectangle(pen,rectangle);
            }
            var cropArea = new Rectangle(minX,minY,maxX-minX,maxY-minY);
            bitmap = bitmap.Clone(cropArea, bitmap.PixelFormat);
            bitmap.Save($"{fileName}.bmp");
            
            
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            Point point;
            if (Rectangles.Count == 0)
            {
                point = new Point(center.X-rectangleSize.Width/2,center.Y+rectangleSize.Height/2);
                firstPoint = point;
                rectangle = new Rectangle(point,rectangleSize);
            }
            else
            {
                var newRect = GetNewRectangle(rectangleSize);
                point = new Point(newRect.Value.X, newRect.Value.Y);
                rectangle = newRect.Value;
            }
            Rectangles.Add(point,rectangle);
            return rectangle;

        }

        private Rectangle? GetNewRectangle(Size rectangleSize)
        {
            var orderedPoints = GetOrderedPoints();
            foreach (var orderedPoint in orderedPoints)
            {
               foreach (var rectangle in Rectangles[orderedPoint].GetNeighbour(rectangleSize))
                {
                    var intersections = Rectangles.Values.FirstOrDefault(rect => rectangle.IntersectsWith(rect));
                    if (intersections == new Rectangle())
                    {
                        return rectangle;
                    }
                }
            }
            return null;
        }

        
    }

  }