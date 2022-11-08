using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization.Core.Tests
{
    internal class ArchimedeanSpiralTests
    {
        [TestCase(5, 5, TestName = "GetNextPoint. x, y > 0 and x = y")]
        [TestCase(5, 15, TestName = "GetNextPoint. x, y > 0 and x != y")]
        [TestCase(0, 0, TestName = "GetNextPoint. x, y = 0")]
        [TestCase(-5, -5, TestName = "GetNextPoint. x, y < 0 and x = y")]
        public void GetNextPoint_WhenAnyArgs_ShouldBePointEqualCenter(int x, int y)
        {
            var center = new Point(x, y);
            new ArchimedeanSpiral(center).GetNextPoint()
                .Should().Be(center);
        }

        [TestCase(5, TestName = "GetNextPoint. Generate 5 points")]
        [TestCase(25, TestName = "GetNextPoint. Generate 25 points")]
        [TestCase(50, TestName = "GetNextPoint. Generate 50 points")]
        [TestCase(100, TestName = "GetNextPoint. Generate 100 points")]
        [TestCase(500, TestName = "GetNextPoint. Generate 500 points")]
        public void GetNextPoint_WhenManyPoints_ShouldAllPointsIsNotEqual(int pointsCount)
        {
            var pointsStorage = new HashSet<Point>();
            var point = new Point(0, 0);
            var spiral = new ArchimedeanSpiral(point);

            for (var i = 0; i < pointsCount; i++)
            {
                point = spiral.GetNextPoint();
                pointsStorage.Should().NotContain(point);
                pointsStorage.Add(point);
            }
        }
    }
}
