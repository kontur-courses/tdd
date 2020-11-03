using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudTests
    {
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0));
            rectangles = new List<Rectangle>();
        }

        [Test]
        public void Constructor_IsEmpty_MustSetCenter()
        {
            var layouter = new CircularCloudLayouter(new Point(3, 9));

            layouter.Center.Should().Be(new Point(3, 9));
        }

        [Test]
        public void PutNextRectangle_IsEmpty_ShouldReturnCorrectSize()
        {
            var rectangle = layouter.PutNextRectangle(new Size(4, 11));

            rectangle.Width.Should().Be(4);
            rectangle.Height.Should().Be(11);
        }

        [Test]
        public void PutNextRectangle_IsEmpty_RectanglesShouldNotIntersect()
        {
            var rect1 = layouter.PutNextRectangle(new Size(200, 30));
            var rect2 = layouter.PutNextRectangle(new Size(200, 30));

            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ManyRectangles_RectanglesShouldNotIntersect()
        {
            var rectangles = new List<Rectangle>();
            var size = new Size(300, 20);
            var count = 1000;

            for (var i = 0; i < count; i++)
                rectangles.Add(layouter.PutNextRectangle(size));

            IntersectionsTest(rectangles);
        }

        [Test]
        public void PutNextRectangle_IsEmpty_FirstRectangleShouldBeAroundCenter()
        {
            var rectangle = 
                new CircularCloudLayouter(new Point(-500, 4000))
                .PutNextRectangle(new Size(1, 1));

            rectangle.X.Should().BeInRange(-502, -498);
            rectangle.Y.Should().BeInRange(3998, 4002);
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_IsEmpty_ShouldAdd1000RectanglesIn1Second()
        {
            var count = 1000;
            var size = new Size(20, 50);

            for (var i = 0; i < count; i++)
                layouter.PutNextRectangle(size);
        }

        [Test]
        public void PutNextRectangle_IsEmpty_ShouldLooksAsCircle()
        {
            var size = new Size(20, 40);
            var count = 1000;
            var area = size.Width * size.Height * count;
            var farthestPointDistance = 0d;

            for (var i = 0; i < count; i++)
            {
                var rectangle = PutRectangle(size);
                var distance = LinearMath.DistanceBetween(
                    layouter.Center,
                    rectangle.Center());
                if (distance > farthestPointDistance)
                    farthestPointDistance = distance;
            }

            var circleArea = Math.PI * farthestPointDistance * farthestPointDistance;
            var different = circleArea / area;

            different.Should().BeInRange(1d, 4d);
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status == TestStatus.Failed)
            {
            }
        }

        private void IntersectionsTest(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; ++i)
            {
                for (var j = 0; j < rectangles.Count; ++j)
                {
                    if (i == j)
                        continue;
                    rectangles[i].IntersectsWith(rectangles[j]).Should().Be(false);
                }
            }
        }

        private Rectangle PutRectangle(Size size)
        {
            var rectangle = layouter.PutNextRectangle(size);
            rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
