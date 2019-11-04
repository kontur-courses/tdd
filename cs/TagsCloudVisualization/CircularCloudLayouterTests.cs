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
            new Action(() => new CircularCloudLayouter(new Point())).Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_NotThrow_WhenEmptySize()
        {
            var cloud = new CircularCloudLayouter(new Point());
            new Action(() => cloud.PutNextRectangle(new Size())).Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_OneSimpleRectangle_RectangleInCenter()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.PutNextRectangle(new Size(10, 20)).Should().Be(new Rectangle(-5, -10, 10, 20));
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
            GetRadiusByCoordinates(
                rectangle.X + rectangle.Width / 2 - center.X,
                rectangle.Y + rectangle.Height / 2 - center.Y)
                .Should().BeLessThan(30);
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
                foreach (var r in rectangles)
                {
                    rectangle.IntersectsWith(r).Should().BeFalse();
                }
                rectangles.Append(rectangle);
            }
        }

        private double GetRadiusByCoordinates(int x, int y)
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}

