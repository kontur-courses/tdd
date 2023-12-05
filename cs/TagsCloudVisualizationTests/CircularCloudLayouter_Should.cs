using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using System.Collections;
using static System.Formats.Asn1.AsnWriter;

namespace TagsCloudVisualizationTests
{

    public class CircularCloudLayouter_Should
    {
        private const string RelativePathToFailDirectory = @"..\..\..\Fails";
        private CircularCloudLayouter layouter;
        private Point center = new Point(500, 500);

        [SetUp]
        public void Setup()
        {
            layouter = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void Tear_Down()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                SaveIncorrectResultsToJpg();
        }

        public static IEnumerable TestCasesForWrongCenterPoint
        {
            get
            {
                yield return new TestCaseData(new Point(-1, 0)).SetName("Throw argument exception when the X coordinate of the center is negative");
                yield return new TestCaseData(new Point(2, -1)).SetName("Throw argument exception when the Y coordinate of the center is negative");
                yield return new TestCaseData(new Point(-6, -1)).SetName("Throw argument exception when all coordinates of the center is negative");
            }
        }

        public static IEnumerable TestCasesForCorrectCenterPoint
        {
            get
            {
                yield return new TestCaseData(new Point(0, 0)).SetName("Dont throw argument exception when all the coordinates of the center are zero");
                yield return new TestCaseData(new Point(2, 1)).SetName("Dont throw argument exception when all the coordinates of the center are positive");
                yield return new TestCaseData(new Point(0, 4)).SetName("Dont throw argument exception when the X coordinate of the center is zero");
                yield return new TestCaseData(new Point(6, 0)).SetName("Dont throw argument exception when the Y coordinate of the center is zero");
            }
        }

        public static IEnumerable TestCasesForWrongRectangleSize
        {
            get
            {
                yield return new TestCaseData(new Size(0, 0)).SetName("Throw argument exception when the rectangle dimensions are zero");
                yield return new TestCaseData(new Size(-1, 5)).SetName("Throw argument exception when the width of the rectangle is negative");
                yield return new TestCaseData(new Size(6, -6)).SetName("Throw argument exception when the height of the rectangle is negative");
                yield return new TestCaseData(new Size(6, -6)).SetName("Throw argument exception when the rectangle dimensions are negative");
            }
        }

        public static IEnumerable TestCasesForCorrectRectangleSize
        {
            get
            {
                yield return new TestCaseData(new Size(1, 1)).SetName("Dont throw argument exception when the rectangle dimensions are positive");
                yield return new TestCaseData(new Size(2, 5)).SetName("Dont throw argument exception when the rectangle dimensions are positive");
                yield return new TestCaseData(new Size(34, 100)).SetName("Dont throw argument exception when the rectangle dimensions are positive");
            }
        }

