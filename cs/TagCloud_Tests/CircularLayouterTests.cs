using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud.Layouting;
using TagCloud_TestDataGenerator;
using TagCloud;
using TagCloud.Saving;
using TagCloud.Visualization;

namespace TagCloudVisualization_Tests
{
    public class CircularLayouterTests
    {
        private CircularLayouter layouter;

        [SetUp]
        public void InitLayouter()
        {
            layouter = new CircularLayouter(Point.Empty);
        }

        [Test]
        public void Constructor_ShouldAllowNegativeXY_ForCloudCenter()
        {
            var center = new Point(-1, -1);

            layouter = new CircularLayouter(center);

            layouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void Constructor_ShouldSetCenter_WhenCreate()
        {
            var centerPoint = new Point(10, 10);

            layouter = new CircularLayouter(centerPoint);

            layouter.Center.Should().BeEquivalentTo(centerPoint);
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

        [TearDown]
        public void SaveBitmap_WhenLayoutIsIncorrect()
        {
            var context = TestContext.CurrentContext;

            if (!IsPassedLayoutTest(context))
               return;

            var tagCloud = new TagCloud.TagCloud(layouter,
                new BitmapSaver(), new Visualizer(new Drawer()));

            var path = tagCloud.SaveBitmapTo(true, true, false);
            var msg = string.Join("", "Tag cloud visualization saved to file\n", path);

            Console.WriteLine(msg);
        }

        private bool IsPassedLayoutTest(TestContext context)
        {
            var layoutTestNames = GetLayoutTestNames();

            if (!layoutTestNames.Contains(context.Test.MethodName))
                return false;

            if (context.Result.Outcome.Status == TestStatus.Passed)
                return false;

            return true;
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
            var rectangles = new List<Rectangle>();
            foreach (var rectSize in RectangleSizeGenerator.GetNextNRandomSize(n))
                rectangles.Add(layouter.PutNextRectangle(rectSize));

            return rectangles;
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

        private static IEnumerable<Size> IncorrectRectangleSizes()
        {
            yield return new Size(-1, -1);
            yield return new Size(-1, 0);
            yield return new Size(0, -1);
            yield return new Size(0, 0);
        }
    }
}