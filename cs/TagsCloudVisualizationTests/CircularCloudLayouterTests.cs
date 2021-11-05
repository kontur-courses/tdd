using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private readonly TagsPainter _painter = new();
        private CircularCloudLayouter _sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            if (!Directory.Exists("FailedTests"))
                Directory.CreateDirectory("FailedTests");
        }

        [SetUp]
        public void SetUp()
        {
            _sut = new CircularCloudLayouter(new Point(0, 0));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var filepath =
                    $"{Environment.CurrentDirectory}\\FailedTests\\{TestContext.CurrentContext.Test.Name}.png";
                _painter.SaveToFile(filepath, _sut.GetLayout());
                Console.WriteLine($"Tag cloud visualization saved to file {filepath}");
            }
        }

        [Test]
        public void LayoutIsEmpty_BeforePutNextRectangle()
        {
            _sut.GetLayout().Should().BeEmpty();
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(5000)]
        public void PutNextRectangle_PutRectanglesWithoutIntersections(int layoutLength)
        {
            _sut.GenerateRandomLayout(layoutLength);
            var layout = _sut.GetLayout();

            Func<bool> func = () => layout.Any(x => layout.Any(y => x.IntersectsWith(y) && x != y));

            func().Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_PutFirstRectangleOnCenter()
        {
            _sut.GenerateRandomLayout();

            var rectangle = _sut.GetLayout().First();

            rectangle.Location.Should().BeEquivalentTo(new Point(0 - rectangle.Width / 2, 0 - rectangle.Height / 2));
        }

        [Test]
        public void PutNextRectangle_ThrowArgumentException_RectangleSizeLessZero()
        {
            Action action = () => _sut.PutNextRectangle(new Size(-1, -1));

            action.Should().Throw<ArgumentException>().WithMessage("Rectangle sizes must be great than zero");
        }

        [Test]
        public void PutNextRectangle_ShouldCreateTightLayout()
        {
            _sut.GenerateRandomLayout(1000);
            var radius = _sut.CalculateLayoutRadius();

            var circleArea = Math.PI * radius * radius;
            var rectanglesArea = _sut.GetLayout()
                .Aggregate(0.0, (current, rectangle) => current + rectangle.Height * rectangle.Width);

            rectanglesArea.Should().BeInRange(0.6 * circleArea, circleArea);
        }

        [Test]
        public void PutNextRectangle_ShouldCreateCircleLikeLayout()
        {
            _sut.GenerateRandomLayout(1000);
            var radius = _sut.CalculateLayoutRadius();

            Func<bool> func = () => _sut.GetLayout()
                .Select(rectangle => rectangle.Location + rectangle.Size / 2)
                .Any(rectangleCenter => rectangleCenter.GetDistance(_sut.Center) > radius);

            func().Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldCreateSpiralBasedLayout()
        {
            _sut.GenerateRandomLayout(1000);
            var layout = _sut.GetLayout();
            var oldSpiral = _sut.GetFieldValue<Spiral>("_spiral");
            var spiral = new Spiral(oldSpiral.Center);
            var intersectionCount = 0;
            var pointCount = 0;

            while (spiral.Phi < oldSpiral.Phi)
            {
                var point = spiral.GetNextPoint();
                pointCount++;

                if (layout.Any(x => x.Contains(point)))
                    intersectionCount++;
            }

            intersectionCount.Should().BeInRange((int)(0.8 * pointCount), pointCount);
        }
    }
}