using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Core.Helpers;

namespace TagsCloudVisualization.Core.Tests
{
    public class CircularCloudLayouterTests
    {
        private Point _center;
        private CircularCloudLayouter _circularCloudLayouter;

        [SetUp]
        public void Setup()
        {
            _center = new Point(100, 100);
            _circularCloudLayouter = new CircularCloudLayouter(_center);
        }

        [TestCase(5, 5, TestName = "Constructor. x, y > 0 and x = y")]
        [TestCase(5, 15, TestName = "Constructor. x, y > 0 and x != y")]
        [TestCase(0, 0, TestName = "Constructor. x, y = 0")]
        public void Constructor_WhenCorrectArgs_ShouldNotThrow(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y));
            act.Should().NotThrow<ArgumentException>();
        }
        [TestCase(-5, -5, TestName = "Constructor. x, y < 0")]
        [TestCase(-5, 5, TestName = "Constructor. x < 0 and y > 0")]
        [TestCase(5, -5, TestName = "Constructor. x > 0 and y < 0")]
        public void Constructor_WhenIncorrectArgs_ShouldThrow(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y));
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(-5, -5, TestName = "PutNextRectangle. x, y < 0")]
        [TestCase(-5, 5, TestName = "PutNextRectangle. x < 0 and y > 0")]
        [TestCase(5, -5, TestName = "PutNextRectangle. x > 0 and y < 0")]
        [TestCase(0, 0, TestName = "PutNextRectangle. x, y = 0")]
        public void PutNextRectangle_WhenIncorrectArgs_ShouldThrow(int width, int height)
        {
            Action act = () => _circularCloudLayouter.PutNextRectangle(new Size(width, height));
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 5, TestName = "PutNextRectangle. x, y > 0 and x = y")]
        [TestCase(5, 10, TestName = "PutNextRectangle. x, y > 0 and x < y")]
        [TestCase(10, 5, TestName = "PutNextRectangle. x, y > 0 and x > y")]
        public void PutNextRectangle_WhenCorrectArgsPutRect_ShouldBeEqualSizes(int width, int height)
        {
            var size = new Size(width, height);
            var rect = _circularCloudLayouter.PutNextRectangle(new Size(width, height));
            rect.Size.Should().Be(size);
        }

        [TestCase(TestName = "PutNextRectangle. Check rect intersection")]
        public void PutNextRectangle_RectstIsNotIntersects_ShouldBeTrue()
        {  
            var rnd = new Random();
            var circularCloud = new CircularCloudLayouter(new Point(600, 600));

            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 50; i++)
            {
                var size = new Size(rnd.Next(10, 20), rnd.Next(15, 30));
                rectangles.Add(circularCloud.PutNextRectangle(size));
            }

            foreach (var rect in rectangles)
            {
                rectangles.Any(p => !p.IntersectsWith(rect) && p != rect).Should().BeTrue();
            }
        }


        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, $"{TestContext.CurrentContext.Test.Name}.bmp");

            var bmpSaver = new BitmapSaver(new Size(1200, 1200));
            bmpSaver.Draw(_circularCloudLayouter.Rectangles);
            bmpSaver.Save(path);

            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}