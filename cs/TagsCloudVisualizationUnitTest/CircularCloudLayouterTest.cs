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
        private const string pathFolderFailedTest = "FailedTest";
        [OneTimeSetUp]
        public void CreateFolderForFailedTests()
        {
            if (!Directory.Exists(pathFolderFailedTest))
            {
                Directory.CreateDirectory(pathFolderFailedTest);
            }
        }

        [SetUp]
        public void InitCircularCloudLayouter()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
        }

        [TearDown]
        public void SaveImageWithFailCircularCloudLayouter()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed)
            {
                var painterOfRectangles = new PainterOfRectangles(new Size(1000, 1000));

                painterOfRectangles.CreateImage(circularCloudLayouter.Rectangles,
                    pathFolderFailedTest + "//" + TestContext.CurrentContext.Test.Name + "CircularCloudLayouter.png");
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
            for (int i = 0; i < 1000; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            }

            circularCloudLayouter.Rectangles.Should().OnlyHaveUniqueItems();
        }

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "3000 Rectangles")]
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

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "2000 Rectangles")]
        public void DensityCloud_ShouldBeBig(int countRectangle)
        {
            for (var i = 0; i < countRectangle; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            }

            double square = Math.Pow(circularCloudLayouter.Radius(), 2) * Math.PI;

            (circularCloudLayouter.SquareRectangles / square * 100).Should().BeGreaterThan(70);
        }

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "2000 Rectangles")]
        public void DensityCloud_Random_Size_ShouldBeBig(int countRectangle)
        {
            Random rnd = new Random();

            for (var i = 0; i < countRectangle; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(rnd.Next(10, 20), rnd.Next(10, 20)));
            }

            double square = Math.Pow(circularCloudLayouter.Radius(), 2) * Math.PI;


            (circularCloudLayouter.SquareRectangles / square * 100).Should().BeGreaterThan(65);
        }
    }
}