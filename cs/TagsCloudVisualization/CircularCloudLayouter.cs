using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private List<Rectangle> cloud;

        //Spiral coeffs
        int i = 0;
        float it = (float)Math.PI / 21;
        float ri = 50;

        public CircularCloudLayouter(Point center)
        {
            if (center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Central point coordinates should be in positive");
            cloud = new List<Rectangle>();
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectSize)
        {
            if (rectSize.Width <= 0 || rectSize.Height <= 0)
                throw new ArgumentException("Size width and height should be positive");

            if (!cloud.Any())
                return new Rectangle(center, rectSize);

            Rectangle rect;

            while (true)
            {
                bool findPlace = true;
                var point = GetNextPoint();

                rect = new Rectangle(new Point(point.X - rectSize.Width / 2, point.Y - rectSize.Height / 2), rectSize);
                foreach (var previous in cloud)
                {
                    if (rect.IntersectsWith(previous))
                    {
                        findPlace = false;
                        break;
                    }
                }

                if (findPlace)
                    break;
            }

            return rect;
        }

        public List<Rectangle> CreateCloud(List<Size> rectangleSizes)
        {
            cloud = new List<Rectangle>();

            foreach (var rectangleSize in rectangleSizes)
            {
                cloud.Add(PutNextRectangle(rectangleSize));
            }

            return cloud;
        }

        public Image CreateImage()
        {
            Bitmap image = new Bitmap(center.X * 2, center.Y * 2);
            Graphics gr = Graphics.FromImage(image);
            Pen pen = new Pen(Color.White);

            gr.Clear(Color.Black);
            gr.DrawRectangles(pen, cloud.ToArray());

            return image;
        }

        private Point GetNextPoint()
        {
            float r = (float)Math.Sqrt(ri * i);
            float t = it * i;
            float x = (float)(r * Math.Cos(t) + center.X);
            float y = (float)(r * Math.Sin(t) + center.Y);
            i++;

            //Console.WriteLine("{0} {1}", x, y);

            return new Point((int)x, (int)y);
        }
    }
}
