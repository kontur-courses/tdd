using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;
using FluentAssertions.Execution;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        [Test]
        public void Generator_NotThrow_WhenEmptyPoint()
        {
            var generator = new Action(() => new CircularCloudLayouter(new Point()));
            generator.Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_NotThrow_WhenEmptySize()
        {
            var cloud = new CircularCloudLayouter(new Point());
            var putNextRectangle = new Action(() => cloud.PutNextRectangle(new Size()));
            putNextRectangle.Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_OneSimpleRectangle_RectangleInCenter()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.PutNextRectangle(new Size(10, 20)).Should().Be(new Rectangle(0, 0, 10, 20));
        }

        [Test]
        public void PutNextRectangle_TwoRectangle_DoNotIntersect()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var rectangle1 = cloud.PutNextRectangle(new Size(10, 20));
            var rectangle2 = cloud.PutNextRectangle(new Size(20, 20));
            rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_TwoRectangle_RadiusLessThenMaxAmount()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            cloud.PutNextRectangle(new Size(10, 20));
            var rectangle = cloud.PutNextRectangle(new Size(20, 20));
            var radius =
                Math.Sqrt(
                    Math.Pow(rectangle.X - center.X, 2) +
                    Math.Pow(rectangle.Y - center.Y, 2));
            radius.Should().BeLessThan(30);

        }

        [Test]
        public void PutNextRectangle_OneHundredRectangle_DoNotIntersect()
        {
            var center = new Point(10, 10);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(10, 20));
                rectangles.Append(rectangle);
            }

            foreach (var rectangle1 in rectangles)
            {
                foreach (var rectangle2 in rectangles)
                {
                    if (rectangle1 != rectangle2)
                        rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
                }
            }
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNextRectangle_NegativeSize_ArgumentException(int width, int height)
        {
            var center = new Point(10, 10);
            var cloud = new CircularCloudLayouter(center);
            var message = "The dimensions of the rectangle must be greater than or equal to zero";

            var putNextRectangle = new Action(() => cloud.PutNextRectangle(new Size(width, height)));
            
            putNextRectangle.Should().Throw<ArgumentException>().WithMessage(message);
        }
    }
}

