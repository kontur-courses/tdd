using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnZeroAtStart() =>
            new ArchimedeanSpiral().GetCurrentX().Should().Be(0);

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnZeroAtStart() =>
            new ArchimedeanSpiral().GetCurrentY().Should().Be(0);

        [Test]
        public void ArchimedeanSpiral_Angle_ShouldBeZeroAtStart() =>
            new ArchimedeanSpiral().Angle.Should().Be(0);

        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnCorrectX()
        {
            var spiral = new ArchimedeanSpiral();
            spiral.IncrementAngle();
            spiral.GetCurrentX().Should().Be(1);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnCorrectY()
        {
            var spiral = new ArchimedeanSpiral();
            spiral.IncrementAngle();
            spiral.GetCurrentY().Should().Be(1);
        }

        [Test]
        public void ArchimedeanSpiral_IncrementAngle_ShouldIncrement()
        {
            var spiral = new ArchimedeanSpiral();
            spiral.IncrementAngle();
            spiral.Angle.Should().Be(spiral.Step);
        }

        [Test]
        public void CircularCloudLayouter_RectanglesSet_ShouldNotBeNull() =>
            new CircularCloudLayouter(new Point(0, 0)).Rectangles.Should().NotBeNull();

        [Test]
        public void CircularCloudLayouter_RectanglesSet_ShouldBeZeroLengthAtStart() =>
            new CircularCloudLayouter(new Point(0, 0)).Rectangles.Count.Should().Be(0);

        [Test]
        public void PutNextRectangle_FirstRect_ShouldBeInCenter() =>
            new CircularCloudLayouter(new Point(0, 0))
                .PutNextRectangle(new Size(10, 3))
                .Location.ShouldBeEquivalentTo(new Point(0, 0));

        [Test]
        public void PutNextRectangle_ShouldAddOneRectangle()
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
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
        public void PutNextRectangle_AddingRect_ShouldIncrementAngle()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(5, 6));
            layouter.Spiral.Angle.Should().Be(layouter.Spiral.Step);
        }

        [Test]
        public void PutNextRectangle_AddingRect_ShouldSetNewCurrentCoords()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(5, 6));
            layouter.CurrentCoords.ShouldBeEquivalentTo(new Point(1, 1));
        }

        [Test]
        public void PutNextRectangle_ShouldNotAddIntersectingRect()
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(10, 3));
            layouter.PutNextRectangle(new Size(5, 4));
            layouter.Rectangles.Count.Should().Be(1);
        }
    }
}