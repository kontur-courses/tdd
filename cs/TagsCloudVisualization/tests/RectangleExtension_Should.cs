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
        [Test]
        public void ChangeLocation_WhenMoveRectangle()
        {
            var rectangle = new Rectangle(Point.Empty, new Size(1, 1));
            rectangle.Move(1, 0);
            rectangle.Location.Should().NotBeEquivalentTo(Point.Empty);
        }

        [Test]
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

        [Test]
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
            rectangle.Location.X.Should().Be(newLocation.X);
            rectangle.Location.Y.Should().Be(newLocation.Y);
        }
    }
}
