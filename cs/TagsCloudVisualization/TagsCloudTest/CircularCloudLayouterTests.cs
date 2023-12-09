using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudTest
{
    public class CircularCloudLayouterTests
    {
        private static List<Size> sizes;
        private static List<Rectangle> rectangles;
        private static CircularCloudLayouter circularLayouter;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var rnd = new Random();
            sizes = new();
            for (var i = 0; i < 500; i++)
            {
                sizes.Add(new(10 + rnd.Next(40), 1 + rnd.Next(40)));
            }
            circularLayouter = new CircularCloudLayouter(new(500, 500));
            foreach (var size in sizes)
            {
                circularLayouter.PutNextRectangle(size);
            }
            rectangles = circularLayouter.TagCloud;
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var path = $"{context.Test.Name}.png";
                TagCloudSaver.SaveAsPng(rectangles, path);
                Console.WriteLine($"Tag cloud visualization saved to file <{path}>");
            }
        }

        [Test]
        public void CountTest()
        {
            Assert.That(rectangles, Has.Count.EqualTo(sizes.Count));
        }

        [Test]
        public void IntersectTest()
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False);
                }
        }

        [Test]
        public void DensityTest()
        {
            var rectanglesSquare = .0;
            var maxdX = 0;
            var maxdY = 0;
            foreach (var rect in rectangles)
            {
                rectanglesSquare += rect.Width * rect.Height;
                maxdX = Math.Max(maxdX, Math.Abs(rect.X) + rect.Width / 2 - circularLayouter.Center.X);
                maxdY = Math.Max(maxdY, Math.Abs(rect.Y) + rect.Height / 2 - circularLayouter.Center.Y);
            }

            var radius = Math.Max(maxdX, maxdY);
            var circleSquare = Math.PI * radius * radius;

            Assert.That(rectanglesSquare / circleSquare, Is.GreaterThanOrEqualTo(0.8));
        }
    }
}
