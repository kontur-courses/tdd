using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    public enum TestSnapshotsMode
    {
        None = 0,
        TestFail = 1,
        TestSuccess = 2,
        All = 3
    }

    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private readonly string currentTestFolder = TestContext.CurrentContext.TestDirectory;
        private const string SnapshotTestTag = "Snapshot";
        private readonly TestSnapshotsMode testSnapshotsMode = TestSnapshotsMode.All;
        private DirectoryInfo testSnapshotsFolder;

        private CircularCloudLayouter layout;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            testSnapshotsFolder = Directory.CreateDirectory(Path.Combine(currentTestFolder, "TestSnapshots"));
        }


        [TestCaseSource(typeof(TestsData), nameof(TestsData.LayoutCenterData))]
        public void CircularCloudLayouter_PassPoint_ShouldEqualLayoutCenter(Point layoutCenter)
        {
            layout = new CircularCloudLayouter(layoutCenter);
            layout.LayoutCenter.Should().Be(layoutCenter);
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.NegativeOrZeroSizeRectangleData))]
        public void PutNextRectangle_NegativeOrZeroSize_ShouldThrowException(Size rectSize)
        {
            Action putBadSizeRect = () => { new CircularCloudLayouter(new Point(0, 0)).PutNextRectangle(rectSize); };
            putBadSizeRect.Should().Throw<ArgumentException>();
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.SomeCorrectCenterPointsAndRectSizesData),
            Category = SnapshotTestTag)]
        public void PutNextRectangle_FirstRectangle_ShouldBeInCenter(Point layoutCenter, Size rectSize)
        {
            layout = new CircularCloudLayouter(layoutCenter);
            var newRect = layout.PutNextRectangle(rectSize);
            newRect.GetRectangleCenter().Should().Be(layout.LayoutCenter);
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.MultipleRandomRectSizesData),
            new object[] {100, 1, 1, 1, 100, 100})]
        public void CircularCloudLayouter_AllRectsShouldHavePassedSizes(Point layoutCenter, IEnumerable<Size> rectSizes)
        {
            layout = new CircularCloudLayouter(layoutCenter);

            var rectCount = 0;
            foreach (var rectSize in rectSizes)
            {
                var newRect = layout.PutNextRectangle(rectSize);
                newRect.Size.Should().Be(new Size(rectSize.Width, rectSize.Height)); //exclude reference equals
                rectCount++;
            }

            layout.Rects.Count().Should().Be(rectCount);
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.MultipleRandomRectSizesData),
            new object[] {100, 1, 1, 1, 100, 100}, Category = SnapshotTestTag)]
        public void CircularCloudLayouter_RectsShouldNotIntersect(Point layoutCenter, IEnumerable<Size> rectSizes)
        {
            layout = new CircularCloudLayouter(layoutCenter);
            foreach (var rectSize in rectSizes)
                layout.PutNextRectangle(rectSize);

            layout.Rects
                .Any(rect1 => layout.Rects
                    .Where(rect2 => !rect1.Equals(rect2))
                    .Any(rect2 => rect2.IntersectsWith(rect1)))
                .Should().BeFalse();
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.TightlyPackedRectanglesData), Category = SnapshotTestTag)]
        public void CircularCloudLayouter_RectsShouldTightlyPacked(Point layoutCenter, IEnumerable<Size> rectSizes)
        {
            layout = new CircularCloudLayouter(layoutCenter);
            var rectsSquare = rectSizes
                .Select(rectSize => layout.PutNextRectangle(rectSize))
                .Aggregate(0d, (current, rect) => current + rect.Width * rect.Height);

            var unionRect = layout.Rects.Aggregate(Rectangle.Union);
            var unionRectSquare = unionRect.Width * unionRect.Height;

            rectsSquare.Should().Be(unionRectSquare);
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.RectanglesFormCircleData), Category = SnapshotTestTag)]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MultipleRandomRectSizesData),
            new object[] {200, 1, 10, 10, 10, 10}, Category = SnapshotTestTag)]
        public void CircularCloudLayouter_RectsShouldFormCircle(Point layoutCenter, IEnumerable<Size> rectSizes)
        {
            layout = new CircularCloudLayouter(layoutCenter);
            var rectsSquare = rectSizes
                .Select(rectSize => layout.PutNextRectangle(rectSize))
                .Aggregate(0d, (current, rect) => current + rect.Width * rect.Height);

            var unionRectangle = layout.Rects.Aggregate(Rectangle.Union);
            var circleRadius =
                unionRectangle.Width > unionRectangle.Height
                    ? unionRectangle.Width / 2
                    : unionRectangle.Height / 2;
            var circleSquare = Math.PI * circleRadius * circleRadius;

            unionRectangle.GetRectangleCenter().GetDistance(layout.LayoutCenter).Should().BeInRange(-0.01, 0.01);
            (rectsSquare / circleSquare).Should().BeInRange(0.85, 1.15);
        }

        [Test]
        public void ToBitmap_EmptyLayout_ShouldThrowException()
        {
            Action createBitmap = () => { new CircularCloudLayouter(new Point(0, 0)).ToBitmap("1.bmp"); };
            createBitmap.Should().Throw<InvalidOperationException>();
        }


        [TearDown]
        public void TearDown()
        {
            var isNotSnapshotTest = !TestContext.CurrentContext.Test.Properties["Category"].Contains(SnapshotTestTag);
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (isNotSnapshotTest ||
                testSnapshotsMode == TestSnapshotsMode.None ||
                testSnapshotsMode == TestSnapshotsMode.TestSuccess && testResult != ResultState.Success ||
                testSnapshotsMode == TestSnapshotsMode.TestFail && !(testResult == ResultState.Failure ||
                                                                     testResult == ResultState.Error))
                return;

            var path = Path.Combine(testSnapshotsFolder.FullName,
                $"{TestContext.CurrentContext.Test.ID}-{TestContext.CurrentContext.Test.MethodName}-{testResult}.bmp");
            layout.ToBitmap(path);
            Console.WriteLine(@$"Tag cloud visualization saved to file ""{path}""");
        }
    }

    public class TestsData
    {
        public static IEnumerable<TestCaseData> LayoutCenterData
        {
            get
            {
                yield return new TestCaseData(new Point(10, 10))
                    {TestName = "CircularCloudLayouter_PassPointWithPositiveCoords_ShouldEqualLayoutCenter"};
                yield return new TestCaseData(new Point(0, 0))
                    {TestName = "CircularCloudLayouter_PassPointWithZeroCoords_ShouldEqualLayoutCenter"};
                yield return new TestCaseData(new Point(-10, -10))
                    {TestName = "CircularCloudLayouter_PassPointWithNegativeCoords_ShouldEqualLayoutCenter"};
            }
        }

        public static IEnumerable<TestCaseData> NegativeOrZeroSizeRectangleData
        {
            get
            {
                yield return new TestCaseData(new Size(-15, -15));
                yield return new TestCaseData(new Size(15, 0));
                yield return new TestCaseData(new Size(0, 15));
                yield return new TestCaseData(new Size(0, 0));
            }
        }

        public static IEnumerable<TestCaseData> SomeCorrectCenterPointsAndRectSizesData
        {
            get
            {
                yield return new TestCaseData(new Point(960, 540), new Size(15, 15));
                yield return new TestCaseData(new Point(0, 0), new Size(20, 15));
                yield return new TestCaseData(new Point(-10, -15), new Size(15, 35));
            }
        }

        public static IEnumerable<TestCaseData> MultipleRandomRectSizesData(int count, int rndSeed, int minSizeX,
            int minSizeY, int maxSizeX, int maxSizeY)
        {
            var center = new Point(0, 0);
            var minSize = new Size(minSizeX, minSizeY);
            var maxSize = new Size(maxSizeX, maxSizeY);

            var rnd = new Random(rndSeed);
            var rectSizes = new List<Size>();
            for (int i = 0; i < count; i++)
                rectSizes.Add(new Size(
                    rnd.Next(minSize.Width, maxSize.Width),
                    rnd.Next(minSize.Width, maxSize.Width)));

            yield return new TestCaseData(center, rectSizes);
        }

        public static IEnumerable<TestCaseData> TightlyPackedRectanglesData
        {
            get
            {
                yield return new TestCaseData(new Point(0, 0),
                    new[]
                    {
                        new Size(10, 10),
                        new Size(10, 30),
                        new Size(10, 30),
                        new Size(30, 10),
                        new Size(30, 10),
                        new Size(10, 10),
                        new Size(10, 10)
                    });
            }
        }

        public static IEnumerable<TestCaseData> RectanglesFormCircleData()
        {
            var center = new Point(0, 0);
            var maxSize = 10;

            var rectSizes = new List<Size>();
            for (int i = 1; i < maxSize; i++)
            for (int y = 0; y < i + 4 * (i - 1); y++)
                rectSizes.Add(new Size(maxSize - i, maxSize - i));

            yield return new TestCaseData(center, rectSizes);
        }
    }
}