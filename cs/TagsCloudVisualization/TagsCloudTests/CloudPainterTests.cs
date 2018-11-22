using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class CloudPainterTests
    {
        private CloudPainter painter;
        private CircularCloudLayouter tagCloud = new CircularCloudLayouter(Point.Empty);

        [SetUp]
        public void SetUp()
        {
            painter = new CloudPainter("cloud");
        }

        [Test]
        public void CloudPainter_CreatePainterWithFilename_ShouldReturnPainterWithPath()
        {
            painter.Path.Should().NotBeNullOrEmpty();
        }

        [TestCase(10, 20, 700, 700)]
        public void CreateNewTagCloud_ReturnCloudImage_WithSize(int rectangleWidth, int rectangleHeight, 
            int expectedCloudImageWidth, int expextedCloudImageHeight)
        {
            tagCloud.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));
            var image = painter.CreateNewTagCloud(tagCloud);
            image.Size.Should().BeEquivalentTo(new Size(expectedCloudImageWidth, expextedCloudImageHeight));
        }
    }
}