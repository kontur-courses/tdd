using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;



// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly Spiral spiral;

        public Rectangle[] Rectangles => rectangles.ToArray();

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(1/(float)( 2 * Math.PI), center);
            rectangles = new List<Rectangle>();
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {

            var rectangle = new Rectangle(new Point(center.X - rectangleSize.Width /2 ,
                center.Y - rectangleSize.Height / 2 ), rectangleSize);
            while (true)
            {
                if (!rectangle.AreIntersectedAny(rectangles))
                    break;
                var location = spiral.First();
                location = new Point(location.X - rectangleSize.Width / 2,
                    location.Y - rectangleSize.Height / 2); 
                rectangle = new Rectangle(location, rectangleSize);

            }
            rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
