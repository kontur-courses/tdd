using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    internal class CircularCloudLayouterMainRequirementsTests
    {
        private Point _center;
        private Rectangle[] _rectangles;
        private string _failedTestsDirectory = "FailedTest";

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(_failedTestsDirectory);
        }

        [SetUp]
        public void SetUp()
        {
            _center = new Point(400, 400);
            var minRectangleWidth = 30;
            var maxRectangleWidth = 70;
            var minRectangleHeight = 20;
            var maxRectangleHeight = 50;
            var rectanglesCount = 1000;

            _rectangles = new Rectangle[rectanglesCount];
            var circularCloudLayouter = new CircularCloudLayouter(_center);
            for (var i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = CircularCloudLayouterWorker.GetNextRectangleSize(
                    minRectangleWidth,
                    maxRectangleWidth,
                    minRectangleHeight,
                    maxRectangleHeight);
                _rectangles[i] = circularCloudLayouter.PutNextRectangle(nextRectangleSize);
            }
        }

        [TestCase(0.7, 1000)]
        [Repeat(10)]
        public void ShouldPlaceRectanglesInCircle(double expectedCoverageRatio, int gridSize)
        {
            var maxRadius = _rectangles.Max(r => r.GetMaxDistanceFromPointToRectangleAngles(_center));
            var step = (2 * maxRadius) / gridSize;

            var occupancyGrid = GetOccupancyGrid(gridSize, maxRadius, step);

            var actualCoverageRatio = GetOccupancyGridRatio(occupancyGrid, maxRadius, step);
            actualCoverageRatio.Should().BeGreaterThanOrEqualTo(expectedCoverageRatio);
        }

        [TestCase(15)]
        [Repeat(10)]
        public void ShouldPlaceCenterOfMassOfRectanglesNearCenter(int tolerance)
        {
            var centerX = _rectangles.Average(r => r.Left + r.Width / 2.0);
            var centerY = _rectangles.Average(r => r.Top + r.Height / 2.0);
            var actualCenter = new Point((int)centerX, (int)centerY);

            var distance = Math.Sqrt(Math.Pow(actualCenter.X - _center.X, 2)
                                     + Math.Pow(actualCenter.Y - _center.Y, 2));

            distance.Should().BeLessThanOrEqualTo(tolerance);
        }

        [Test]
        [Repeat(10)]
        public void ShouldPlaceRectanglesWithoutOverlap()
        {
            for (var i = 0; i < _rectangles.Length; i++)
            {
                for (var j = i + 1; j < _rectangles.Length; j++)
                {
                    Assert.That(
                        _rectangles[i].IntersectsWith(_rectangles[j]) == false,
                        $"Прямоугольники пересекаются:\n" +
                        $"{_rectangles[i].ToString()}\n" +
                        $"{_rectangles[j].ToString()}");
                }
            }
        }

        [TearDown]
        public void Cleanup()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0)
            {
                return;
            }

            var name = $"{TestContext.CurrentContext.Test.Name}.png";
            var path = Path.Combine(_failedTestsDirectory, name);
            ImageSaver.SaveFile(CircularCloudLayouterPainter.Draw(_rectangles), path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            if (Directory.Exists(_failedTestsDirectory)
                && Directory.GetFiles(_failedTestsDirectory).Length == 0)
            {
                Directory.Delete(_failedTestsDirectory);
            }
        }

        private (int start, int end) GetGridIndexesInterval(
            int rectangleStartValue,
            int rectangleCorrespondingSize,
            double maxRadius,
            double step)
        {
            var start = (int)((rectangleStartValue - _center.X + maxRadius) / step);
            var end = (int)((rectangleStartValue + rectangleCorrespondingSize - _center.X + maxRadius) / step);
            return (start, end);
        }

        private bool[,] GetOccupancyGrid(int gridSize, double maxRadius, double step)
        {
            var result = new bool[gridSize, gridSize];
            foreach (var rect in _rectangles)
            {
                var xInterval = GetGridIndexesInterval(rect.X, rect.Width, maxRadius, step);
                var yInterval = GetGridIndexesInterval(rect.Y, rect.Height, maxRadius, step);
                for (var x = xInterval.start; x <= xInterval.end; x++)
                {
                    for (var y = yInterval.start; y <= yInterval.end; y++)
                    {
                        result[x, y] = true;
                    }
                }
            }
            return result;
        }

        private double GetOccupancyGridRatio(bool[,] occupancyGrid, double maxRadius, double step)
        {
            var totalCellsInsideCircle = 0;
            var coveredCellsInsideCircle = 0;
            for (var x = 0; x < occupancyGrid.GetLength(0); x++)
            {
                for (var y = 0; y < occupancyGrid.GetLength(0); y++)
                {
                    var cellCenterX = x * step - maxRadius + _center.X;
                    var cellCenterY = y * step - maxRadius + _center.Y;

                    var distance = Math.Sqrt(
                        Math.Pow(cellCenterX - _center.X, 2) + Math.Pow(cellCenterY - _center.Y, 2));

                    if (distance > maxRadius)
                    {
                        continue;
                    }

                    totalCellsInsideCircle += 1;
                    if (occupancyGrid[x, y])
                    {
                        coveredCellsInsideCircle += 1;
                    }
                }
            }
            return (double)coveredCellsInsideCircle / totalCellsInsideCircle;
        }
    }
}