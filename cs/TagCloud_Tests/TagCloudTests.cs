using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Layouting;

namespace TagCloudVisualization_Tests
{
    public class TagCloudTests
    {
        [Test]
        public void SaveToBitmap_ShouldThrow_WhenNoOneRectangleWasPut()
        {
            var tagCloud = new TagCloud.TagCloud(new CircularLayouter(Point.Empty),
                null, null);

            Action act = () => tagCloud.SaveBitmapTo(true, true, false);
            act.Should().Throw<ArgumentException>("image width and height can't be lower than 1");
        }
    }
}