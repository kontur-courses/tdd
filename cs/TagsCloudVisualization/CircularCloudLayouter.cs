using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point center;
        private List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0) throw new ArgumentException("center x must be a positive number");
            if (center.Y < 0) throw new ArgumentException("center y must be a positive number");
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0) throw new ArgumentException("width must be a positive number");
            if (rectangleSize.Height < 0) throw new ArgumentException("height must be a positive number");
            if (rectangles.Count == 0) {
                var rectangle1 = new Rectangle(new Point(center.X-rectangleSize.Width/2, center.Y - rectangleSize.Height / 2), rectangleSize);
                rectangles.Add(rectangle1);
                return rectangle1;
            }
            var rectangle = new Rectangle(new Point(90, 0), rectangleSize);
            rectangles.Add(rectangle);
            var sumSquare = rectangles.Sum(r => r.Width*r.Height);
            var arrHeight = (int)rectangles.Average(r => r.Height);
            var arrWidth = (int)rectangles.Average(r => r.Width);
            var radius = (int)Math.Sqrt(sumSquare / Math.PI);
            var diameter = (int)radius * 2;
            rectangle.X = diameter;
            var colYar = radius / arrHeight;
            var currentHorda = diameter;

            var newRectangles = new List<Rectangle>();

            var enumeratorRectangles = rectangles.GetEnumerator();
            var currentY = center.Y;
            while (colYar-- > 0)
            {
                var rrr = new List<Rectangle>();
                var osh = 0;

                while (osh < currentHorda && enumeratorRectangles.MoveNext())
                {
                    osh += enumeratorRectangles.Current.Width;
                    rrr.Add(enumeratorRectangles.Current);
                }

                var currnetX = center.X - currentHorda / 2;
                var newRectanglesrChast = rrr.Select(r => new Rectangle(new Point(currnetX += r.Width, currentY), r.Size)).ToList();
                newRectangles = newRectangles.Concat(newRectanglesrChast).ToList();

                currentY += arrHeight;
                //currentHorda -= arrWidth * 2;
            }

            rectangles = newRectangles;
            return new Rectangle(0, 0, 0, 0);
        }

        public void Generate()
        {
            var bmp = new Bitmap(center.X*2+100, center.Y*2+100);
            Graphics g = Graphics.FromImage(bmp);
            rectangles.ToList().ForEach(r => g.DrawRectangle(new Pen(Color.BlueViolet, 5f), r));
            bmp.Save("./578.jpg");
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_should
    {
        [TestCase(-1, 1, "center x must be a positive number*", TestName = "x less than zero")]
        [TestCase(1, -1, "center y must be a positive number*", TestName = "y less than zero")]
        public void Constructor_ThrowsArgumentException_When
            (int centerX, int centerY, string expectedExceptionMessage)
        {
            Action ctorInvocation = () => new CircularCloudLayouter(new Point(centerX, centerY));
            ctorInvocation.Should().Throw<ArgumentException>().WithMessage(expectedExceptionMessage);
        }

        [TestCase(-1, 1, "width must be a positive number*", TestName = "x less than zero")]
        [TestCase(1, -1, "height must be a positive number*", TestName = "y less than zero")]
        public void PutFirstRectangle_ThrowsArgumentException_When
            (int width, int height, string expectedExceptionMessage)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(1000, 1000));

            Action ctorInvocation = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));
            ctorInvocation.Should().Throw<ArgumentException>().WithMessage(expectedExceptionMessage);
        }

        [Test]
        public void PutFirstRectangle()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));

            circularCloudLayouter.PutNextRectangle(new Size(100, 20)).Should().Be(new Rectangle(450, 490, 100, 20));
        }

        [Test]
        public void PutManyRectangle()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));

            circularCloudLayouter.PutNextRectangle(new Size(100, 20));
            circularCloudLayouter.PutNextRectangle(new Size(100, 20)).Should().Be(new Rectangle(0, 0, 100, 20));
        }
    }
}
