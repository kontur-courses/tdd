using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudTask;
using TagCloudTask.Layouting;
using TagCloudTask.Saving;
using TagCloudTask.Visualization;
using TestDataGenerator;

namespace TagCloudTaskTests
{
    public class CircularLayouterTests
    {
        private CircularLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularLayouter();
        }

        [Test]
        public void Constructor_ShouldCreate_WithSpecifiedCenter(
            [ValueSource(nameof(CloudCenters))] Point center)
        {
            layouter = new CircularLayouter(center);

            layouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangle_AfterFirstPut()
        {
            var expectedRect = new Rectangle(Point.Empty, new Size(10, 10));

            var actualRect = layouter.PutNextRectangle(new Size(10, 10));

            actualRect.Size.Should().BeEquivalentTo(expectedRect.Size);
        }

        [Test]
        public void PutNextRectangle_ShouldThrow_WhenIncorrectSize(
            [ValueSource(nameof(IncorrectRectangleSizes))]
            Size rectSize)
        {
            Action act = () => layouter.PutNextRectangle(rectSize);

            act.Should().Throw<ArgumentException>("Incorrect size:", rectSize);
        }

        [Test]
        public void PutNextRectangle_ShouldAlignRectangleMiddlePointToCenter()
        {
            var rectSize = new Size(10, 10);

            var rect = layouter.PutNextRectangle(rectSize);

            rect.Location
                .Should()
                .BeEquivalentTo(new Point(-5, -5));
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(32)]
        [TestCase(64)]
        [TestCase(128)]
        [TestCase(256)]
        [Timeout(10000)]
        public void PutNextRectangle_ShouldNotIntersect_ForEachRectangle(int n)
        {
            var rectangles = PutNextNRectangles(n);

            IsIntersectExist(rectangles).Should().BeFalse();
        }

        [Test]
        public void CircularLayouter_ShouldPutRectanglesLikeCircle()
        {
            PutNextNRectangles(256);
            var expectedRatio = 0.8d;

            var actualRatio = GetDiametersRatio();

            actualRatio.Should().BeGreaterOrEqualTo(expectedRatio);
        }

        [Test]
        public void CircularLayouter_CloudDensityTest()
        {
            PutNextNRectangles(256);
            var expectedSquaresRatio = 0.5d;

            var actualSquaresRatio = GetSquaresRatio();

            actualSquaresRatio.Should().BeGreaterOrEqualTo(expectedSquaresRatio);
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;

            if (!IsLayoutTest(context))
                return;

            var tagCloud = new TagCloud(layouter,
                new BitmapSaver(), new Visualizer(new Drawer()));

            var path = tagCloud.SaveBitmap(true, true, false);

            Console.WriteLine($"Tag cloud visualization saved to file\n{path}");
        }

        private double GetSquaresRatio()
        {
            var radius = layouter.GetCloudBoundaryRadius();
            var circleSquare = 2 * Math.PI * radius * radius;

            var boundaryBox = layouter.GetRectanglesBoundaryBox();
            var rectanglesSquare = boundaryBox.Width * boundaryBox.Height;

            return rectanglesSquare / circleSquare;
        }

        private bool IsLayoutTest(TestContext context)
        {
            var layoutTestNames = GetLayoutTestNames();

            return layoutTestNames.Contains(context.Test.MethodName);
        }

        private List<string> GetLayoutTestNames()
        {
            var names = new List<string>();

            names.Add(nameof(CircularLayouter_ShouldPutRectanglesLikeCircle));
            names.Add(nameof(PutNextRectangle_ShouldAlignRectangleMiddlePointToCenter));
            names.Add(nameof(PutNextRectangle_ShouldNotIntersect_ForEachRectangle));

            return names;
        }

        private double GetDiametersRatio()
        {
            var boundaryBox = layouter.GetRectanglesBoundaryBox();
            var biggestSide = (double)Math.Max(boundaryBox.Width, boundaryBox.Height);
            var smallestSide = (double)Math.Min(boundaryBox.Width, boundaryBox.Height);

            return smallestSide / biggestSide;
        }

        private List<Rectangle> PutNextNRectangles(int n)
        {
            return RectangleSizeGenerator.GetNextNRandomSizes(n)
                .Select(size => layouter.PutNextRectangle(size)).ToList();
        }

        private static bool IsIntersectExist(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                var rect = rectangles[i];
                for (var j = i + 1; j < rectangles.Count; j++)
                    if (rect.IntersectsWith(rectangles[j]))
                        return true;
            }

            return false;
        }

        private static IEnumerable<Point> CloudCenters()
        {
            yield return Point.Empty;
            yield return new Point(10, 10);
            yield return new Point(-1, -1);
        }

        private static IEnumerable<Size> IncorrectRectangleSizes()
        {
            yield return new Size(-1, -1);
            yield return new Size(-1, 0);
            yield return new Size(0, -1);
            yield return new Size(0, 0);
        }
    }
}