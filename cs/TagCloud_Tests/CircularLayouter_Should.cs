using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Layouting;

namespace TagCloudVisualization_Tests
{
    public class CircularLayouter_Should
    {
        [Test]
        public void NotThrow_WhenCreated()
        {
            Action act = () => new CircularCloudLayouter(new Point(0, 0));
            act.Should().NotThrow();
        }

        [Test]
        public void Have_CenterPoint_AfterCreate()
        {
            var centerPoint = new Point(0, 0);

            var layouter = new CircularCloudLayouter(centerPoint);

            layouter.GetCloudCenter().Should().BeEquivalentTo(centerPoint);
        }

        [Test]
        public void ReturnRectangle_WhenPutNext()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            var expectedRect = new Rectangle(new Point(0, 0), new Size(10, 10));
            var actualRect = layouter.PutNextRectangle(new Size(10, 10));

            actualRect.Size.Should().BeEquivalentTo(expectedRect.Size);
        }

        [Test]
        public void Allow_NegativeCoordinates_ForCloudCenter()
        {
            var center = new Point(-1, -1);

            var layouter = new CircularCloudLayouter(center);

            layouter.GetCloudCenter().Should().BeEquivalentTo(center);
        }

        [Test]
        public void ThrowArgumentException_WhenNotPositiveSize(
            [ValueSource(nameof(IncorrectRectangleSizes))]
            Size rectSize)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            Action act = () => layouter.PutNextRectangle(rectSize);

            act.Should().Throw<ArgumentException>("Negative size not allowed");
        }

        [Test]
        public void PutFirstRectangle_InCenter()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectSize = new Size(10, 10);

            var rect = layouter.PutNextRectangle(rectSize);

            rect.Location
                .Should()
                .BeEquivalentTo(new Point(-5, -5), "because first rectangle should align by its middle point");
        }

        [Test]
        public void NotIntersect_TwoRectangles()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));

            var firstRect = layouter.PutNextRectangle(new Size(10, 10));
            var secondRect = layouter.PutNextRectangle(new Size(5, 15));

            firstRect.IntersectsWith(secondRect).Should().BeFalse();
        }

        private static IEnumerable<Size> IncorrectRectangleSizes()
        {
            yield return new Size(-1, -1);
            yield return new Size(-1, 0);
            yield return new Size(0, -1);
            yield return new Size(0, 0);
        }
    }
}
