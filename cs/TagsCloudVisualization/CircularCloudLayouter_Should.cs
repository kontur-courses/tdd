using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private readonly Size canvasSize = new Size(1000, 1000);
        private readonly Point center = new Point(500, 500);
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(center);
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var path = $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.FullName}.png";

            Visualizer.GetImageFromRectangles(rectangles, canvasSize).Save(path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [Test]
        public void RectangleInCenter_WhenOneInputRectangle()
        {
            var size = new Size(100, 100);

            var rectangle = layouter.PutNextRectangle(size);

            rectangle.Should().Be(new Rectangle(new Point(center.X - 50, center.Y - 50), size));
        }

        [TestCase(8, 2)]
        [TestCase(8, 5)]
        [TestCase(13, 10)]
        [TestCase(12, 50)]
        [TestCase(15, 200)]
        public void RectanglesShouldNotIntersect(int randomSeed, int rectCount)
        {
            PutRectangles(randomSeed, rectCount);

            for (int i = 0; i < rectCount; i++)
            {
                for (int j = i+1; j < rectCount; j++)
                {
                    rectangles[i]
                        .IntersectsWith(rectangles[j])
                        .Should().BeFalse();
                }
            }
        }

        [TestCase(15, 400)]
        [TestCase(8, 800)]
        public void CloudShouldBeApproximateToCircle(int randomSeed, int rectCount)
        {
            PutRectangles(randomSeed, rectCount);

            var dist = rectangles
                .Select(rect => MaxDistanseToCenter(rect))
                .OrderBy(d => d)
                .Skip(Convert.ToInt32(rectangles.Count * 0.9));
                
            ( dist.Min() / dist.Max() ).Should().BeGreaterThan(0.71);
        }

        [TestCase(15, 400)]
        [TestCase(8, 800)]
        public void CloudShouldBeCompact(int randomSeed, int rectCount)
        {
            PutRectangles(randomSeed, rectCount);

            var dist = rectangles
                .Select(rect => MaxDistanseToCenter(rect));

            var radius = dist.Max();
            var square = rectangles.Select(r => r.Width * r.Height).Sum();
            (square/(Math.PI * radius * radius)).Should().BeGreaterThan(0.7);
        }

        private void PutRectangles(int seed, int count)
        {
            var rand = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(rand.Next(10, 45), rand.Next(10, 45))));
            }
        }

        private double MaxDistanseToCenter(Rectangle rect)
        {
            Point[] points = new Point[4]
            {
                rect.Location,
                new Point(rect.Location.X + rect.Width, rect.Location.Y),
                new Point(rect.Location.X + rect.Width, rect.Location.Y + rect.Height),
                new Point(rect.Location.X, rect.Location.Y + rect.Height)
            };

            return points.Max(point => Math.Sqrt(Math.Pow(center.X - point.X, 2) + Math.Pow(center.Y - point.Y, 2)));
        }
    }
}