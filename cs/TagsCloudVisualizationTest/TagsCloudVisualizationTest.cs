using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest
{
    public class TagsCloudVisualizationTest
    {
        private Point center = new(10, 20);

        [Test]
        public void GetPoints_FirstPoint_IsSpiralCenter()
        {
            var spiral = new ArchimedeanSpiral(center);

            var firstPoint = spiral.GetPoints().First();

            firstPoint.Should().Be(center);
        }

        [Test]
        public void GetPoints_ReturnsPointsLyingOnArchimedeanSpiral()
        {

            var spiral = new ArchimedeanSpiral(center, radiusStep: 4, angleStep: Math.PI / 2);
            var expectedPoints = new[]
            {
                new Point(10, 20),
                new Point(10, 21), 
                new Point(8, 20),  
                new Point(10, 17), 
                new Point(14, 20)  
            };

            var actualPoints = spiral.GetPoints().Take(5);

            actualPoints.Should().Equal(expectedPoints);
        }

        [Test]
        public void GetPoints_PointsMoveAwayFromCenter()
        {
            var center = new Point(0, 0);
            var spiral = new ArchimedeanSpiral(center);
            var points = spiral.GetPoints().Take(5_000).ToList();

            var earlyDistance = DistanceBetween(center, points[100]);
            var lateDistance = DistanceBetween(center, points[4_999]);

            lateDistance.Should().BeGreaterThan(earlyDistance);
        }

        [Test]
        public void GetPoints_IsEndless()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));

            var points = spiral.GetPoints().Take(10_000).ToList();

            points.Should().HaveCount(10_000);
        }

        [Test]
        public void GetPoints_CalledTwice_ReturnsSameSequence()
        {
            var spiral = new ArchimedeanSpiral(new Point(5, 5));
            var firstCall = spiral.GetPoints().Take(100).ToList();

            var secondCall = spiral.GetPoints().Take(100).ToList();

            secondCall.Should().Equal(firstCall);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_NonPositiveRadiusStep_Throws(double radiusStep)
        {
            Action create = () => new ArchimedeanSpiral(new Point(0, 0), radiusStep: radiusStep);

            create.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_NonPositiveAngleStep_Throws(double angleStep)
        {
            Action create = () => new ArchimedeanSpiral(new Point(0, 0), angleStep: angleStep);

            create.Should().Throw<ArgumentOutOfRangeException>();
        }

        private static double DistanceBetween(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

}
