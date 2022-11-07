using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization.Core.Tests
{
    public class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void Setup()
        {
            center = new Point(100, 100);
            circularCloudLayouter = new CircularCloudLayouter(center);
        }

        [TestCase(5, 5, TestName = "Ctor. x, y > 0 and x = y")]
        [TestCase(5, 15, TestName = "Ctor. x, y > 0 and x != y")]
        [TestCase(0, 0, TestName = "Ctor. x, y = 0")]
        public void Constructor_WhenCorrectArgs_ShouldNotThrow(int x, int y)
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(x, y)));
        }
        [TestCase(-5, -5, TestName = "Ctor. x, y < 0")]
        [TestCase(-5, 5, TestName = "Ctor. x < 0 and y > 0")]
        [TestCase(5, -5, TestName = "Ctor. x > 0 and y < 0")]
        public void Constructor_WhenIncorrectArgs_ShouldThrow(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(x, y)));
        }

        [TestCase(-5, -5, TestName = "x, y < 0")]
        [TestCase(-5, 5, TestName = "x < 0 and y > 0")]
        [TestCase(5, -5, TestName = "x > 0 and y < 0")]
        [TestCase(0, 0, TestName = "x, y = 0")]
        public void PutNextRectangle_WhenIncorrectArgs_ShouldThrow(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => circularCloudLayouter.PutNextRectangle(new Size(width, height)));
        }

        [TestCase(5, 5, TestName = "x, y > 0 and x = y" )]
        [TestCase(5, 10, TestName = "x, y > 0 and x < y")]
        [TestCase(10, 5, TestName = "x, y > 0 and x > y")]
        public void PutNextRectangle_WhenCorrectArgsPutRect_ShouldBeEqualSizes(int width, int height)
        {
            var size = new Size(width, height);
            var rect = circularCloudLayouter.PutNextRectangle(new Size(width, height));
            rect.Size.Should().Be(size);
        }

        [Test]
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