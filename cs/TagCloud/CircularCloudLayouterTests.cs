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
    }
}