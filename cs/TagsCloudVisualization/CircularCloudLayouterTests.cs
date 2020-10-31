using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Point center = new Point(500, 500);

        [SetUp]
        public void SetLayouter()
        {
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void PutNextRectangle_PutRectangleInCenter_IfRectangleIsFirst()
        {
            var rectangle = layouter.PutNextRectangle(new Size(50, 50));
            rectangle.Location.Should().BeEquivalentTo(new Point(475, 475));
        }

        [Test]
        public void PutNextRectangle_PutDifferentRectanglesInDifferentPlaces_Always()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(50, 50));
            var secondRrectangle = layouter.PutNextRectangle(new Size(50, 50));
            firstRectangle.Location.Should().NotBeEquivalentTo(secondRrectangle.Location);
        }
    }
}
