using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class GeometryHelper_Tests
    {
        private const float Delta = 1e-3f;

        [TestCase(1, 1, 1, 1)]
        [TestCase(0, 0, 1, 1)]
        [TestCase(1, 1, 0, 0)]
        [TestCase(1, 0, 0, 1)]
        [TestCase(0, 1, 1, 0)]
        [TestCase(0, -1, -1, 0)]
        [TestCase(0, 0, -1, -1)]
        [TestCase(-1, -1, 0, 0)]
        public void GetDistanceBetweenPoints_Always_NotNegative(float x1, float y1, float x2, float y2)
        {
            var point1 = new PointF(x1, y1);
            var point2 = new PointF(x2, y2);

            var distance = GeometryHelper.GetDistanceBetweenPoints(point1, point2);

            distance.Should().BeGreaterThanOrEqualTo(0, "Distance can't to be negative");
        }


        [TestCase(-4, 3, 0, 0, 5)]
        [TestCase(2, 3, 2, 3, 0)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(100, 124, 140, 133, 41)]
        public void GetDistanceBetweenPoints_ShouldBe_ExpectedValue
            (float x1, float y1, float x2, float y2, float expectedDistance)
        {
            var point1 = new PointF(x1, y1);
            var point2 = new PointF(x2, y2);

            var actualDistance = GeometryHelper.GetDistanceBetweenPoints(point1, point2);

            actualDistance.Should().BeApproximately(expectedDistance, Delta);
        }


        [TestCase(0, 0, 2, 2, -1, -1)]
        [TestCase(0, 0, 3, 3, -2, -2)]
        [TestCase(2, 0, 3, 3, 0, -2)]
        [TestCase(0, 0, 0, 0, 0, 0)]
        public void GetRectangleLocation_ShouldBe_ExpectedValue
            (float xCentre, float yCentre, int width, int height, int xExpect, int yExpect)
        {
            var centralPoint = new PointF(xCentre, yCentre);
            var size = new Size(width, height);
            var expectedLocation = new Point(xExpect, yExpect);

            var actualLocation = GeometryHelper.GetRectangleLocationFromCentre(centralPoint, size);

            actualLocation.Should().Be(expectedLocation);
        }

        [TestCase(0, 0, 2, 2, 1, 1)]
        [TestCase(1, 1, 3, 2, 2.5f, 2)]
        [TestCase(0, 0, 0, 0, 0, 0)]
        [TestCase(-5, -3, 1, 3, -4.5f, -1.5f)]
        public void GetRectangleCentre_ShouldBe_ExpectedValue
            (int x, int y, int width, int height, float xExpect, float yExpect)
        {
            var rectangle = new Rectangle(x, y, width, height);
            var expectedCentre = new PointF(xExpect, yExpect);

            var actualCentre = GeometryHelper.GetRectangleCentre(rectangle);

            actualCentre.X.Should().BeApproximately(expectedCentre.X, Delta);
            actualCentre.Y.Should().BeApproximately(expectedCentre.Y, Delta);
        }
    }
}