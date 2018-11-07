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

            var spiral = new ArchimedeanSpiral(1);
            spiralEnumerator = spiral.GetIenumeratorDecart(0.1);
            this.center = center;
        }

        private Point GetNextPoint()
        {
            spiralEnumerator.MoveNext();
            return new Point(spiralEnumerator.Current.X + center.X, spiralEnumerator.Current.Y + center.Y);
        }

        private bool RectanglesAreIntersecting(Rectangle rectangle)
        {
            foreach (var rect in addedRectangles)
            {
                if (rect.IntersectsWith(rectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var currentPoint = center;
            var currentRectangle = new Rectangle(center.X, center.Y, rectangleSize.Width, rectangleSize.Height);

            while (RectanglesAreIntersecting(currentRectangle))
            {
                currentPoint = GetNextPoint();
                currentRectangle = new Rectangle
                    (currentPoint.X, currentPoint.Y, rectangleSize.Width, rectangleSize.Height);
            }


            addedRectangles.Add(currentRectangle);
            return currentRectangle;
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
            for (var i = 0; i < rectanglesCount; i++)
            {
                putedRectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(50, 10)));
            }

            for (var i = 0; i < putedRectangles.Count; i++)
            {
                for (var j = 0; j < putedRectangles.Count; j++)
                {
                    if (i != j)
                    {
                        putedRectangles[i].IntersectsWith(putedRectangles[j]).Should().BeFalse();
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