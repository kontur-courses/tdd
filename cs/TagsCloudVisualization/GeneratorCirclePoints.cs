using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class EternityGeneratorCirclePoints : IGeneratorCloudPoints
    {
        private readonly Point centerPoint;
        private readonly IEnumerator<Point> generatorPoints;
        private int radiusOfCircle = 1;


        public EternityGeneratorCirclePoints(Point centerPoint)
        {
            this.centerPoint = centerPoint;
            generatorPoints = GetAllPointsInSpiralWay();
        }

        public EternityGeneratorCirclePoints(int x, int y)
        {
            this.centerPoint = new Point(x, y);
            generatorPoints = GetAllPointsInSpiralWay();
        }

        public Point GetCenterPoint()
            => centerPoint;

        private IEnumerable<Point> GetAllPointsInCirclePerimeter()
        {
            for (var deltaX = -radiusOfCircle; deltaX <= radiusOfCircle; deltaX++)
            {
                var deltaY = (int)Math.Sqrt(radiusOfCircle * radiusOfCircle - deltaX * deltaX);
                var x = deltaX + centerPoint.X;
                yield return new Point(x, deltaY + centerPoint.Y);
                if (deltaY > 0)
                    yield return new Point(x, -deltaY + centerPoint.Y);
            }
        }

        private IEnumerator<Point> GetAllPointsInSpiralWay()
        {
            yield return centerPoint;
            while (true)
            {
                foreach (var nextPoint in GetAllPointsInCirclePerimeter())
                    yield return nextPoint;
                radiusOfCircle++;
            }
        }

        public Point GetNextPoint()
        {
            generatorPoints.MoveNext();
            return generatorPoints.Current;
        }
    }

    [TestFixture]
    public class EternityGeneratorCirclePoints_Should
    {
        [TestCase(0, 0, TestName = "start point is zero point")]
        [TestCase(0, 1, TestName = "start point in ox axis")]
        [TestCase(1, 1, TestName = "start point has positive x and y")]
        [TestCase(-1, -1, TestName = "start point has negative x and y")]
        public void GetNextPoint_ShouldReturnCenterPoint_WhenExecutesAtFirst(int x, int y)
        {
            var centerPoint = new Point(x, y);
            var eternityGeneratorPoints = new EternityGeneratorCirclePoints(centerPoint);

            eternityGeneratorPoints.GetNextPoint()
                .ShouldBeEquivalentTo(centerPoint);
        }

        [Test, Timeout(1000)]
        public void GetNextPoint_ShouldWorksFast_WhenExecutesManyTimes()
        {
            var eternityGeneratorPoints = new EternityGeneratorCirclePoints(0, 0);

            for (var index = 0; index < 1000000; index++)
                eternityGeneratorPoints.GetNextPoint();
        }

        [Test, Timeout(10000)]
        public void GetNextPoint_ShouldNotReturnSamePointsInARow_WhenExecutesManyTimes()
        {
            var eternityGeneratorPoints = new EternityGeneratorCirclePoints(0, 0);

            var points = GetNextPointManyTimes(eternityGeneratorPoints, 100000).ToList();

            for (var index = 1; index < points.Count; index++)
                Assert.AreNotEqual(points[index - 1], points[index]);
        }

        private IEnumerable<Point> GetNextPointManyTimes(EternityGeneratorCirclePoints generatorCirclePoints, int countPoints)
        {
            for (var index = 0; index < countPoints; index++)
                yield return generatorCirclePoints.GetNextPoint();
        }

        [Test]
        public void GetNextPoint_ShouldReturnDifferentPoints_WhenExecutesManyTimes()
        {
            var eternityGeneratorPoints = new EternityGeneratorCirclePoints(0, 0);
            var countItems = 10000;

            var differentPointsCount = GetNextPointManyTimes(eternityGeneratorPoints, countItems)
                .Distinct()
                .Count();

            differentPointsCount.Should().Be(countItems);
        }

        [Test]
        public void GetNextPoint_ShouldReturnPointWithLengthToCentreMoreOrEqualThanAverage_WhenExecutesManyTimes()
        {
            var centerPoint = new Point(0, 0);
            var eternityGeneratorPoints = new EternityGeneratorCirclePoints(0, 0);
            var currentAverageLength = 0.0;

            var lengthsToCentre = GetNextPointManyTimes(eternityGeneratorPoints, 1000000)
                .Select(x => Math.Sqrt(x.X * x.X + x.Y * x.Y))
                .ToList();

            for (var index = 0; index < lengthsToCentre.Count; index++)
            {
                var currentLengthToCentre = lengthsToCentre[index];
                currentLengthToCentre.Should().BeGreaterOrEqualTo(currentAverageLength);
                currentAverageLength = (currentAverageLength * index + currentLengthToCentre) / (index + 1);
            }
        }

        [Test]
        public void GetNextPoint_ReturnsCorrectPointOnFirstGenerator_WhenSecondGeneratorIsCreated()
        {
            var firstGenerator = new EternityGeneratorCirclePoints(0, 0);
            var secondGenerator = new EternityGeneratorCirclePoints(1, 1);

            firstGenerator.GetNextPoint()
                .ShouldBeEquivalentTo(firstGenerator.GetCenterPoint());
        }

        [Test]
        public void GetNextPoint_ReturnsCorrectPointOnSecondGenerator_WhenTwoGeneratorsAreCreated()
        {
            var firstGenerator = new EternityGeneratorCirclePoints(0, 0);
            var secondGenerator = new EternityGeneratorCirclePoints(1, 1);

            secondGenerator.GetNextPoint()
                .ShouldBeEquivalentTo(secondGenerator.GetCenterPoint());
        }

        [Test]
        public void GetCenterPoint_ReturnsCorrectPoint_WhenGeneratorIsCreated()
        {
            var centerPoint = new Point(10, 10);
            var generatorPoints = new EternityGeneratorCirclePoints(centerPoint);

            generatorPoints.GetCenterPoint()
                .ShouldBeEquivalentTo(centerPoint);
        }

        [Test]
        public void GetCenterPoint_ReturnsCorrectPoint_WhenGetNextPointExecutedManyTimes()
        {
            var centerPoint = new Point(10, 10);
            var generatorPoints = new EternityGeneratorCirclePoints(centerPoint);

            for (var index = 0; index < 10000; index++)
                generatorPoints.GetNextPoint();

            generatorPoints.GetCenterPoint()
                .ShouldBeEquivalentTo(centerPoint);
        }

        [Test]
        public void GetNextPoint_IncreasesDistanceToCenterNotTooFast_WhenExecutesManyTimes()
        {
            var generatorPoints = new EternityGeneratorCirclePoints(0, 0);
            
            var distances = GetNextPointManyTimes(generatorPoints, 100000)
                .Select(x => Math.Sqrt(x.X * x.X + x.Y * x.Y))
                .ToList();

            for (var index = 1; index < distances.Count; index++)
                (distances[index] - 1).Should().BeLessOrEqualTo(distances[index - 1]);
        }

        [Test]
        public void GetNextPoint_IncreasesAverageDistanceNotSoFast_WhenExecutesManyTimes()
        {
            var firstPartCountPoints = 1000;
            var allCountPoints = firstPartCountPoints * 2;
            var generator = new EternityGeneratorCirclePoints(0, 0);

            var distances = GetNextPointManyTimes(generator, allCountPoints)
                .Select(x => Math.Sqrt(x.X * x.X + x.Y * x.Y))
                .ToList();

            var firstPartAvetageDistance = distances.Take(firstPartCountPoints).Sum() / firstPartCountPoints;
            var allPointsAverageDistance = distances.Sum() / allCountPoints;

            (firstPartAvetageDistance * 2).Should()
                .BeGreaterOrEqualTo(allPointsAverageDistance);
        }

    }
}
