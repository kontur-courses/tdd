using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationUnitTest
{
    public class TagsCloudVisualizationTest
    {
        private CircularCloudLayouter circularCloudLayouter;
        private PainterOfRectangles painterOfRectangles;
        private const string PathFolderFailedTest = "FailedTest";

        [SetUp]
        public void InitCircularCloudLayouter()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
            painterOfRectangles = new PainterOfRectangles(new Size(1000, 1000));
        }

        [TearDown]
        public void SaveImageWithFailCircularCloudLayouter()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed)
            {
                painterOfRectangles.CreateImage(circularCloudLayouter.Rectangles,
                    PathFolderFailedTest + "//" + TestContext.CurrentContext.Test.FullName +
                    "Failed.png");
            }
        }

        [TestCase(-10, -10, TestName = "Negative size")]
        [TestCase(0, 0, TestName = "Size is Empty")]
        [TestCase(10, 0, TestName = "Weight = zero")]
        public void PutNextRectangle_ShouldBeThrowWhen(int height, int width)
        {
            Action act = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));

            act.Should().Throw<Exception>();
        }

        [Test]
        public void PutNextRectangle_RectanglesShouldBeHaveUniqueCoordinates()
        {
            for (var i = 0; i < 1000; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            }

            circularCloudLayouter.Rectangles.Should().OnlyHaveUniqueItems();
        }

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "2000 Rectangles")]
        public void PutNextRectangle_ShouldBeOptimized(int countRectangle)
        {
            Action act = () =>
            {
                for (int i = 0; i < countRectangle; i++)
                {
                    circularCloudLayouter.PutNextRectangle(new Size(10, 10));
                }
            };

            act.ExecutionTime().Should().BeLessThanOrEqualTo(1.Seconds());
        }

        [Test]
        public void SquareRectangles_ShouldBeCorrect()
        {
            for (var i = 0; i < 2000; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            }

            circularCloudLayouter.SquareRectangles.Should().Be(200000);
        }

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "2000 Rectangles")]
        public void DensityCloud_ShouldBeBig(int countRectangle)
        {
            var generator = new GeneratorOfRectangles();

            for (var i = 0; i < countRectangle; i++)
            {
                circularCloudLayouter.PutNextRectangle(generator.GetRectangleSize(10, 10));
            }

            var square = Math.Pow(circularCloudLayouter.Radius(), 2) * Math.PI;

            var densityCloud = circularCloudLayouter.SquareRectangles / square * 100;

            densityCloud.Should().BeGreaterThan(70);
        }

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "2000 Rectangles")]
        public void DensityCloud_Random_Size_ShouldBeBig(int countRectangle)
        {
            var generator = new GeneratorOfRectangles();

            for (var i = 0; i < countRectangle; i++)
            {
                circularCloudLayouter.PutNextRectangle(generator.GetRectangleSize(10, 20));
            }

            var square = Math.Pow(circularCloudLayouter.Radius(), 2) * Math.PI;

            var densityCloud = circularCloudLayouter.SquareRectangles / square * 100;

            densityCloud.Should().BeGreaterThan(70);
        }
    }
}