        [Test]
        public void ReturnEmptyList_WhenCreated()
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(1, 1));
            layouter.Rectangles.Should().BeEmpty();
        }

        [Test, TestCaseSource(nameof(TestCasesForWrongCenterPoint))]
        public void ThrowArgumentEsception_WhenWrongCenterPoint(Point center)
        {

            Action action = () => new CircularCloudLayouter(center);
            action.Should().Throw<ArgumentException>();
        }

        [Test, TestCaseSource(nameof(TestCasesForCorrectCenterPoint))]
        public void DontThrowArgumentEsception_WhenWrongCenterPoint(Point center)
        {

            Action action = () => new CircularCloudLayouter(center);

            action.Should().NotThrow();
        }

        [Test, TestCaseSource(nameof(TestCasesForWrongRectangleSize))]
        public void ThrowArgumentEsception_WhenAddWrongRectangleSize(Size rectSize)
        {

            Action action = () => new CircularCloudLayouter(new Point(0,0)).PutNextRectangle(rectSize);
            action.Should().Throw<ArgumentException>();
        }

        [Test, TestCaseSource(nameof(TestCasesForCorrectRectangleSize))]
        public void DontThrowArgumentEsception_WhenAddCorrectRectangleSize(Size rectSize)
        {

            Action action = () => new CircularCloudLayouter(new Point(0, 0)).PutNextRectangle(rectSize);
            action.Should().NotThrow();
        }

        [Test]
        public void ReturnOneRectangle_WhenAddOne()
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(100, 100));

            layouter.PutNextRectangle(new Size(50, 50));

            layouter.Rectangles.Count().Should().Be(1);
        }

        [Test]
        public void ReturnTwoRectangle_WhenAddTwo()
        {
            CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(100, 100));

            layouter.PutNextRectangle(new Size(50, 50));
            layouter.PutNextRectangle(new Size(50, 50));

            layouter.Rectangles.Count().Should().Be(2);
        }

        [TestCase(20, 100, 30, 40, 50)]
        public void AlwaysFail(int widthMin, int widthMax, int heightMin, int heightMax, int count)
        {
            var rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(widthMin, widthMax), rnd.Next(heightMin, heightMax));
                layouter.PutNextRectangle(size);
            }

            Assert.Fail();
        }

        [TestCase(10, 60, 20, 80, 50)]
        public void AddMultipleRectangles_WithRandomSize(int widthMin, int widthMax, int heightMin, int heightMax, int count)
        {
            var rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(widthMin, widthMax), rnd.Next(heightMin, heightMax));
                layouter.PutNextRectangle(size);
            }

            layouter.Rectangles.Count().Should().Be(count);
        }

        [TestCase(35, 35, 100, TestName = "Add multiple rectangles with equal width and height")]
        [TestCase(20, 20, 100, TestName = "Add multiple rectangles with unequal width and height")]
        public void AddMultipleRectangles_WithConstantSize(int width, int height, int count)
        {
            var size = new Size(width, height);

            for (int i = 0; i < count; i++)
                layouter.PutNextRectangle(size);

            layouter.Rectangles.Count().Should().Be(count);
        }

        [TestCase(40, 40, 100, TestName = "No intersections when add multiple rectangles with equal width and height")]
        [TestCase(30, 71, 100, TestName = "No intersections when add add multiple rectangles with unequal width and height")]
        public void NoIntersections_WhenAdding_Multiple_Rectangles_WithConstantSize(int width, int height, int count)
        {
            var size = new Size(width, height);
            for (int i = 0; i < count; i++)
                layouter.PutNextRectangle(size);

            isIntersections().Should().BeFalse();
        }

        [TestCase(15, 75, 10, 40, 100)]
        public void NoIntersections_WhenAdding_Multiple_Rectangles_WithRandomSize(int widthMin, int widthMax, int heightMin, int heightMax, int count)
        {
            var rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                var size = new Size(rnd.Next(widthMin, widthMax), rnd.Next(heightMin, heightMax));
                layouter.PutNextRectangle(size);
            }

            isIntersections().Should().BeFalse();
        }

        private void SaveIncorrectResultsToJpg()
        {
            var rectangles = layouter.Rectangles;
            if (rectangles.Count() == 0)
                return;

            var pathToFile = @$"{RelativePathToFailDirectory}\{TestContext.CurrentContext.Test.FullName}.png";
            var absolutePath = Path.GetFullPath(pathToFile);
            var visualizator = new TagsCloudVisualizator(layouter, new Size(1600, 1600));

            visualizator.drawCloud();
            visualizator.saveImage(pathToFile);
            
            Console.WriteLine($"Tag cloud visualization saved to file {absolutePath}");
        }

        public bool isIntersections()
        {
            var rectList = layouter.Rectangles;

            for (int i = 0; i < rectList.Count; i++)
                for(int j = i + 1; j < rectList.Count; j++)
                    if (rectList[i].IntersectsWith(rectList[j]))
                        return true;

            return false;
        }

    }
}
