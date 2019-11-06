using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;


namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CloudLayouter_Should
    {
        private readonly Point center = new Point(0, 0);
        private CircularCloudLayouter layouter;
        private Random rnd = new Random();
        private Size size;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(center);
            size = GetRandomSize();
        }

        [TestCase(10, 10)]
        [TestCase(25, 25)]
        public void PutNextRectangle_WithoutChangingSize(int width, int height)
        {
            var size = new Size(width, height);
            layouter.PutNextRectangle(size).Size.Should().BeEquivalentTo(size);
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(-10, 0)]
        public void ThrowArgumentException_OnNonPositiveSize(int width, int height)
        {
            Action act = () => layouter.PutNextRectangle(new Size(width, height));
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0)]
        [TestCase(1000, 1000)]
        [TestCase(-1001, 1001)]
        public void PutFirstRectangle_NearCenter(int x, int y)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));
            var rectangle = layouter.PutNextRectangle(size);

            rectangle.GetCenter().X.Should().BeApproximately(x, (float)size.Width / 2);
            rectangle.GetCenter().Y.Should().BeApproximately(y, (float)size.Height / 2);
        }

        [Test]
        public void PutRectanglesOnLayout_WithoutIntersection()
        {
            var amountOfRectangles = rnd.Next(2, 25);
            var rectangles = new List<Rectangle>();
            while (amountOfRectangles-- > 0)
            {
                var nextRectangle = layouter.PutNextRectangle(GetRandomSize());
                rectangles.Any(rect => rect.IntersectsWith(nextRectangle))
                    .Should().BeFalse("rectangles should not intersect!");
                rectangles.Add(nextRectangle);
            }

        }

        [TestCase(0, 0)]
        [TestCase(100, -100)]
        public void PutRectangles_DenselyAroundCenter(int x, int y)
        {
            var center = new Point(x, y);
            var layouter = new CircularCloudLayouter(center);

            var farthestCenterPoint = new PointF(x, y);
            var maxSquaredDistance = 0.0;
            var totalMass = 0.0;
            var amountOfRectangles = rnd.Next(25, 50);
            while(amountOfRectangles-- > 0)
            {
                var nextRectangle = layouter.PutNextRectangle(GetRandomSize());
                totalMass += nextRectangle.Width * nextRectangle.Height;
                var distanceToCenter = nextRectangle
                    .GetCenter().GetSquaredDistanceTo(center);
                if(distanceToCenter > maxSquaredDistance)
                    maxSquaredDistance = distanceToCenter;
            }

            var circleSize = Math.PI * maxSquaredDistance;
            var emptyArea = circleSize - totalMass;
            emptyArea.Should().NotBeInRange(circleSize / 2, double.MaxValue, 
                "more than half of the minimum circular area containing all" +
                "of the rectangles should not be empty");

        }

        public Size GetRandomSize()
        {
            return new Size(rnd.Next(5, 100), rnd.Next(5, 100));
        }
    }
}
