using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class RectangleTests
    {
        [Test]
        public void Center_Rectangle100x100onZeroPoint_ShouldBeHalfSide()
        {
            var rectangle = new Rectangle(0, 0, 100, 100);
            var expected = new Point(50, 50);

            var center = rectangle.Center();

            center.Should().Be(expected);
        }
    }
}
