using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationUnitTest
{
    public class TagsCloudVisualizationTest
    {
        private CircularCloudLayouter circularCloudLayouter;
        private PainterOfRectangles painterOfRectangles;
        private ArchimedesSpiral archimedesSpiral;
        private List<Rectangle> rectangles;
        private Point centrPoint;
        private const string PathFolderFailedTest = "FailedTest";

        [SetUp]
        public void InitCircularCloudLayouter()
        {
            centrPoint = new Point(500, 500);
            archimedesSpiral = new ArchimedesSpiral(centrPoint);
            circularCloudLayouter = new CircularCloudLayouter(centrPoint, archimedesSpiral);
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void SaveImageWithFailCircularCloudLayouter()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed)
            {
                Directory.CreateDirectory(PathFolderFailedTest);
                var nameFile = PathFolderFailedTest + "//" + TestContext.CurrentContext.Test.FullName + "Failed.png";
                var saver = new SaverImage(nameFile);
                painterOfRectangles = new PainterOfRectangles(new Size(1000, 1000));
                painterOfRectangles.CreateImage(rectangles, saver);
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
                rectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(10, 10)));
            }

            rectangles.Should().OnlyHaveUniqueItems();
        }

        [TestCase(10, TestName = "10 Rectangles")]
        [TestCase(1000, TestName = "1000 Rectangles")]
        [TestCase(2000, TestName = "2000 Rectangles")]
        public void DensityCloud_ShouldBeBig(int countRectangle)
        {
            var generator = new GeneratorOfRectangles();

            for (var i = 0; i < countRectangle; i++)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(generator.GetSize(10, 10)));
            }

            var square = Math.Pow(Radius(), 2) * Math.PI;

            var densityCloud = SquareRectangles() / square * 100;

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
                rectangles.Add(circularCloudLayouter.PutNextRectangle(generator.GetSize(10, 20)));
            }

            var square = Math.Pow(Radius(), 2) * Math.PI;

            var densityCloud = SquareRectangles() / square * 100;

            densityCloud.Should().BeGreaterThan(70);
        }

        private int Radius()
        {
            if (rectangles == null)
                throw new Exception("rectangles is null");

            var xMax = rectangles.Max(rectangle => Math.Abs(rectangle.X));
            var yMax = rectangles.Max(rectangle => Math.Abs(rectangle.Y));

            var distanceToXMax = Math.Abs(xMax) - Math.Abs(centrPoint.X);
            var distanceToYMax = Math.Abs(yMax) - Math.Abs(centrPoint.Y);

            return distanceToXMax > distanceToYMax ? distanceToXMax : distanceToYMax;
        }

        private double SquareRectangles()
        {
            if (rectangles == null)
                throw new Exception("rectangles is null");

            return rectangles.Sum(rectangle => rectangle.Square());
        }
    }
}