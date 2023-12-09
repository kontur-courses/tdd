using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudTest
{
    public class CircularCloudLayouterTests
    {
        private static List<Rectangle> rectangles;

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var path = new FileInfo($"..\\..\\..\\failed tests\\{context.Test.Name}.png");
                TagCloudSaver.SaveAsPng(rectangles, path.FullName);
                Console.WriteLine($"Tag cloud visualization saved to file <{path.FullName}>");
            }
        }

        [TestCase()]
        [TestCase(5, 10, 5, 15, 30)]
        [TestCase(5, 200, 5, 200)]
        public void Rectangles_ShouldNot_Intersect(
            int minRectangleWidth = 10,
            int maxRectangleWidth = 60,
            int minRectangleHeight = 10,
            int maxRectangleHeight = 30,
            int rectanglesCount = 300)
        {
            CreateRandomTagCloud(
                rectanglesCount,
                minRectangleWidth,
                maxRectangleWidth,
                minRectangleHeight,
                maxRectangleHeight);

            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False);
                }
        }

        [TestCase()]
        [TestCase(5, 10, 5, 15, 30)]
        [TestCase(5, 200, 5, 200)]
        public void CloudDensity_ShouldBe_EqualOrGreaterThan_70Percent(
            int minRectangleWidth = 10,
            int maxRectangleWidth = 60,
            int minRectangleHeight = 10,
            int maxRectangleHeight = 30,
            int rectanglesCount = 300)
        {
            CreateRandomTagCloud(
                rectanglesCount,
                minRectangleWidth,
                maxRectangleWidth,
                minRectangleHeight,
                maxRectangleHeight);

            var rectanglesSquare = .0;
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            foreach (var rectangle in rectangles)
            {
                rectanglesSquare += rectangle.Width * rectangle.Height;
                maxX = Math.Max(maxX, rectangle.Right);
                maxY = Math.Max(maxY, rectangle.Bottom);
                minX = Math.Min(minX, rectangle.Left);
                minY = Math.Min(minY, rectangle.Top);
            }

            var radius = (maxX - minX + maxY - minY) / 4.0;
            var circleSquare = Math.PI * radius * radius;

            Assert.That(rectanglesSquare / circleSquare, Is.GreaterThanOrEqualTo(0.7));
        }

        [Test]
        public void ShouldThrow_ArgumentNullException_IfExistingRectanglesNull()
        {
            Assert.That(
                () => new CircularCloudLayouter(new()).PutNextRectangle(new(), null),
                Throws.ArgumentNullException);
        }

        private void CreateRandomTagCloud(
            int rectanglesCount = 300,
            int minRectangleWidth = 10,
            int maxRectangleWidth = 60,
            int minRectangleHeight = 10,
            int maxRectangleHeight = 30,
            int centerX = 500,
            int centerY = 500)
        {
            var rnd = new Random();
            var circularLayouter = new CircularCloudLayouter(new(centerX, centerY));
            rectangles = [];
            for (var i = 0; i < rectanglesCount; i++)
            {
                var size = new Size(
                    minRectangleWidth + rnd.Next(maxRectangleWidth - minRectangleWidth + 1),
                    minRectangleHeight + rnd.Next(maxRectangleHeight - minRectangleHeight + 1));
                circularLayouter.PutNextRectangle(size, rectangles);
            }
        }
    }
}
