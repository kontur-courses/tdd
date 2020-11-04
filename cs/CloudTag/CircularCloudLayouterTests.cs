using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace CloudTag
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private readonly Point layouterCenter = new Point(400, 400);

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(layouterCenter);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
                return;

            foreach (var filePath in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
                "Failures Tests Layouts")))
                File.Delete(filePath);

            var outWriter = TestContext.Out;
            var pic = CloudPainter.DrawTagCloud(Pens.Blue, layouter);
            var testName = $"{TestContext.CurrentContext.Test.MethodName} {TestContext.CurrentContext.Test.Name}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Failures Tests Layouts",
                $"{testName}.png");

            outWriter.WriteLine($"Tag cloud visualization saved to file Failures Tests Layouts/{testName}");

            pic.Save(path);
        }

        [Test]
        public void Constructor_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(0, 0)));
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangle_FirstAdding()
        {
            var size = new Size(10, 10);
            var actualRectangle = layouter.PutNextRectangle(size);
            var expectedRectangle = new Rectangle(Point.Empty, size);
            expectedRectangle.SetCenter(layouterCenter);
            actualRectangle.Should().Be(expectedRectangle);
        }

        [TestCase(2, TestName = "2 rect")]
        [TestCase(10, TestName = "10 rect")]
        [TestCase(80, TestName = "80 rect")]
        [TestCase(200, TestName = "200 rect")]
        public void PutNextRectangle_ShouldNotIntersect_RectSameSize(int rectCount)
        {
            var size = new Size(10, 10);
            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(size);

            var rectsInLayout = layouter.GetRectangles().ToArray();
            for (var i = 0; i < rectCount; i++)
            {
                rectsInLayout[i].Should().Match<Rectangle>(
                    rect1 => !rectsInLayout
                        .Skip(i + 1)
                        .Any(rect1.IntersectsWith)
                );
            }
        }

        [Test]
        public void PutNextRectangle_ReturnEmptyRect_SizeIsZero()
        {
            var actualRect = layouter.PutNextRectangle(new Size(0, 0));
            actualRect.Should().Be(Rectangle.Empty);
        }

        [TestCase(1, TestName = "1 rect")]
        [TestCase(20, TestName = "20 rect")]
        public void GetRectangles_ReturnEmptyCollection_AddedRectsWithZeroSize(int rectCount)
        {
            var size = new Size(0, 0);
            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(size);

            layouter.GetRectangles().Should().BeEmpty();
        }

        [Test]
        public void PutNextRectangle_ReturnsEmptyRect_OneOfSidesIs0()
        {
            var actualRect1 = layouter.PutNextRectangle(new Size(0, 10));
            var actualRect2 = layouter.PutNextRectangle(new Size(10, 0));

            actualRect1.Should().Be(Rectangle.Empty);
            actualRect2.Should().Be(Rectangle.Empty);
        }

        [Timeout(1000)]
        [Test]
        public void PutNextRectangle_PerformanceTest_5000RectSameSize()
        {
            var size = new Size(10, 10);
            for (var i = 0; i < 5000; i++)
                layouter.PutNextRectangle(size);
        }

        [TestCase(10, TestName = "10 rect")]
        [TestCase(20, TestName = "20 rect")]
        [TestCase(50, TestName = "50 rect")]
        public void PutNextRectangle_LayoutShouldBeTight_RectsDifferentSize(int rectCount)
        {
            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(new Size((i * 15 + 25) % 90 + 30, (i * 10 + 20) % 60 + 20));

            var maxDistanceToCenter = layouter.GetRectangles().Max(rectangle =>
                Math.Sqrt(Math.Pow(rectangle.Location.X - layouterCenter.X, 2) +
                          Math.Pow(rectangle.Location.Y - layouterCenter.Y, 2)));

            var commonArea = layouter.GetRectangles().Sum(rectangle => rectangle.Height * rectangle.Width);
            var borderCircleArea = maxDistanceToCenter * maxDistanceToCenter * Math.PI;

            (borderCircleArea - commonArea).Should().BeLessThan(2 * borderCircleArea / 3);
        }

        [TestCase(10, TestName = "10 rect")]
        [TestCase(20, TestName = "20 rect")]
        [TestCase(50, TestName = "50 rect")]
        public void PutNextRectangle_LayoutShouldBeRoundShape_RectsDifferentSize(int rectCount)
        {
            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(new Size((i * 15 + 25) % 90 + 30, (i * 5 + 20) % 60 + 20));

            var commonArea = layouter.GetRectangles().Sum(rectangle => rectangle.Height * rectangle.Width);
            var suggestedCircleRadius = Math.Sqrt(commonArea / Math.PI);

            var l = new List<double>();


            foreach (var rectangle in layouter.GetRectangles())
            {
                var distanceToCenter = Math.Sqrt(Math.Pow(rectangle.Location.X - layouterCenter.X, 2) +
                                                 Math.Pow(rectangle.Location.Y - layouterCenter.Y, 2));

                l.Add(distanceToCenter);

                (distanceToCenter - suggestedCircleRadius).Should().BeLessThan(suggestedCircleRadius / 2);
            }
        }
    }
}