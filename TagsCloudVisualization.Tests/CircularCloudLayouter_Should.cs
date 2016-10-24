using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;
        private Random random = new Random();
        private int defaultWidthRectangle = 200;
        private int defaultHeightRectangle = 200;

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));
        }

        [TearDown]
        public void TearDown()
        {
            if (Equals(TestContext.CurrentContext.Result.Outcome.Status, ResultState.Failure.Status))
            {
                var picture = new Picture(cloud);
                picture.DrawRectanglesFromCloud();
                var filename = $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.FullName}.png";
                picture.SaveToFile(filename);
                TestContext.WriteLine(filename);
            }
        }

        private void AssertDoNotIntersect(List<Rectangle> rectangles)
        {
            var intersectingRectangles = GetPairIntersectingRectangles(rectangles);
            string message = "";
            if (intersectingRectangles != null)
                message = String.Format("{0}: {1} {2}",
                    "intersecting rectangles",
                    intersectingRectangles.Item1.ToString(),
                    intersectingRectangles.Item2.ToString());
            Assert.IsNull(intersectingRectangles, message);

        }

        private Tuple<Rectangle, Rectangle> GetPairIntersectingRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return Tuple.Create(rectangles[i], rectangles[j]);
                }
            }
            return null;
        }

        [Test]
        public void returnRectangleWithCorrectSize_AfterPut()
        {
            Size size = new Size(100, 150);
            cloud.PutNextRectangle(size).Size.Should().Be(size);
        }

        [Test]
        public void returnRectangleInCenter_AfterFirstPutting()
        {
            var center = cloud.Center;
            var rectangleSize = new Size(59, 37);
            var rectangle = cloud.PutNextRectangle(rectangleSize);
            var expectedLocation = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            rectangle.Location.Should().Be(expectedLocation);
            rectangle.Size.Should().Be(rectangleSize);
        }

        private List<Rectangle> PutRectanglesToCloud(int numberOfRectangles)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < numberOfRectangles; i++)
            {
                var rectangleSize = new Size(
                    (int)(defaultWidthRectangle * random.NextDouble()),
                    (int)(defaultHeightRectangle * random.NextDouble()));
                var rect = cloud.PutNextRectangle(rectangleSize);
                rectangles.Add(rect);
            }
            return rectangles;
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(300)]
        [TestCase(1000), Timeout(1000)]
        public void returnNotOverlappingRectangles_AfterPutting(int numberOfRectangles)
        {
            var rectangles = PutRectanglesToCloud(numberOfRectangles);
            AssertDoNotIntersect(rectangles);
        }


        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void shiftRectangleToCenter_AfterPutting(int numberOfRectangles)
        {
            var rectangles = PutRectanglesToCloud(numberOfRectangles);
            foreach (var rectangle in rectangles)
            {
                var rectengleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
                var directionY = Math.Sign(cloud.Center.Y - rectengleCenter.Y);
                var directionX = Math.Sign(cloud.Center.X - rectengleCenter.X);
                if (directionX != 0 || directionY != 0)
                {
                    var newLocation = new Point(rectangle.X + directionX, rectangle.Y + directionY);
                    var newRectangle = new Rectangle(newLocation, rectangle.Size);
                    var numberOfIntersection = rectangles.Where(r => r != rectangle).Count(r => r.IntersectsWith(newRectangle));
                    numberOfIntersection.Should().BeGreaterThan(0);
                }
            }
        }

        [TestCase(100)]
        [TestCase(500)]
        [TestCase(1000)]
        public void placeRectanglesTightly_AfterPutting(int numberOfRectangles)
        {
            var rectangles = PutRectanglesToCloud(numberOfRectangles);
            AssertDoNotIntersect(rectangles);
            var totalSquare = rectangles.Sum(r => r.Width * r.Height);
            var minX = rectangles.Min(r => r.X);
            var maxX = rectangles.Max(r => r.Right);
            var minY = rectangles.Min(r => r.Y);
            var maxY = rectangles.Max(r => r.Bottom);
            int squareBoundingRectangle = (maxX - minX) * (maxY - minY);
            ((double)totalSquare / squareBoundingRectangle).Should().BeGreaterThan(0.5);
        }
    }
}
