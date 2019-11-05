using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private double Alpha { get; set; }
        public List<Rectangle> Rectangles { get;}
        public Point Center { get;}
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            Alpha = 0.0;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            if (Rectangles.Count == 0)
            {
                var x = Center.X - rectangleSize.Width / 2;
                var y = Center.Y + rectangleSize.Height / 2;
                var rectangle = new Rectangle(new Point(x, y), rectangleSize);
                Rectangles.Add(rectangle);
                return rectangle;
            }
            while (true)
            {
                Alpha += Math.PI / 12;
                var intersected = false;
                var r = Alpha*0.4;
                var x = (int)(r * Math.Cos(Alpha)) - rectangleSize.Width / 2 + Center.X;
                var y = (int)(r * Math.Sin(Alpha)) + rectangleSize.Height / 2 + Center.Y;
                var rectangle = new Rectangle(new Point(x, y), rectangleSize);
                foreach (var rec in Rectangles)
                    if (AreIntersected(rec, rectangle))
                        intersected = true;
                if (!intersected)
                {
                    Rectangles.Add(rectangle);
                    return rectangle;
                }
            }
        }

        public bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            return r1.Width + r2.Width >= Math.Max(r2.Left + r2.Width - r1.Left, r1.Left + r1.Width - r2.Left) &&
                   (r1.Height + r2.Height >= Math.Max(r2.Height + r2.Top - r1.Top, r1.Height + r1.Top - r2.Top));
        }
    }
}
