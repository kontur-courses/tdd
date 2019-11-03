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
        private readonly Point center = new Point(800, 600);

        [SetUp]
        public void SetUp()
        {
            tagsCloud = new TagsCloudVisualization(center);
        }


        [Test]
        public void DoesNotTrowException_WhenPutFirstRectangle()
        {
            var rectangleSize = new Size(100, 100);
            Action act = () => tagsCloud.PutNextRectangle(rectangleSize);
            act.Should().NotThrow();
        }
    }
}
