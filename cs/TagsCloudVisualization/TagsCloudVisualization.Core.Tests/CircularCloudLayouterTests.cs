using System.Drawing;
using FluentAssertions;

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
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(x, y)));
        }
        [TestCase(-5, -5, TestName = "Constructor. x, y < 0")]
        [TestCase(-5, 5, TestName = "Constructor. x < 0 and y > 0")]
        [TestCase(5, -5, TestName = "Constructor. x > 0 and y < 0")]
        public void Constructor_WhenIncorrectArgs_ShouldThrow(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(x, y)));
        }

        [TestCase(-5, -5, TestName = "PutNextRectangle. x, y < 0")]
        [TestCase(-5, 5, TestName = "PutNextRectangle. x < 0 and y > 0")]
        [TestCase(5, -5, TestName = "PutNextRectangle. x > 0 and y < 0")]
        [TestCase(0, 0, TestName = "PutNextRectangle. x, y = 0")]
        public void PutNextRectangle_WhenIncorrectArgs_ShouldThrow(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => _circularCloudLayouter.PutNextRectangle(new Size(width, height)));
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
        public void PutNextRectangle_CheckRectIntersections_ShouldBeFalse()
        {  
            var rnd = new Random();
            var circularCloud = new CircularCloudLayouter(new Point(600, 600));

            for (var i = 0; i < 50; i++)
            {
                var size = new Size(rnd.Next(30, 60), rnd.Next(30, 60));
                circularCloud.PutNextRectangle(size);
            }

            foreach (var rect in circularCloud.Rectangles)
            {
                foreach (var rect2 in circularCloud.Rectangles)
                {
                    Assert.False(rect.IntersectsWith(rect2));
                }
            }
        }
    }
}