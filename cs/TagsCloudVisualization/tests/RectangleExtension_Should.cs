using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class RectangleExtension_Should
    {

        [TestCase(1, 0)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(100, 100)]
        public void Delta_Should_EqualDeltaAfterMove(int deltaXAfterMove, int deltaYAfterMove)
        {
            var rectangle = new Rectangle(Point.Empty, new Size(1, 1));
            rectangle.Move(deltaXAfterMove, deltaYAfterMove);
            rectangle.Location.X.Should().Be(Point.Empty.X + deltaXAfterMove);
            rectangle.Location.Y.Should().Be(Point.Empty.Y + deltaYAfterMove);
        }

        [TestCase(1, 0)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(100, 100)]
        public void Location_Should_BeEqualsNewLocation(int newX, int newY)
        {
            var newLocation = new Point(newX, newY);
            var rectangle = new Rectangle(Point.Empty, new Size(1, 1));
            rectangle.MoveToPosition(newLocation);
            rectangle.Location.Should().BeEquivalentTo(newLocation);
        }

        [TestCase(5, 4, 1, 0)]
        [TestCase(1, 3, 0, 0)]
        [TestCase(9, 7, 0, 1)]
        [TestCase(10, 4, -1, 0)]
        [TestCase(6, 9, 0, -1)]
        [TestCase(3, 5, 100, 100)]
        public void RectangleSize_Should_StayUnchanged(int width, int height, int newX, int newY)
        {
            var newLocation = new Point(newX, newY);
            var rectangle = new Rectangle(Point.Empty, new Size(width, height));
            var oldSize = rectangle.Size;
            rectangle.MoveToPosition(newLocation);
            rectangle.Size.Should().Be(oldSize);
        }
    }
}
