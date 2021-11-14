using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterTest
    {
        private Point center;
        private CircularCloudLayouter sut;
        private Rectangle[] rectangles;

        [SetUp]
        public void Setup()
        {
            center = new Point(0, 0);
            sut = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure) 
                return;
            var testName = TestContext.CurrentContext.Test.Name + ".bmp";
            var drawer = new RectanglesDrawer();
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "FailureTestImages");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var pathToImage = Path.Combine(path, testName);
            var image = drawer.Draw(rectangles, Color.Red);
            image.Save(pathToImage);
            
            Console.WriteLine($"Tag cloud visualization saved to file {pathToImage}");
        }
        

        [TestCase(0, 1, TestName = "0 width")]
        [TestCase(1, 0, TestName = "0 height")]
        [TestCase(-1, 1, TestName = "negative width")]
        [TestCase(1, -1, TestName = "negative height")]
        public void PutNextRectangle_ShouldThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            FluentActions.Invoking(
                    () => sut.PutNextRectangle(size))
                .Should().Throw<ArgumentException>();
        }


        [Test]
        public void PutNextRectangle_ShouldReturnRectangleInCenter_WhenCalledFirstTime()
        {
            var rectangle = sut.PutNextRectangle(new Size(10, 10));

            rectangle.GetCenter().Should().Be(center);
        }

        [TestCase(100)]
        [TestCase(500)]
        public void PutNextRectangle_ShouldReturnNotIntersectRectangles_WhenRandomSize(int count)
        {
            var rnd = new Random();
            var sizeFactory = new Func<Size>(() =>
                new Size(rnd.Next(20, 50), rnd.Next(10, 20)));
            rectangles = GetRectangles(count, sizeFactory);
            
            rectangles
                .ContainsIntersectingRectangles()
                .Should().BeFalse();
        }
        
        [TestCase(100)]
        [TestCase(500)]
        public void PutNextRectangle_ShouldReturnNotIntersectRectangles_WhenSameSize(int count)
        {
            rectangles = GetRectangles(count, () => new Size(20, 40));
            
            rectangles
                .ContainsIntersectingRectangles()
                .Should().BeFalse();
        }
        

        [TestCase(10, 200, 300, 100, TestName = "small count of big rectangles")]
        [TestCase(100, 20, 30, 10, TestName = "big count of small rectangles")]
        public void PutNextRectangle_ShouldReturnRectanglesThatFormACircle(int count, int maxWidth, int maxHeight, int precision)
        {
            var rnd = new Random();
            var width = rnd.Next(1, maxWidth);
            var height = rnd.Next(1, maxWidth);
            var layoutArea = count * width * height;
            var expectedRadius = Math.Sqrt(layoutArea / Math.PI);
            var sizeFactory = new Func<Size>(() => new Size(width, height));

            rectangles = GetRectangles(count, sizeFactory);
            var distanceX = (double)rectangles.Select(rectangle => rectangle.GetCenter().X).Max() - center.X;
            var distanceY = (double)rectangles.Select(rectangle => rectangle.GetCenter().Y).Max() - center.Y;

            Math.Abs(distanceX - expectedRadius).Should().BeLessThan(precision);
            Math.Abs(distanceY - expectedRadius).Should().BeLessThan(precision);
        }
        
        private Rectangle[] GetRectangles(int count, Func<Size> sizeFactory)
        {
            return Enumerable.Range(0, count).Select(x =>
                sut.PutNextRectangle(sizeFactory.Invoke())).ToArray();
        }
    }
}