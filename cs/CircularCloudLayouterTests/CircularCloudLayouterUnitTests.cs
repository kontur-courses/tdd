using System.Drawing;
using TagsCloudVisualization.Models;
using FluentAssertions;
namespace CircularCloudLayouterTests
{
    [TestFixture]
    public class CircularCloudLayouterUnitTests
    {

        [Test]
        public void PutNextRectangle_ReturnsRightRectangle_AtCenter()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(100, 50);

            var rectangle = layouter.PutNextRectangle(size);

            var expectedRectangle = new Rectangle(-50, -25,
                                                  size.Width,size.Height);

            rectangle.Should()
                .BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_DoNotIntersect_WhenFirstRectangleIsAlreadyPlaced()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var firstRectangleSize = new Size(100, 50);
            var secondRectangleSize = new Size(200, 100);

            var firstRectangle = layouter.PutNextRectangle(firstRectangleSize);
            var secondRectangle = layouter.PutNextRectangle(secondRectangleSize);

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }
    }
}
