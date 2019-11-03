using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Test_CircularCloudLayouter
    {
        [TearDown]
        public void SaveImage()
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.FailCount == 0)
                return;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestBug");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, $"{testContext.Test.Name}.png");
            var cloudLayouter = (CircularCloudLayouter) testContext.Test.Properties.Get("cloudLayouter");
            cloudLayouter.Visualization(new Bitmap(1000, 1000))
                .Save(path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [Test]
        public void TestSaveOfImageOnTearDown_ShouldFallenAnyTime()
        {
            var cloudLayouter = new CircularCloudLayouter();
            var testContext = TestContext.CurrentContext;
            for (var i = 0; i < 100; i++)
                cloudLayouter.PutNextRectangle(new Size(testContext.Random.Next(1, 100),
                    testContext.Random.Next(1, 100)));

            true.Should().BeFalse();
        }

        [TestCase(0, 0)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -1)]
        public void PutNextRectangle_OnNegativeSize_ShouldThrowArgExcept(int width, int height)
        {
            var cloudLayouter = new CircularCloudLayouter();
            Action action = () => cloudLayouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnSomeRectangleWithRightSize()
        {
            var cloudLayouter = new CircularCloudLayouter();
            cloudLayouter.PutNextRectangle(new Size(7, 5)).Size.Should().Be(new Size(7, 5));
        }

        [Test]
        public void PutNextRectangle_AnyTwoRectanglesNotHaveIntersect()
        {
            var cloudLayouter = new CircularCloudLayouter();
            var random = TestContext.CurrentContext.Random;
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 30; i++)
                rectangles.Add(cloudLayouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100))));

            for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [Test]
        public void CircularCloudLayout_AnyTwoRectanglesNotHaveIntersectAfterCentreings()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(450, 450));
            var random = TestContext.CurrentContext.Random;
            for (var i = 0; i < 30; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100)));

            var rectanglesHash = cloudLayouter.Centreings();
            var rectangles = new List<Rectangle>(rectanglesHash);

            for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [TestCase("smallPicture.png", 20, 400, 600)]
        [TestCase("middlePicture.png", 60, 800, 400)]
        [TestCase("largePicture.png", 300, 1500, 200)]
        public void VisualizationSomeImage(string name, int sizeOfRectangle, int size, int count)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(size, size));
            var random = new Random();
            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(sizeOfRectangle / 3, sizeOfRectangle),
                    random.Next(sizeOfRectangle / 3, sizeOfRectangle)));

            var bitMap = new Bitmap(size * 2, size * 2);
            bitMap = cloudLayouter.Visualization(bitMap);
            bitMap.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name));
            
            bitMap = new Bitmap(size * 2, size * 2);
            bitMap = cloudLayouter.Visualization(bitMap, cloudLayouter.Centreings());
            bitMap.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Centering{name}"));
        }
    }
}