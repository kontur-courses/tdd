using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagsCloudVisualization_Should
    {
        private TagsCloudVisualization tagsCloud;

        [SetUp]
        public void SetUp()
        {
            tagsCloud = new TagsCloudVisualization(new Point(800, 600));
        }


        [Test]
        public void DoesNotTrowException_WhenPutFirstRectangle()
        {
            var rectangleSize = new Size(100, 100);
            tagsCloud.PutNextRectangle(rectangleSize)
                .Should()
                .BeEquivalentTo(new Rectangle(
                new Point(tagsCloud.GetCenter().X - (rectangleSize.Width / 2), tagsCloud.GetCenter().Y - (rectangleSize.Height / 2)), rectangleSize));
        }
    }
}
