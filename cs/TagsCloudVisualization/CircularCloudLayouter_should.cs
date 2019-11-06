using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_should
    {
        private static readonly Point Center = new Point(10, 10);

        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(Center);
        }

        [TestCase(9, 9, 2, 2, Description = "Coordinates without bias")]
        [TestCase(10, 10, 1, 1, Description = "Coordinates with bias")]
        public void PutNextRectangle_CenterRectangle_OnTheFirstCall(int x, int y, int width, int height)
        {
            var expected = new Rectangle(x, y, width, height);

            var actual = layouter.PutNextRectangle(expected.Size);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect_AfterPuttingRectangles()
        {
            var rectangles = GenerateRandomRectangleSizes(30, 10, 10)
                .Select(layouter.PutNextRectangle);

            var intersections = rectangles
                .Where(f => rectangles.Any(s => f != s && f.IntersectsWith(s)));
            Assert.IsEmpty(intersections);
        }

        private static IEnumerable<TestCaseData> LocationTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new Func<Rectangle, bool>(rectangle => rectangle.X > Center.X)
                ).SetDescription("Rectangle is located to the right of the center");
                yield return new TestCaseData(
                    new Func<Rectangle, bool>(rectangle => rectangle.X < Center.X)
                ).SetDescription("Rectangle is located to the left of the center");
                yield return new TestCaseData(
                    new Func<Rectangle, bool>(rectangle => rectangle.Y > Center.Y)
                ).SetDescription("Rectangle is located above the center");
                yield return new TestCaseData(
                    new Func<Rectangle, bool>(rectangle => rectangle.Y < Center.Y)
                ).SetDescription("Rectangle is located below the center");
            }
        }

        [TestCaseSource(nameof(LocationTestCases))]
        public void PutNextRectangle_RectanglesAreLocatedAroundTheCenter_AfterPuttingRectangles(Func<Rectangle, bool> checkSide)
        {
            var rectangles = GenerateRandomRectangleSizes(10, 10, 10)
                .Select(layouter.PutNextRectangle);

            Assert.IsNotEmpty(rectangles.Where(checkSide));
        }

        private static IEnumerable<Size> GenerateRandomRectangleSizes(int count, int maxWidth, int maxHeight)
        {
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var width = random.Next(maxWidth) + 1;
                var height = random.Next(maxHeight) + 1;
                yield return new Size(width, height);
            }
        }
    }
}
