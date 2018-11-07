using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class CircularCloudLayouter
    {
        private Point center;
        private IEnumerator<Point> spiralEnumerator;
        public List<Rectangle> addedRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            var x = center.X;
            var y = center.Y;
            if (x < 0 || y < 0)
                throw new ArgumentException();
            this.center = center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rnd = new Random();

            return new Rectangle(rnd.Next(5, 100), rnd.Next(5, 100), rectangleSize.Width, rectangleSize.Height);
        }
    }

    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [TestCase(-1, 1, TestName = "X less than 0")]
        [TestCase(1, -1, TestName = "Y less than 0")]
        public void ctor_ThrowsArgumentExceptionWhen(int centerX, int centerY)
        {
            Action act = () => new CircularCloudLayouter(new Point(centerX, centerY));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnsTwoNonIntersectRectangles()
        {
            var putedRectangles = new List<Rectangle>();
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            var rect1 = circularCloudLayouter.PutNextRectangle(new Size(50, 10));
            var rect2 = circularCloudLayouter.PutNextRectangle(new Size(70, 140));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [TestCase(4)]
        [TestCase(16)]
        [TestCase(65)]
        [TestCase(100)]
        public void PutNextRectangle_ReturnsManyNonIntersectRectangles(int rectanglesCount)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            var putedRectangles = new List<Rectangle>();
            putedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(50, 10)));
            putedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(70, 140)));
            foreach (var rect in putedRectangles)
            {
                foreach (var rect1 in putedRectangles)
                {
                    foreach (var rect2 in putedRectangles)
                    {
                        rect1.IntersectsWith(rect2).Should().BeFalse();
                    }
                }
            }
        }

        [Test]
        public void PutNextRectangle_ReturnsDifferentRectangles()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            var rect1 = circularCloudLayouter.PutNextRectangle(new Size(50, 10));
            var rect2 = circularCloudLayouter.PutNextRectangle(new Size(70, 140));
            rect1.Should().NotBe(rect2);
        }
    }
}