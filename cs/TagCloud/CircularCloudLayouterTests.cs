using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud
{
    public class CircularCloudLayouterTests
    {
        private readonly string failedTestsPictureFolder = "FailedTestsPicture";
        private CircularCloudLayouter testedTagCloud;

        [SetUp]
        public void PrepareCircularCloudLayouter()
        {
            testedTagCloud = new CircularCloudLayouter();
        }

        [TearDown]
        public void SavePicture_WhenTestFailed()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status != TestStatus.Failed)
                return;

            Directory.CreateDirectory(failedTestsPictureFolder);
            var fileName = $"{context.Test.MethodName}{new Random().Next()}";
            var filePath = Path.Combine(failedTestsPictureFolder, fileName);

            File.WriteAllText(filePath + ".txt", $"The test {context.Test.FullName} failed with an error: {context.Result.Message}" + 
                                                 Environment.NewLine + "StackTrace:" + context.Result.StackTrace);
            TagCloudVisualization.SaveAsBitmap(testedTagCloud, filePath + ".bmp");

            TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }



        [TestCase(0, 0)]
        [TestCase(5, 10)]
        public void Ctor_SetCenterPoint(int x, int y)
        {
            var planningCenter = new Point(x, y);

            testedTagCloud = new CircularCloudLayouter(planningCenter);

            testedTagCloud.Center.Should().BeEquivalentTo(planningCenter);
            testedTagCloud.GetWidth().Should().Be(0);
            testedTagCloud.GetHeight().Should().Be(0);
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(10, 0)]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            Action act = () => testedTagCloud.PutNextRectangle(new Size(width, height));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(100)]
        [TestCase(50)]
        public void GetWidth_EqualsToTheRectangleWidth(int width)
        {
            testedTagCloud.PutNextRectangle(new Size(width, 3));

            testedTagCloud.GetWidth().Should().Be(width);
        }

        [TestCase(100)]
        [TestCase(50)]
        public void GetHeight_EqualsToTheRectangleHeight(int height)
        {
            testedTagCloud.PutNextRectangle(new Size(3, height));

            testedTagCloud.GetHeight().Should().Be(height);
        }

        [TestCase(0, 0, 35, 75)]
        [TestCase(3, 3, 5, 5)]
        public void PutNextRectangle_FirstRectangleMustBeInCenter(int centerX, int centerY, int reactWidth, int reactHeight)
        {
            testedTagCloud = new CircularCloudLayouter(new Point(centerX, centerY));

            var rectangle = testedTagCloud.PutNextRectangle(new Size(reactWidth, reactHeight));
            var planningReactLocation = new Point(centerX - reactWidth / 2, centerY - reactHeight / 2);

            rectangle.Location.Should().BeEquivalentTo(planningReactLocation);
        }

        [TestCase(0, 0, 350, 750)]
        [TestCase(3, 3, 500, 500)]
        public void PutNextRectangle_ReturnedNotIntersectedRectangle(int centerX, int centerY, int firstRectWidth, int firstRectHeight)
        {
            testedTagCloud = new CircularCloudLayouter(new Point(centerX, centerY));

            do
            {
                var newRect = testedTagCloud.PutNextRectangle(new Size(firstRectWidth, firstRectHeight));
                testedTagCloud.Reactangles.Where(rect => rect != newRect).All(rect => !rect.IntersectsWith(newRect))
                    .Should().BeTrue();

                firstRectHeight /= 2;
                firstRectWidth /= 2;
            } while (firstRectHeight > 1 && firstRectWidth > 1);
        }
    }
}