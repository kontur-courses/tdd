using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;
using System.Drawing;

namespace TagsCloudTests
{
    [TestFixture]
    public class TagCloudTests
    {
        private CircularCloudLayouter tagCloud;
        [SetUp]
        public void SetUp()
        {
            var center = new Point(0, 0);
            tagCloud = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void PutNewRectangle_ReturnsRectangleWithCenterCoordinates_ForFirstCall()
        {
            var rectangleSize = new Size(10, 20);
            tagCloud.PutNextRectangle(rectangleSize).Location.Should().BeEquivalentTo(
                new Point(tagCloud.Center.X - rectangleSize.Width / 2, tagCloud.Center.X - rectangleSize.Height / 2 ));
        }

        [Test]
        public void PutNewRectangle_ChangesCloudBoundaries_WhenAddingRectangle()
        {
            var rectangleSize = new Size(10, 5);
            tagCloud.PutNextRectangle(rectangleSize).Should().BeEquivalentTo(tagCloud.GetRectangle);
        }
    }
}