using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;
        private List<Rectangle> rectangles;
        private Point center;
        private readonly Random random = new Random();
        private const int DefaultWidthRectangle = 200;
        private const int DefaultHeightRectangle = 200;

        [SetUp]
        public void SetUp()
        {
            rectangles = new List<Rectangle>();
            center = new Point(0, 0);
            cloud = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            if (Equals(TestContext.CurrentContext.Result.Outcome.Status, ResultState.Failure.Status))
            {
                var width = 3000;
                var height = 3000;
                var visualizer = new RectangleCloudVisualizer();
                var shiftedRectangles = rectangles
                    .Select(r => new Rectangle(new Point(r.X + width/2, r.Y + height/2), r.Size)).ToList();
                var image = visualizer.GetImageCloud(shiftedRectangles, width, height, Color.Crimson, Color.BurlyWood);
                var filename = $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.FullName}.png";
                image.Save(filename);
                TestContext.WriteLine(filename);
            }
        }

        [Test]
        public void returnRectangleWithCorrectSize_AfterPut()
        {
            var size = new Size(100, 150);
            cloud.PutNextRectangle(size).Size.Should().Be(size);
        }

        [Test]
        public void returnRectangleInCenter_AfterFirstPutting()
        {
            var rectangleSize = new Size(59, 37);
            var rectangle = cloud.PutNextRectangle(rectangleSize);
            var expectedLocation = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            rectangle.Location.Should().Be(expectedLocation);
            rectangle.Size.Should().Be(rectangleSize);
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(300)]
        [TestCase(1000), Timeout(1000)]
        public void returnNotOverlappingRectangles_AfterPutting(int numberOfRectangles)
        {
            rectangles = PutRectanglesToCloud(numberOfRectangles);
            AssertDoNotIntersect(rectangles);
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void shiftRectangleToCenter_AfterPutting(int numberOfRectangles)
        {
            rectangles = PutRectanglesToCloud(numberOfRectangles);
            foreach (var rectangle in rectangles)
            {
                var rectangleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
                var directionY = Math.Sign(center.Y - rectangleCenter.Y);
                var directionX = Math.Sign(center.X - rectangleCenter.X);
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
            rectangles = PutRectanglesToCloud(numberOfRectangles);
            AssertDoNotIntersect(rectangles);
            var totalSquare = rectangles.Sum(r => r.Width * r.Height);
            var minX = rectangles.Min(r => r.X);
            var maxX = rectangles.Max(r => r.Right);
            var minY = rectangles.Min(r => r.Y);
            var maxY = rectangles.Max(r => r.Bottom);
            var squareBoundingRectangle = (maxX - minX) * (maxY - minY);
            ((double)totalSquare / squareBoundingRectangle).Should().BeGreaterThan(0.5);
        }

        [TestCase(200)]
        [TestCase(500)]
        [TestCase(1000)]
        public void striveToFormCircle_AfterPutting(int numberOfRectangles)
        {
            rectangles = PutRectanglesToCloud(numberOfRectangles);
            var distanceToLastRectangles = rectangles
                .Skip(numberOfRectangles * 9 / 10)
                .Select(r => r.MaxDistanceToPoint(center));
            var minDistance = double.MaxValue;
            var maxDistance = 0.0;
            foreach (var distance in distanceToLastRectangles)
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
                    
            }
            (minDistance/maxDistance).Should().BeGreaterThan(0.7);
        }

        private List<Rectangle> PutRectanglesToCloud(int numberOfRectangles)
        {
            for (var i = 0; i < numberOfRectangles; i++)
            {
                var rectangleSize = new Size(
                    (int)(DefaultWidthRectangle * random.NextDouble()),
                    (int)(DefaultHeightRectangle * random.NextDouble()));
                var rect = cloud.PutNextRectangle(rectangleSize);
                rectangles.Add(rect);
            }
            return rectangles;
        }

        private Tuple<Rectangle, Rectangle> GetPairIntersectingRectangles(List<Rectangle> rectanglesList)
        {
            for (var i = 0; i < rectanglesList.Count - 1; i++)
            {
                for (var j = i + 1; j < rectanglesList.Count; j++)
                {
                    if (rectanglesList[i].IntersectsWith(rectanglesList[j]))
                    {
                        return Tuple.Create(rectanglesList[i], rectanglesList[j]);
                    }
                }
            }
            return null;
        }

        private void AssertDoNotIntersect(List<Rectangle> rectanglesList)
        {
            var intersectingRectangles = GetPairIntersectingRectangles(rectanglesList);
            var message = "";
            if (intersectingRectangles != null)
            {
                message = $"intersecting rectangles: {intersectingRectangles.Item1} {intersectingRectangles.Item2}";
            }
            Assert.IsNull(intersectingRectangles, message);
        }
    }
}
