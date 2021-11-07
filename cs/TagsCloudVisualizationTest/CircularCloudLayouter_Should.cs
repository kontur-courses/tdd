using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        
        [Test]
        public void CircularCloudLayouter_PutNextRectangle_DoNotThrowAnyExceptionOnPositiveSize()
        {
            Invoking(() => layouter.PutNextRectangle(new Size(1, 1))).Should().NotThrow();
        }
        
        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void CircularCloudLayouterConstructor_PutNextRectangle_ThrowArgumentExceptionOnNonPositiveWidthOrHeight(int width, int height)
        {
            Invoking(() => layouter.PutNextRectangle(new Size(width, height)))
                .Should()
                .Throw<ArgumentException>($"width = {width}, height = {height}");
        }
        
        [Test]
        public void CircularCloudLayouterConstructor_DoNotThrowAnyExceptionOnNonNullParameter()
        {
            var builder = CircularCloudLayouterBuilder
                .ACircularCloudLayouter()
                .WithCenterAt(Point.Empty)
                .WithDegreesParameter(1)
                .WithDensityParameter(1);

            Invoking(() => builder.Build()).Should().NotThrow();
        }
        
        [Test]
        public void CircularCloudLayouterConstructor_ThrowArgumentExceptionOnNullParameter()
        {
            Invoking(() => new CircularCloudLayouter(null))
                .Should()
                .Throw<ArgumentException>();
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
        
        [Test]
        public void CircularCloudLayouter_PutNextRectangle_RectangleCircularAutoTest()
        {
            for (var j = 0; j < 100; j++)
            {
                layouter = CircularCloudLayouterBuilder
                    .ACircularCloudLayouter()
                    .WithCenterAt(Point.Empty)
                    .Build();
                
                var rectangles = new HashSet<Rectangle>();
                var rnd = new Random();

                for (var i = 0; i < 1000; i++)
                {
                    rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(25, 300), rnd.Next(25, 300))));
                }

                var smallCircleRadius = rectangles
                    .Select(x => new[]
                        {
                            Math.Abs(x.Top),
                            Math.Abs(x.Bottom),
                            Math.Abs(x.Left),
                            Math.Abs(x.Right)
                        }.Max()
                    ).Max();
                
                var bigCircleRadius = rectangles
                    .Select(x => new []
                        {
                            x.Location.MetricTo(Point.Empty),
                            new Point(x.X, x.Y + x.Height).MetricTo(Point.Empty),
                            new Point(x.X + x.Width, x.Y).MetricTo(Point.Empty),
                            new Point(x.X + x.Width, x.Y + x.Height).MetricTo(Point.Empty)
                        }.Max()
                    ).Max();

                var outsideCircleSquare = Math.Sqrt(2 * Math.Pow(bigCircleRadius, 2));
                
                
                (outsideCircleSquare / bigCircleRadius).Should().BeGreaterThan(bigCircleRadius / smallCircleRadius * 1.35);
            }
        }
        
        [Test]
        public void CircularCloudLayouter_PutNextRectangle_RectangleCircularAutoTest()
        {
            for (var j = 0; j < 100; j++)
            {
                layouter = CircularCloudLayouterBuilder
                    .ACircularCloudLayouter()
                    .WithCenterAt(Point.Empty)
                    .Build();
                
                var rectangles = new HashSet<Rectangle>();
                var rnd = new Random();

                for (var i = 0; i < 1000; i++)
                {
                    rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(25, 300), rnd.Next(25, 300))));
                }

                var smallCircleRadius = rectangles
                    .Select(x => new[]
                        {
                            Math.Abs(x.Top),
                            Math.Abs(x.Bottom),
                            Math.Abs(x.Left),
                            Math.Abs(x.Right)
                        }.Max()
                    ).Max();
                
                var bigCircleRadius = rectangles
                    .Select(x => new []
                        {
                            x.Location.MetricTo(Point.Empty),
                            new Point(x.X, x.Y + x.Height).MetricTo(Point.Empty),
                            new Point(x.X + x.Width, x.Y).MetricTo(Point.Empty),
                            new Point(x.X + x.Width, x.Y + x.Height).MetricTo(Point.Empty)
                        }.Max()
                    ).Max();

                var outsideCircleSquare = Math.Sqrt(2 * Math.Pow(bigCircleRadius, 2));
                
                
                (outsideCircleSquare / bigCircleRadius).Should().BeGreaterThan(bigCircleRadius / smallCircleRadius * 1.35);
            }
        }
    }
}