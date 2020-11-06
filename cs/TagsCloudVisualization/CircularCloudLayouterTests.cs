using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(1200, 1200));
        }

        
        [Test]
        public void FirstRectangle_ShouldBeCentral()
        {
            layouter.PutNextRectangle(new Size(10, 10)).Location
                .Should().Be(new Point(layouter.Center.X - 5, layouter.Center.Y - 5));
        }

        [Test]
        public void Rectangles_ShouldntIntersect()
        {
            var rng = new Random();
            var rectangles = Enumerable
                .Range(0, 200)
                .Select(_ => new Size(rng.Next(100, 200), rng.Next(50, 100)))
                .Select(size => layouter.PutNextRectangle(size))
                .ToList();
            
            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                    Rectangle.Intersect(rectangles[j], rectangles[i]).Should().Be(Rectangle.Empty);
            }
        }

        [Test]
        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        public void ShouldThrowOnIncorrectRectangle(int width, int height)
        {
            Action action = () => { layouter.PutNextRectangle(new Size(width, height)); };
            action.Should().Throw<ArgumentException>().WithMessage("Rectangle size should be positive.");
        }

        [Test]
        public void PutNextRectangle_ShouldReturnCorrectRectangle()
        {
            var rng = new Random();
            for (var i = 0; i < 100; i++)
            {
                var size = new Size(rng.Next(10, 100), rng.Next(10, 100));
                layouter.PutNextRectangle(size).Size.Should().Be(size);
            }
        }

        [Test]
        public void Rectangles_ShouldBeInsideCircle()
        {
            var rng = new Random();
            var rectangles = Enumerable
                .Range(0, 200)
                .Select(_ => new Size(rng.Next(100, 200), rng.Next(50, 100)))
                .Select(size => layouter.PutNextRectangle(size))
                .ToList();
            var square = rectangles
                .Select(rectangle => rectangle.Width * rectangle.Height)
                .Sum();
            var radius = (int)Math.Ceiling(Math.Sqrt(square / Math.PI) * 1.25);
            
            foreach (var rectangle in rectangles)
            {
                var distance = GetMaximumDistance(rectangle, layouter.Center);
                distance.Should().BeLessThan(radius);
            }
        }

        private double GetMaximumDistance(Rectangle rectangle, Point center)
        {
            var maxX = Math.Max(Math.Abs(center.X - rectangle.X), Math.Abs(center.X - rectangle.X - rectangle.Width));
            var maxY = Math.Max(Math.Abs(center.Y - rectangle.Y), Math.Abs(center.Y - rectangle.Y - rectangle.Height));
            return Math.Sqrt(maxX * maxX + maxY * maxY);
        }
    }
}