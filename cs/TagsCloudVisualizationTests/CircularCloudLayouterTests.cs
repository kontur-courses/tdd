using System;
using System.Collections.Generic;
using NUnit.Framework;
using TagsCloudVisualization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(100, 100);
            layouter = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var visualizator = new CloudVisualizator(1500, 1500, Color.DarkRed, Color.Aquamarine);

            if (testStatus == TestStatus.Failed)
            {
                var rectangles = (List<Rectangle>)typeof(CircularCloudLayouter)
                    .GetField("rectangles", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(layouter);

                foreach (var rectangle in rectangles)
                    visualizator.DrawRectangle(rectangle);

                var folderName = "FailedTestsResults";
                if (!Directory.Exists(folderName))
                    Directory.CreateDirectory(folderName);

                var fullName = $"{folderName}\\{TestContext.CurrentContext.Test.FullName}.png";
                visualizator.SaveImage(fullName, ImageFormat.Png);
                TestContext.Out.WriteLine($"Tag cloud visualization saved to file {Environment.CurrentDirectory}\\{fullName}");
            }
        }

        [TestCase(0, 0, TestName = "With Zero Point")]
        [TestCase(100, 100, TestName = "With Non Zero Point")]
        public void CtorShouldSetCenter(int x, int y)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));
            var expected = new Point(x, y);

            layouter.Center.Should().Be(expected);
        }

        [TestCaseSource(nameof(CasesForPutNextRectangles))]
        public void PutNextRectangles(int rectanglesNumber, Point expectedLocation)
        {
            var size = new Size(50, 50);

            for (var i = 0; i < rectanglesNumber; i++)
                layouter.PutNextRectangle(size);
            var rectangle = layouter.PutNextRectangle(size);

            rectangle.Location.Should().Be(expectedLocation);
        }

        private static IEnumerable<TestCaseData> CasesForPutNextRectangles
        {
            get
            {
                yield return new TestCaseData(0, new Point(75, 75))
                    .SetName("First Rectangle Should Be Center");
                yield return new TestCaseData(1, new Point(75, 25))
                    .SetName("Second Rectangle Should Be Over First");
                yield return new TestCaseData(4, new Point(25, 75))
                    .SetName("Rectangle Should Go Clockwise");
            }
        }

        [TestCaseSource(nameof(CasesForPutNextRectangleThrows))]
        public void PutNextRectangle_ShouldThrows(Size rectangleSize)
        {
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(rectangleSize));
        }

        private static IEnumerable<TestCaseData> CasesForPutNextRectangleThrows
        {
            get
            {
                yield return new TestCaseData(new Size(0, 1))
                    .SetName("When Size Width Zero");
                yield return new TestCaseData(new Size(1, 0))
                    .SetName("When Size Height Zero");
                yield return new TestCaseData(new Size(0, 0))
                    .SetName("When Size Width And Height Zero");
                yield return new TestCaseData(new Size(-1, 1))
                    .SetName("When Negative Size Width");
                yield return new TestCaseData(new Size(1, -1))
                    .SetName("When Negative Size Height");
                yield return new TestCaseData(new Size(-1, -1))
                    .SetName("When Negative Size Width And Height");
            }
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersectWithOther()
        {
            var rectangles = new List<Rectangle>();
            var size = new Size(10, 10);

            for (var i = 0; i < 50; i++)
                rectangles.Add(layouter.PutNextRectangle(size));
            var rectanglesCopy = rectangles.ToArray();

            rectangles.Count(rect => rectanglesCopy
                .All(r => r.IntersectsWith(rect)))
                .Should().Be(0);
        }

        [Test]
        public void PutNextRectangle_ShouldBeDense()
        {
            var rectangles = new Rectangle[100];
            var random = new Random();

            for (var i = 0; i < 100; i++)
                rectangles[i] = layouter.PutNextRectangle(new Size(random.Next(5, 100), random.Next(5, 100)));

            CalculateDensityRatio(rectangles).Should().BeGreaterOrEqualTo(0.5);
        }

        private double CalculateDensityRatio(Rectangle[] rectangles)
        {
            var circleRadius = GetCircumscribeCircleRadius(rectangles);
            var rectanglesSquare = rectangles.Sum(r => r.Width * r.Height);
            var circleSquare = Math.PI * circleRadius * circleRadius;
            return rectanglesSquare / circleSquare;
        }

        private double GetCircumscribeCircleRadius(Rectangle[] rectangles)
        {
            return rectangles.SelectMany(rect => GetRectangleCorners(rect))
                .Max(current => center.DistanceTo(current));
        }

        private IEnumerable<Point> GetRectangleCorners(Rectangle rectangle)
        {
            yield return new Point(rectangle.Left, rectangle.Top);
            yield return new Point(rectangle.Left, rectangle.Bottom);
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
        }
    }
}
