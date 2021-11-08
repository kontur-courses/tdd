using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using static FluentAssertions.FluentActions;


namespace TestProject1
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private readonly HashSet<Rectangle> rectangles = new HashSet<Rectangle>();
        private readonly HashSet<string> testsToSaveLayout = new HashSet<string>
        {
            "AutoTest_CircularCloudLayouter_PutNextRectangle_RectanglesShouldBeCompact",
            "AutoTest_CircularCloudLayouter_PutNextRectangle_RectanglesShouldBeCircular",
            "AutoTest_CircularCloudLayouter_PutNextRectangle_RectanglesShouldNotIntersect",
            "PutNextRectangle_RectangleOnTheMiddle_AfterAdditionSingleRectangle"
        };

        [SetUp]
        public void SetUp()
        {
            rectangles.Clear();
            layouter = CircularCloudLayouterBuilder
                .ACircularCloudLayouter()
                .WithCenterAt(Point.Empty)
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed 
                && testsToSaveLayout.Contains(context.Test.MethodName))
            {
                var filePath = Path.GetFullPath($"..\\..\\Failure__{context.Test.MethodName}.jpg");
                
                try
                {
                    RectanglePainter
                        .GetBitmapWithRectangles(rectangles)  // Throw ArgumentException while creating too big bmp.
                        .Save(filePath, ImageFormat.Jpeg);
                
                    TestContext.Out.WriteLine($"Tag cloud visualization saved to file <{filePath}>");
                }
                catch (ArgumentException e)
                {
                    TestContext.Out.WriteLine($"Tag cloud too big to save to file");
                }
            }
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
                .WithDegreesDelta(1)
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
        public void PutNextRectangle_RectangleOnTheMiddle_AfterAdditionSingleRectangle(int x, int y)
        {
            var layouter2 = CircularCloudLayouterBuilder
                .ACircularCloudLayouter()
                .WithCenterAt(new Point(x, y))
                .Build();
            var rectangleSize = new Size(100, 100);

            var actual = layouter2.PutNextRectangle(rectangleSize);
            rectangles.Add(actual);

            actual.Should().Be(
                new Rectangle(x - 50, y - 50, 100, 100), 
                $"Point is {Point.Empty.ToString()} and rectangle size is {rectangleSize.ToString()}"
                );
        }

        [Test]
        [Repeat(20)]
        public void AutoTest_CircularCloudLayouter_PutNextRectangle_RectanglesShouldNotIntersect()
        {
            var rnd = new Random();
            var lastRectangle = layouter.PutNextRectangle(new Size(200, 200));

            for (var i = 0; i < 1000; i++)
            {
                var randomSize = new Size(rnd.Next(1, 1000), rnd.Next(1, 1000));
                var rectangle = layouter.PutNextRectangle(randomSize);
                rectangles.Add(rectangle);

                lastRectangle.IntersectsWith(rectangle).Should().BeFalse($"on try {i}");
            }
        }
        
        [Test]
        [Repeat(100)]
        public void AutoTest_CircularCloudLayouter_PutNextRectangle_RectanglesShouldBeCircular()
        {
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
            var actualRatio = outsideCircleSquare / bigCircleRadius;
            var expectedRatio = bigCircleRadius / smallCircleRadius;

            actualRatio.Should().BeGreaterThan(expectedRatio * 1.3);
        }
        
        [Test]
        [Repeat(100)]
        public void AutoTest_CircularCloudLayouter_PutNextRectangle_RectanglesShouldBeCompact()
        {
            var rnd = new Random();

            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(3, 105-i), rnd.Next(3, 105-i))));
            }

            var rectanglesSquareSum = (ulong)rectangles.Select(x => x.Height * x.Width).Sum();
            var outsideRectangleSquare = (ulong)Math.Abs(rectangles.Max(x => x.Right) - rectangles.Min(x => x.Left)) 
                                         * (ulong)Math.Abs(rectangles.Max(x => x.Bottom) - rectangles.Min(x => x.Top));

            var actualRatio = (double)rectanglesSquareSum / outsideRectangleSquare;
            var expectedMaxRatio = 0.16;
            
            // To check the reaction to test failure, uncomment the next line.
            // expectedMaxRatio = 2d;

            actualRatio.Should().BeGreaterThan(expectedMaxRatio);
        }
    }
}