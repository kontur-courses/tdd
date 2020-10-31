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
        public void PutNextRectangle_PutDifferentRectanglesInDifferentPlaces()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(50, 50));
            var secondRectangle = layouter.PutNextRectangle(new Size(50, 50));
            firstRectangle.Location.Should().NotBeEquivalentTo(secondRectangle.Location);
        }

        [Test]
        public void PutNextRectangle_ReturnsNotIntersectedRectangles()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(50, 50));
            var secondRectangle = layouter.PutNextRectangle(new Size(50, 50));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void CircularCloudLayouter_CreateDenseCloud()
        {
            var firstRectangle = layouter.PutNextRectangle(new Size(50, 50));
            var secondRectangle = layouter.PutNextRectangle(new Size(50, 50));
            Rectangle tempRectangle = new Rectangle(0, 0, 50, 50);
            foreach(var direction in layouter.Directions)
            {
                tempRectangle.X = firstRectangle.X + direction.Item1 * layouter.Shift;
                tempRectangle.X = firstRectangle.Y + direction.Item2 * layouter.Shift;
                if (layouter.GetDistanceToCenter(tempRectangle) < layouter.GetDistanceToCenter(firstRectangle))
                {
                    tempRectangle.IntersectsWith(secondRectangle).Should().BeTrue();
                }
                tempRectangle.X = secondRectangle.X + direction.Item1 * layouter.Shift;
                tempRectangle.X = secondRectangle.Y + direction.Item2 * layouter.Shift;
                if (layouter.GetDistanceToCenter(tempRectangle) < layouter.GetDistanceToCenter(secondRectangle))
                {
                    tempRectangle.IntersectsWith(firstRectangle).Should().BeTrue();
                }
            }
        }
    }
}
