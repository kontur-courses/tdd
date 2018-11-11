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
                var directory = TestContext.CurrentContext.TestDirectory;
                var filename = TestContext.CurrentContext.Test.Name;
                var path = $"{directory}\\{filename}.png";
                var bitmap = RectanglesVisualizer.Visualize(resRectangles);
                bitmap.Save(path);
                TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [Test]
        public void ReturnRectangleLocatedInTheCenter_OnTheFirstCallOfPutNextRectangleMethod()
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
        
        [Test]
        public void ReturnNonIntersectingRectangles()
        {
            const int rectanglesCount = 100;
            var layouter = new CircularCloudLayouter(new Point(1, -1));
            var rectangleSize = new Size(10, 10);

            for (int i = 0; i < rectanglesCount; i++)
                resRectangles.Add(layouter.PutNextRectangle(rectangleSize));

            for (int i = 0; i < rectanglesCount; i++)
                for (int j = i + 1; j < rectanglesCount; j++)
                    resRectangles[i].IntersectsWith(resRectangles[j]).Should().BeFalse();
        }
        
        [Test]
        public void ReturnDifferentRectangles()
        {
            const int rectanglesCount = 100;
            var layouter = new CircularCloudLayouter(new Point(1, 1));
            var rectangleSize = new Size(10, 10);

            for (int i = 0; i < rectanglesCount; i++)
                resRectangles.Add(layouter.PutNextRectangle(rectangleSize));

            for (int i = 0; i < rectanglesCount; i++)
                for (int j = i + 1; j < rectanglesCount; j++)
                    resRectangles[i].Should().NotBe(resRectangles[j]);
        }

        [Test]
        public void PutNewRectanglesInsideTheCircleWithRadius24_AfterPutting100Rectangles3x4()
        {
            const int rectanglesCount = 100;
            const int radius = 24;
            var center = new Point(-2, 2);
            var layouter = new CircularCloudLayouter(new Point());
            var rectangleSize = new Size(3, 4);

            for (int i = 0; i < rectanglesCount; i++)
                resRectangles.Add(layouter.PutNextRectangle(rectangleSize));

            foreach (var rectangle in resRectangles)
                rectangle.Location.GetDistanceTo(center).Should().BeLessThan(radius);
        }

        [Test, Timeout(500)]
        public void Put1000RectanglesIn500Milliseconds()
        {
            const int rectanglesCount = 1000;
            var layouter = new CircularCloudLayouter(new Point(-1, 1));
            var rectangleSize = new Size(10, 10);

            for (int i = 0; i < rectanglesCount; i++)
                layouter.PutNextRectangle(rectangleSize);
        }
    }
}