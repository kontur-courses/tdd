using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        private List<Rectangle> resRectangles;

        [SetUp]
        public void SetUp()
        {
            resRectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
            {
                var testhehe = TestContext.CurrentContext.WorkDirectory;
                var directory = TestContext.CurrentContext.TestDirectory;
                var filename = TestContext.CurrentContext.Test.Name;
                var path = $"{directory}\\{filename}.png";
                var bitmap = RectanglesVisualizer.Visualize(resRectangles);
                bitmap.Save(path);
                TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [TestCase(0, 0, TestName = "On point with zero coordinates")]
        [TestCase(-1, -1, TestName = "On point with negative coordinates")]
        [TestCase(1, 1, TestName = "On point with positive coordinates")]
        public void InitiateWithoutException(int x, int y)
        {
            Action layouterInit = () => new CircularCloudLayouter(new Point(x, y));
            layouterInit.Should().NotThrow("Circular Cloud Layouter should work with any center points");
        }

        [Test]
        public void PutFirstRectangleAtTheCenter()
        {
            var centerPoint = new Point(0, 0);
            var layouter = new CircularCloudLayouter(centerPoint);

            var firstRectangle = layouter.PutNextRectangle(new Size(10, 10));
            
            firstRectangle.Location.Should().Be(centerPoint);
        }

        [TestCase(10, -10, TestName = "With negative height")]
        [TestCase(10, 0, TestName = "With zero height")]
        [TestCase(-10, 10, TestName = "With negative width")]
        [TestCase(0, 10, TestName = "With zero width")]
        public void ThrowArgumentException_OnPuttingInvalidRectangle(int width, int height)
        {
            var layouter = new CircularCloudLayouter(new Point());
            Action putIncorrectSizeAction = () => layouter.PutNextRectangle(new Size(width, height));
            putIncorrectSizeAction.Should().Throw<ArgumentException>();
        }

        [TestCase(10, TestName = "After putting 10 rectangles")]
        [TestCase(100, TestName = "After putting 100 rectangles")]
        [TestCase(1000, TestName = "After putting 1000 rectangles")]
        public void ReturnNonIntersectingRectangles(int rectanglesCount)
        {
            var layouter = new CircularCloudLayouter(new Point(1, -1));
            var rectangleSize = new Size(10, 10);

            for (int i = 0; i < rectanglesCount; i++)
                resRectangles.Add(layouter.PutNextRectangle(rectangleSize));

            for (int i = 0; i < rectanglesCount; i++)
                for (int j = i + 1; j < rectanglesCount; j++)
                    resRectangles[i].IntersectsWith(resRectangles[j]).Should().BeFalse();
        }

        [TestCase(10, TestName = "After putting 10 rectangles")]
        [TestCase(100, TestName = "After putting 100 rectangles")]
        [TestCase(1000, TestName = "After putting 1000 rectangles")]
        public void ReturnDifferentRectangles(int rectanglesCount)
        {
            var layouter = new CircularCloudLayouter(new Point(1, 1));
            var rectangleSize = new Size(10, 10);

            for (int i = 0; i < rectanglesCount; i++)
                resRectangles.Add(layouter.PutNextRectangle(rectangleSize));

            for (int i = 0; i < rectanglesCount; i++)
                for (int j = i + 1; j < rectanglesCount; j++)
                    resRectangles[i].Should().NotBe(resRectangles[j]);
        }

        [TestCase(10, 8, TestName = "With radius 8 after putting 10 rectangles 3x4")]
        [TestCase(100, 24, TestName = "With radius 80 after putting 100 rectangles 3x4")]
        [TestCase(1000, 240, TestName = "With radius 800 after putting 1000 rectangles 3x4")]
        public void PutNewRectanglesInsideTheCircle(int rectanglesCount, int radius)
        {
            var center = new Point(-2, 2);
            var layouter = new CircularCloudLayouter(new Point());
            var rectangleSize = new Size(3, 4);

            for (int i = 0; i < rectanglesCount; i++)
                resRectangles.Add(layouter.PutNextRectangle(rectangleSize));

            foreach (var rectangle in resRectangles)
                rectangle.Location.GetDistanceTo(center).Should().BeLessThan(radius);
        }
    }
}