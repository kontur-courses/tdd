using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnZeroAtStart()
        {
            new ArchimedeanSpiral(new Point(0, 0)).GetNewPointLazy().GetEnumerator().Current.X.Should().Be(0);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnZeroAtStart()
        {
            new ArchimedeanSpiral(new Point(0, 0)).GetNewPointLazy().GetEnumerator().Current.Y.Should().Be(0);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnCorrectX()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            spiral.GetNewPointLazy().First().X.Should().Be(7);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnCorrectY()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            spiral.GetNewPointLazy().First().Y.Should().Be(3);
        }

        [Test]
        public void CircularCloudLayouter_RectanglesSet_ShouldNotBeNull()
        {
            new CircularCloudLayouter(new Point(0, 0)).Rectangles.Should().NotBeNull();
        }

        [Test]
        public void CircularCloudLayouter_RectanglesSet_ShouldBeZeroLengthAtStart()
        {
            new CircularCloudLayouter(new Point(0, 0)).Rectangles.Count.Should().Be(0);
        }

        [Test]
        public void PutNextRectangle_FirstRect_ShouldBeInCenterWithSomeBias()
        {
            new CircularCloudLayouter(new Point(0, 0))
                .PutNextRectangle(new Size(10, 3))
                .Location.ShouldBeEquivalentTo(new Point(7, 3));
        }

        [Test]
        public void PutNextRectangle_ShouldAddOneRectangle()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(10, 3));
            layouter.Rectangles.Count.Should().Be(1);
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_NonPositiveRectSize_ThrowsException(int width, int height)
        {
            Action action = () => new CircularCloudLayouter(new Point(0, 0)).PutNextRectangle(new Size(width, height));
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_AddingRect_ShouldSetNewCurrentCoords()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(5, 6));
            layouter.Spiral.GetNewPointLazy().First().ShouldBeEquivalentTo(new Point(3, 6));
        }
    }
}