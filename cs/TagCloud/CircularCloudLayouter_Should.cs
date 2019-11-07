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
            new ArchimedeanSpiral(new Point(0, 0)).GetNewPoint().X.Should().Be(0);

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnZeroAtStart() =>
            new ArchimedeanSpiral(new Point(0, 0)).GetNewPoint().Y.Should().Be(0);

        [Test]
        public void ArchimedeanSpiral_GetCurrentX_ShouldReturnCorrectX()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            spiral.GetNewPoint();
            spiral.GetNewPoint().X.Should().Be(1);
        }

        [Test]
        public void ArchimedeanSpiral_GetCurrentY_ShouldReturnCorrectY()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            spiral.GetNewPoint();
            spiral.GetNewPoint().Y.Should().Be(1);
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
        public void PutNextRectangle_AddingRect_ShouldSetNewCurrentCoords()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(5, 6));
            layouter.Spiral.GetNewPoint().ShouldBeEquivalentTo(new Point(1, 1));
        }

        [TestCase(0, 0, 0, 0, 0.0)]
        [TestCase(0, 0, 2, 2, 2.8284271247461903)]
        [TestCase(0, 0, 1, 3, 3.1622776601683795)]
        [TestCase(0, 0, -3, 2, 3.6055512754639891)]
        [TestCase(1, 1, 1, 1, 0)]
        [TestCase(1, 1, 1, -1, 2)]
        [TestCase(1, 1, 4, 1, 3)]
        [TestCase(1, 1, 4, -1, 3.6055512754639891)]
        [TestCase(-5, 13, 4, 6, 11.40175425099138)]
        [TestCase(4, 6, -5, 13, 11.40175425099138)]
        public void GetDistanceBetweenPoints_Should(int xFrom, int yFrom, int xTo, int yTo, double expected)
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.GetDistanceBetweenPoints(new Point(xFrom, yFrom), new Point(xTo, yTo)).Should().Be(expected);
        }

        [TestCase(3, 2, 3.6056)]
        [TestCase(2, 3, 3.6056)]
        [TestCase(5, 5, 7.0711)]
        [TestCase(10, 1, 10.0499)]
        public void GetFurthestDistance_Should(int width, int height, double expected)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(width, height));
            Math.Round(layouter.GetFurthestDistance(), 4).Should().Be(expected);
        }

        [TestCase(1, 0.1469)]
        [TestCase(10, 0.1528)]
        public void GetCloudFullness_Should(int rectCount, double expected)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            for (int i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(new Size(3, 2));
            Math.Round(layouter.GetCloudFullnessPercent(), 4).Should().Be(expected);
        }

        [TestCase(0, 0, 0, 0, -3, -2, 3.6056)]
        [TestCase(0, 0, 1, 1, -3, -2, 2.2361)]
        [TestCase(-1, 1, 2, 2, -3, -2, 3.1623)]
        [TestCase(1, 1, 0, 0, 3, 2, 2.2361)]
        [TestCase(0, 0, 0, 0, 1, 1, 1.4142)]
        public void GetDistanceToRectangle_Should(int xFrom, int yFrom, int xTo, int yTo, int width, int height,
            double expected)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Math.Round(
                    layouter.GetDistanceToRectangle(new Point(xFrom, yFrom),
                        new Rectangle(new Point(xTo, yTo), new Size(width, height))),
                    4)
                .Should().Be(expected);
        }
    }
}