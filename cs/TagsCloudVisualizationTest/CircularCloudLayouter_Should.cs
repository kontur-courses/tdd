using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using static FluentAssertions.FluentActions;


namespace TestProject1
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = CircularCloudLayouterBuilder
                .ACircularCloudLayouter()
                .WithCenterAt(Point.Empty)
                .Build();
        }
        
        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void CircularCloudLayouterConstructor_DoNotThrowAnyExceptionOnAnyCenterPoint(int x, int y)
        {
            var builder = CircularCloudLayouterBuilder
                .ACircularCloudLayouter()
                .WithCenterAt(new Point(x, y));

            Invoking(() => builder.Build()).Should().NotThrow($"X = {x}; Y = {y}");
        }
        
        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void RectangleOnTheMiddle_PutNextRectangle_AfterAdditionSingleRectangle(int x, int y)
        {
            var layouter2 = CircularCloudLayouterBuilder
                .ACircularCloudLayouter()
                .WithCenterAt(new Point(x, y))
                .Build();
            var rectangleSize = new Size(100, 100);

            var actual = layouter2.PutNextRectangle(rectangleSize);

            actual.Should().Be(
                new Rectangle(x - 50, y - 50, 100, 100), 
                $"Point is {Point.Empty.ToString()} and rectangle size is {rectangleSize.ToString()}"
                );
        }

        [Test]
        public void CircularCloudLayouter_PutNextRectangle_RectangleIntersectAutoTest()
        {
            var rectCount = 1000;
            var rnd = new Random();
            var lastRectangle = layouter.PutNextRectangle(new Size(200, 200));

            for (var i = 0; i < rectCount; i++)
            {
                var randomSize = new Size(rnd.Next(1, 1000), rnd.Next(1, 1000));
                var rectangle = layouter.PutNextRectangle(randomSize);

                lastRectangle.IntersectsWith(rectangle).Should().BeFalse($"on try {i}");
            }
        }
    }
}