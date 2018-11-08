using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class CircularCloudTests
    {
        private CircularCloudLayouter layouter;
        private Size size = new Size(20, 20);

        [SetUp]
        public void SetUp()
        {
            var point = new Point(0, 0);
            layouter = new CircularCloudLayouter(point);
        }

        [Test]
        public void Constructor_CreatesLayouterWithCenter()
        {
            layouter.Center.Should().Be(new Point(0, 0));
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectRectangle()
        {
            layouter.PutNextRectangle(size)
                .Should().Be(new Rectangle(layouter.Center, size));
        }

        [Test]
        public void PutNextRectangle_PlacesRectangleIntoCollection()
        {
            layouter.PutNextRectangle(size);
            layouter.Rectangles.Should().Contain(new Rectangle(layouter.Center, size));
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect()
        {
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(
                    random.Next(20, 201),
                    random.Next(20, 41)));
            }

            CloudVisualizer.CreateImage(layouter);

            var rectanglesChecked = 1;
            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles.Skip(rectanglesChecked++))
                {
                    rectangle.IntersectsWith(otherRectangle).Should().BeFalse();
                }
            }
        }
    }
}
