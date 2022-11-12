using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Core.Helpers;

namespace TagsCloudVisualization.Core.Tests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter circularCloudLayouter;

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
            circularCloudLayouter = new CircularCloudLayouter(new Point(600, 600));

            Action act = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 5, TestName = "PutNextRectangle. x, y > 0 and x = y")]
        [TestCase(5, 10, TestName = "PutNextRectangle. x, y > 0 and x < y")]
        [TestCase(10, 5, TestName = "PutNextRectangle. x, y > 0 and x > y")]
        public void PutNextRectangle_WhenCorrectArgsPutRect_ShouldBeEqualSizes(int width, int height)
        {
            var size = new Size(width, height);
            circularCloudLayouter = new CircularCloudLayouter(new Point(600, 600));

            var rect = circularCloudLayouter.PutNextRectangle(new Size(width, height));
            rect.Size.Should().Be(size);
        }

        [TestCase(1200, 1200, TestName = "PutNextRectangle. 1200x1200, the first rectangle in the center")]
        [TestCase(600, 600, TestName = "PutNextRectangle. 600x600, the first rectangle in the center")]
        [TestCase(555, 555, TestName = "PutNextRectangle. Odd size, the first rectangle in the center")]
        public void PutNextRectangle_FirstRectInCenter_ShouldBeTrue(int height, int width)
        {  
            var rnd = new Random();
            var center = new Point(width / 2, height / 2);
            circularCloudLayouter = new CircularCloudLayouter(center);

            circularCloudLayouter.PutNextRectangle(new Size(rnd.Next(10, 20), rnd.Next(15, 30)));
            var rect = circularCloudLayouter.Rectangles[0];

            center.X.Should().BeInRange((rect.Left + rect.Right) / 2 - 1, (rect.Left + rect.Right) / 2 + 1);
            center.Y.Should().BeInRange((rect.Bottom + rect.Top) / 2 - 1, (rect.Bottom + rect.Top) / 2 + 1);
        }

        [TestCase(1200, 1200, 50, TestName = "PutNextRectangle. Rects must not intersect")]
        public void PutNextRectangle_RectsIsNotIntersects_ShouldBeTrue(int height, int width, int rectsCount)
        {
            var rnd = new Random();
            circularCloudLayouter = new CircularCloudLayouter(new Point(height / 2, width / 2));
            
            for (var i = 0; i < rectsCount; i++)
                circularCloudLayouter.PutNextRectangle(new Size(rnd.Next(10, 20), rnd.Next(15, 30)));
            
            foreach (var rect in circularCloudLayouter.Rectangles)
                IsNotIntersectedWithAnyAndExcludeSelf(rect).Should().BeTrue();
            
            bool IsNotIntersectedWithAnyAndExcludeSelf(Rectangle rect)
            {
               return circularCloudLayouter.Rectangles.Any(p => !p.IntersectsWith(rect) && p != rect);
            }
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, $"{TestContext.CurrentContext.Test.Name}.bmp");
            var bmpSaver = new BitmapSaver(new Size(1200, 1200));

            bmpSaver.Draw(circularCloudLayouter.Rectangles);
            bmpSaver.Save(path);

            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}