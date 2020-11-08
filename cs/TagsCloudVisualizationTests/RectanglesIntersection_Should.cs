using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class RectanglesIntersection_Should
    {
        private List<Rectangle> _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new List<Rectangle>
            {
                new Rectangle(new Point(0, 0), new Size(5, 5)),
                new Rectangle(new Point(7, -5), new Size(1, 3)),
                new Rectangle(new Point(-7, -5), new Size(5, 10))
            };
        }

        [TestCase(-5, -4, 2, 3,
            TestName = "When rectangle to check within rectangle")]
        [TestCase(1, 2, 5, 7,
            TestName = "When simple intersection")]
        public void IsAnyIntersectWithRectangles_ExistIntersections(int coordinateX, int coordinateY, int width,
            int height)
        {
            var rectangle = new Rectangle(new Point(coordinateX, coordinateY), new Size(width, height));

            var isIntersect = RectanglesIntersection.IsAnyIntersectWithRectangles(rectangle, _sut);

            isIntersect.Should().BeTrue();
        }

        [TestCase(5, 5, 3, 7,
            TestName = "When exist common points")]
        [TestCase(6, 6, 3, 7,
            TestName = "When not exist common points")]
        public void IsAnyIntersectWithRectangles_NotIntersections(int coordinateX, int coordinateY, int width,
            int height)
        {
            var rectangle = new Rectangle(new Point(coordinateX, coordinateY), new Size(width, height));

            var isIntersect = RectanglesIntersection.IsAnyIntersectWithRectangles(rectangle, _sut);

            isIntersect.Should().BeFalse();
        }
    }
}