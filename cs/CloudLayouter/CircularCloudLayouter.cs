using System;
using System.Drawing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public int Count{ get { return rectSet.Count; }}
        private HashSet<Point> anchorpoints;
        private HashSet<Rectangle> rectSet;
        private Point center;
        
        
        public CircularCloudLayouter(Point point, string directoryToSave = null)
        {
            anchorpoints = new HashSet<Point>();
            rectSet = new HashSet<Rectangle>();
            center = point;
            anchorpoints.Add(center);
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.IsEmpty)
                throw new ArgumentException("Size of rectangle can't be Empty.");
            
            Rectangle newRect = new Rectangle(new Point(), rectangleSize);
            
            foreach (var point in anchorpoints.OrderBy(x => CloudLayouterExtensions.Distance(x,center)))
            {
                foreach (var pointTmp in point.GetArea(rectangleSize))
                {
                    newRect.Location = pointTmp;
                    if (!newRect.IntersectsWith(rectSet))
                        goto exit_flag;
                }
                
            }
            
            exit_flag:
            this.UpdateAnchorPoints(newRect);
            rectSet.Add(newRect);
            return newRect;
        }
        
        private void UpdateAnchorPoints(Rectangle rectangle)
        {
            foreach (var point in rectangle.GetPoints())
            {
                anchorpoints.Add(point);
            }
        }
       

        public Bitmap Draw(Bitmap bitmap)
        {
            var graphics = Graphics.FromImage(bitmap);
            Random rnd = new Random();
            foreach (var rectangle in rectSet)
            {
                Color rndColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                graphics.FillRectangle(new HatchBrush(HatchStyle.Percent90,rndColor),rectangle);
            }
            return bitmap;
        }


    }

    public static class CloudLayouterExtensions
    {
        public static bool IntersectsWith(this Rectangle basicRectangle, IEnumerable<Rectangle> rectEnum)
        {
            foreach (var rectangle in rectEnum)
            {
                if (rectangle.IntersectsWith(basicRectangle) || basicRectangle.IntersectsWith(rectangle))
                    return true;
            }
            return false;
        }


        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static IEnumerable<Point> GetArea(this Point point, Size size)
        {
            yield return point;
            yield return new Point(point.X - size.Width,point.Y);
            yield return new Point(point.X, point.Y - size.Height);
            yield return new Point(point.X - size.Width,point.Y - size.Height);
        }


        public static IEnumerable<Point> GetPoints(this Rectangle rectangle)
        {
            yield return new Point(rectangle.Left,rectangle.Top);
            yield return new Point(rectangle.Left,rectangle.Bottom);
            yield return new Point(rectangle.Right,rectangle.Top);
            yield return new Point(rectangle.Right,rectangle.Bottom);
        }      
    }
}

