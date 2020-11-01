using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTest
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter tagCloudWithCenterInZero;
        private Random rnd;

        [SetUp]
        public void SetUp()
        {
            tagCloudWithCenterInZero = new CircularCloudLayouter(new Point(0, 0));
        }

        private Size GetRandomSize()
        {
            return new Size(rnd.Next() % 100 + 1, rnd.Next() % 100 + 1);
        }

        [Test]
        public void PutNextRectangle_DoesntThrow()
        {
            Assert.DoesNotThrow(() => tagCloudWithCenterInZero.PutNextRectangle(new Size(10, 10)));
        }

        [Test]
        public void PutNextRectangle_PutsFirstRectangleInCenter()
        {
            var center = new Point(10, 18);
            var shiftedTagCloud = new CircularCloudLayouter(center);
            shiftedTagCloud.PutNextRectangle(new Size(10, 5));

            shiftedTagCloud.Rectangles[0].Location.Should().Be(center);
        }

        [Test]
        public void Rectangles_ShouldNotIntersect()
        {
            for (var i = 0; i < 100; i++)
                tagCloudWithCenterInZero.PutNextRectangle(GetRandomSize());

            foreach (var rectangle in tagCloudWithCenterInZero.Rectangles)
            {
                tagCloudWithCenterInZero.Rectangles.All(
                        other => other.Equals(rectangle) || !other.IntersectsWith(rectangle))
                    .Should().BeTrue();
            }
        }

        [Test, Timeout(100)]
        public void Put1000Rectangles_StopsInSufficientTime()
        {
            for (var i = 0; i < 1000; i++)
                tagCloudWithCenterInZero.PutNextRectangle(GetRandomSize());
        }
    }
}