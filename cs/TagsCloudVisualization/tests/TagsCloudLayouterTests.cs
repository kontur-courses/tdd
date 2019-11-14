using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.tools;


namespace TagsCloudVisualization.tests
{
    [TestFixture]
    public class TagsCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private Size rectangleSize;
        private CloudVisualizer visualizer = new CloudVisualizer(5000, 5000);


        [SetUp]
        public void SetUp()
        {
            center = new Point(7, 11);
            layouter = new CircularCloudLayouter(center);
            rectangleSize = new Size(6, 3);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            if (layouter.Cloud.Rectangles.Count == 0)
                return;

            var format = ImageFormat.Png;
            var fileName = $"{TestContext.CurrentContext.Test.Name}.{format}";
            var bitmap = visualizer.Draw(layouter.Cloud);
            var savePath = Path.Combine(TestContext.CurrentContext.TestDirectory, fileName);

            bitmap.Save(savePath, format);

            Console.WriteLine($"Tag cloud visualization saved to file \"{savePath}\"");
        }

        [Test]
        public void GetTagsCloud_ShouldNotNull()
        {
            var actual = layouter.Cloud;
            actual.Should().NotBeNull();
        }

        [Test]
        public void GetTagsCloudCenter_ShouldReturnRightPoint()
        {
            var actual = layouter.Cloud.Center;
            actual.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_WithValidSize_ShouldReturnRectangleWithThisSize()
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Size.Should().BeEquivalentTo(rectangleSize);
        }

        [Test]
        public void PutNextRectangle_CloudShouldContainsThisRectangle()
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            var actual = layouter.Cloud.Rectangles.Contains(rectangle);
            actual.Should().BeTrue();
        }

        [Test]
        public void PutNextRectangles_AfterPutNRectangles_ShouldNotIntersection()
        {
            var count = new Random().Next(50, 100);
            var rectangles = layouter.Cloud.Rectangles;

            RepeatPutNextRectangle(rectangleSize, count);

            rectangles.All(x => rectangles.Count(y => y.IntersectsWith(x)) == 1).Should().BeTrue();
        }

        [Test]
        [Timeout(500)]
        public void PutNextRectangle_Put1000BigRectangles_ShouldNotThrowException()
        {
            var size = new Size(100, 50);
            RepeatPutNextRectangle(size, 1000);
        }

        [Test]
        public void PutNextRectangle_TwoRectanglesWithEqualsSize_ShouldBeNotEquals()
        {
            var first = layouter.PutNextRectangle(rectangleSize);
            var second = layouter.PutNextRectangle(rectangleSize);

            first.Should().NotBeEquivalentTo(second);
        }

        [Test]
        public void PutNextRectangle_AfterPutNRectangles_CloudShouldContainsNRectangles()
        {
            var n = new Random().Next(50, 100);
            RepeatPutNextRectangle(rectangleSize, n);
            layouter.Cloud.Rectangles.Count.Should().Be(n);
        }

        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(500)]
        public void PutNextRectangle_DistanceOfAdjacentRectanglesShouldNotExceedN(int n)
        {
            RepeatPutNextRectangle(rectangleSize, n);

            var rectangles = layouter.Cloud.Rectangles;

            foreach (var rectangle in rectangles)
            {
                rectangles
                    .Count(other => rectangle.Distance(other) < Math.Max(rectangle.Width, rectangle.Height))
                    .Should()
                    .BeGreaterThan(3);
            }
        }

        [TestCase(15)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(500)]
        public void PutRectangle_AfterPutNRectangles_TheyShouldBeTightlySpaced(int rectanglesCount)
        {
            var rand = new Random(31337);
            RepeatPutNextRectangle(new Size(rand.Next(100, 200), rand.Next(100, 200)), rectanglesCount);
            
            var rectangles = layouter.Cloud.Rectangles;
            var hull = GeometryHelper.BuildConvexHull(rectangles);
            var cloudSquare = GeometryHelper.GetSquareOfConvexHull(hull);
            var rectanglesSquare = rectangles.Sum(x => x.Width * x.Height);

            var ration = cloudSquare / rectanglesSquare;

            ration.Should().BeLessOrEqualTo(1.5);
            Console.WriteLine(ration);
        }

        private void RepeatPutNextRectangle(Size rectanglesSize, int count)
        {
            for (var i = 0; i < count; i++)
            {
                layouter.PutNextRectangle(rectanglesSize);
            }
        }
    }
}