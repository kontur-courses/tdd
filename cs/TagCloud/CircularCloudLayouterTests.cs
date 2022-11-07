using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class CircularCloudLayouterTests
    {
        [TestCase(0, 0)]
        [TestCase(5, 10)]
        public void Ctor_SetCenterPoint(int x, int y)
        {
            var planningCenter = new Point(x, y);

            var cloud = new CircularCloudLayouter(planningCenter);

            cloud.Center.Should().BeEquivalentTo(planningCenter);
            cloud.GetWidth().Should().Be(0);
            cloud.GetHeight().Should().Be(0);
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(10, 0)]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            var cloud = new CircularCloudLayouter();

            Action act = () => cloud.PutNextRectangle(new Size(width, height));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(100)]
        [TestCase(50)]
        public void GetWidth_EqualsToTheReactangleWidth(int width)
        {
            var cloud = new CircularCloudLayouter();

            cloud.PutNextRectangle(new Size(width, 3));

            cloud.GetWidth().Should().Be(width);
        }

        [TestCase(100)]
        [TestCase(50)]
        public void GetHeight_EqualsToTheReactangleHeight(int height)
        {
            var cloud = new CircularCloudLayouter();

            cloud.PutNextRectangle(new Size(3, height));

            cloud.GetHeight().Should().Be(height);
        }

        [TestCase(0, 0, 35, 75)]
        [TestCase(3, 3, 5, 5)]
        public void PutNextRectangle_FirstRectangleMustBeInCenter(int centerX, int centerY, int reactWidth, int reactHeight)
        {
            var cloud = new CircularCloudLayouter(new Point(centerX, centerY));

            var rectangle = cloud.PutNextRectangle(new Size(reactWidth, reactHeight));
            var planningReactLocation = new Point(centerX - reactWidth / 2, centerY - reactHeight / 2);

            rectangle.Location.Should().BeEquivalentTo(planningReactLocation);
        }
    }
}