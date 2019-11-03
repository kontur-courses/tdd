using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization
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
            layouter.PutNextRectangle(size).Size.Should().BeEquivalentTo(size);
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(10, 0)]
        public void ThrowArgumentException_OnZeroSize(int width, int height)
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
        public void PutNextRectangle_NotIntersectingWithPrevious()
        {
            var secondSize = GetRandomSize();
            var firstRectangle = layouter.PutNextRectangle(size);
            var secondRectangle = layouter.PutNextRectangle(secondSize);

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse(
                $"rectangles\n" +
                $"{firstRectangle.ToString()}, {secondRectangle.ToString()}\n" +
                $"should not intersect");
        }

        public Size GetRandomSize()
        {
            return new Size(rnd.Next(5, 100), rnd.Next(5, 100));
        }
    }
}
