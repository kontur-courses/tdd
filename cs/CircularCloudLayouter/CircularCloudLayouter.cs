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

        private readonly Random random =  new Random();

        private readonly Spiral spiral;

        public Rectangle[] Rectangles => rectangles.ToArray();


        public CircularCloudLayouter(Point center)
        {
            this.spiral = new Spiral(1/(float)( 2 * Math.PI), center);
            rectangles = new List<Rectangle>();
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {

            var r = new Rectangle(new Point(center.X - rectangleSize.Width /2 ,
                center.Y - rectangleSize.Height / 2 ), rectangleSize);
            while (true)
            {
                if (!r.AreIntersectedAny(rectangles))
                    break;
                var location = spiral.First();
                location = new Point(location.X - rectangleSize.Width / 2,
                    location.Y - rectangleSize.Height / 2); 
                r = new Rectangle(location, rectangleSize);

            }
            rectangles.Add(r);
            return r;
        }
    }
}
