using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    [TestFixture]
    class RectangleGeometryTest
    {
        [TestCase(1, 1, 1, 1)]
        [TestCase(3, 2, 1, 4)]
        [TestCase(1, 23, 434, 23)]
        public void GetCornerRectangles_Should_ReturnRightRectangles(int x, int y, int width, int height)
        {
            var expectedRec = new List<Rectangle>()
            {
                new Rectangle(x, y, width, height),
                new Rectangle(x - width, y, width, height),
                new Rectangle(x, y - height, width, height),
                new Rectangle(x - width, y - height, width, height)
            };
            RectangleGeometry
                .GetCornerRectangles(new Size(width, height), new HashSet<Point>() { new Point(x, y) })
                .ToList()
                .Should()
                .OnlyHaveUniqueItems()
                .And
                .BeEquivalentTo(expectedRec);
        }

        [TestCase(1, 2, 3, 4)]
        [TestCase(12, 24, 32, 4)]
        [TestCase(1, 21, 3, 432)]
        [TestCase(12, 2, 33, 14)]

        public void GetRectangleCorners_Should_ReturnRightRectangles(int x, int y, int width, int height)
        {
            var expectedPoints = new List<Point>()
            {
                new Point(x, y),
                new Point(x + width, y),
                new Point(x, y + height),
                new Point(x + width, y + height)
            };
            new Rectangle(x, y, width, height).GetCorners()
                .ToList()
                .Should()
                .OnlyHaveUniqueItems()
                .And
                .BeEquivalentTo(expectedPoints);
        }
    }
}
