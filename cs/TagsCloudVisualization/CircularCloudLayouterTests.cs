using System;
using System.Drawing;
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
            layouter = new CircularCloudLayouter(new Point(600, 600));
        }
        
        [Test]
        public void RectanglesShouldBeEmpty_AfterLayouterInitialization()
        {
            layouter.Rectangles.Should().BeEmpty();
        }
        
        [Test]
        public void FirstRectangleShouldBeCentral()
        {
            layouter.PutNextRectangle(new Size(10, 10)).Location
                .Should().Be(new Point(layouter.Center.X - 5, layouter.Center.Y - 5));
        }

        [Test]
        public void RectanglesShouldntIntersect()
        {
            var rng = new Random();
            for (var i = 0; i < 200; i++)
                layouter.PutNextRectangle(new Size(rng.Next(10, 40), rng.Next(10, 40)));

            new Visualization(layouter.Center.X * 2, layouter.Center.Y * 2).DrawRectangles(layouter.Rectangles);

            for (var i = 0; i < layouter.Rectangles.Count; i++)
            {
                for (var j = i + 1; j < layouter.Rectangles.Count; j++)
                    Rectangle.Intersect(layouter.Rectangles[j], layouter.Rectangles[i]).Should().Be(Rectangle.Empty);
            }
            
        }
    }
}