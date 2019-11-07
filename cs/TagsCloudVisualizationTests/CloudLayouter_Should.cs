using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;
using System.IO;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CloudLayouter_Should
    {
        private Random rnd = new Random();
        private Point center;
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;
        private Size size;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(center);
            rectangles = new List<Rectangle>();
            size = GetRandomSize();
        }

        [TestCase(10, 10)]
        [TestCase(25, 25)]
        public void PutNextRectangle_WithoutChangingSize(int width, int height)
        {
            var size = new Size(width, height);
            var rectangle = layouter.PutNextRectangle(size);
            rectangles.Add(rectangle);
            rectangle.Size.Should().BeEquivalentTo(size);
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(-10, 0)]
        public void ThrowArgumentException_OnNonPositiveSize(int width, int height)
        {
            Action act = () => layouter.PutNextRectangle(new Size(width, height));
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0)]
        [TestCase(1000, 1000)]
        [TestCase(-1001, 1001)]
        public void PutFirstRectangle_NearCenter(int x, int y)
        {
            center = new Point(x, y);
            layouter = new CircularCloudLayouter(center);
            var rectangle = layouter.PutNextRectangle(size);
            rectangles.Add(rectangle);

            rectangle.GetCenter().X.Should().BeApproximately(x, (float)size.Width / 2);
            rectangle.GetCenter().Y.Should().BeApproximately(y, (float)size.Height / 2);
        }

        [Test]
        public void PutRectanglesOnLayout_WithoutIntersection()
        {
            var amountOfRectangles = rnd.Next(2, 25);

            while (amountOfRectangles-- > 0)
            {
                var nextRectangle = layouter.PutNextRectangle(GetRandomSize());
                var isIntersecting = rectangles.Any(rect => rect.IntersectsWith(nextRectangle));
                rectangles.Add(nextRectangle);
                isIntersecting.Should().BeFalse("rectangles should not intersect!");
            }

        }

        [TestCase(0, 0)]
        [TestCase(100, -100)]
        public void PutRectangles_DenselyAroundCenter(int x, int y)
        {
            center = new Point(x, y);
            layouter = new CircularCloudLayouter(center);

            var totalMass = 0.0;
            var amountOfRectangles = rnd.Next(25, 50);
            while(amountOfRectangles-- > 0)
            {
                var nextRectangle = layouter.PutNextRectangle(GetRandomSize());
                totalMass += nextRectangle.Width * nextRectangle.Height;
                rectangles.Add(nextRectangle);
            }

            var maxSquaredDistance = center.GetMaxSquaredDistanceTo(rectangles);
            var circleSize = Math.PI * maxSquaredDistance;
            var emptyArea = circleSize - totalMass;
            emptyArea.Should().NotBeInRange(circleSize / 2, double.MaxValue, 
                "more than half of the minimum circular area containing all" +
                "of the rectangles should not be empty");

        }

        [TearDown]
        public void TearDown()
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.Outcome.Status == TestStatus.Passed)
                return;
            var bitmap = CloudLayouterUtilities.GetCenteredBitmapFromRectangles(center, rectangles);
            var testImageDirectory = Path.Combine(testContext.WorkDirectory, "failed");
            var testImagePath = Path.Combine(testImageDirectory, $"{testContext.Test.FullName}.bmp");
            Directory.CreateDirectory(testImageDirectory);
            bitmap.Save(testImagePath);

            TestContext.WriteLine($"Tag cloud visualization saved to file {testImagePath}");
        }

        public Size GetRandomSize()
        {
            return new Size(rnd.Next(5, 100), rnd.Next(5, 100));
        }
    }
}
