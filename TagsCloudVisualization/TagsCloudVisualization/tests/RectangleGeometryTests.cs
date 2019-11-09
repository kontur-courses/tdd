using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.tests
{
    [TestFixture]
    class RectangleGeometryTest
    {
        private readonly Size _rectangleSize = new Size(1, 4);
        private readonly Point _rectangleLocation = new Point(3, 2);

        [Test]
        public void GetCornerRectangles_Should_ReturnRightRectangles()
        {
            var expectedRectangles = new List<Rectangle>()
            {
                new Rectangle(3, 2, 1, 4),
                new Rectangle(2, 2, 1, 4),
                new Rectangle(3, -2, 1, 4),
                new Rectangle(2,-2, 1, 4)
            };

            RectangleGeometry.GetCornerRectangles(_rectangleSize, new HashSet<Point>() { _rectangleLocation })
                .ToList()
                .Should()
                .BeEquivalentTo(expectedRectangles);
        }

        [Test]
        public void GetRectangleCorners_Should_ReturnRightCorners()
        {
            var expectedPoints = new List<Point>()
            {
                new Point(3, 2),
                new Point(4, 2),
                new Point(3, 6),
                new Point(4, 6)
            };

            new Rectangle(_rectangleLocation, _rectangleSize).GetCorners()
                .ToList()
                .Should()
                .BeEquivalentTo(expectedPoints);    
        }
    }
}
