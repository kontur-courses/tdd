using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class PointsDistance_Should
    {
        [TestCase(1, 5, 1, 5, TestName = "When points are equivalent")]
        [TestCase(-7, 7, -7, 7, TestName = "When points are equivalent")]
        public void GetDistanceBetweenPoint_ZeroDistance(int firstX, int firstY,
            int secondX, int secondY)
        {
            var firstPoint = new Point(firstX, firstY);
            var secondPoint = new Point(secondX, secondY);

            var distanceBetweenPoints = PointsDistance.GetCeilingDistanceBetweenPoints(firstPoint, secondPoint);

            distanceBetweenPoints.Should().Be(0);
        }

        [TestCase(5, -1, 1, 2, 5,
            TestName = "When points are different")]
        [TestCase(5, -1, 5, 2, 3,
            TestName = "When first coordinates are equal")]
        [TestCase(3, -1, 5, -1, 2,
            TestName = "When second coordinates are equal")]
        [TestCase(5, -1, 2, -3, 4,
            TestName = "Positive ceiling distance when ceiling is needed")]
        public void GetDistanceBetweenPoint_PositiveDistance(int firstX, int firstY,
            int secondX, int secondY, int expectedDistance)
        {
            var firstPoint = new Point(firstX, firstY);
            var secondPoint = new Point(secondX, secondY);

            var distanceBetweenPoints = PointsDistance.GetCeilingDistanceBetweenPoints(firstPoint, secondPoint);

            distanceBetweenPoints.Should().Be(expectedDistance);
        }
    }
}