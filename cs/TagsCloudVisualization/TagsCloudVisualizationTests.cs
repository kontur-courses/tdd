using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagsCloudVisualizationTests
    {

        private TagsCloudVisualization tagsCloud;

        [SetUp]
        public void SetUp()
        {
            tagsCloud = new TagsCloudVisualization(new Point(1, 2));
        }

        [Test]
        public void TagsCloudVisualization_Initialize_Params()
        {
            Assert.AreEqual(0,tagsCloud.Radius);
            Assert.AreEqual(0,tagsCloud.Angle);
            Assert.AreEqual(null,tagsCloud.WordPositions);
            Assert.AreEqual(new Point(1,2), tagsCloud.Center);
        }

        [TestCase(-1,2, TestName = "When_Width_Is_Negative")]
        [TestCase(1, -2, TestName = "When_Height_Is_Negative")]
        [TestCase(0,1, TestName = "When_Width_Is_Zero")]
        [TestCase(1, 0, TestName = "When_Height_Is_Zero")]
        public void PutNextRectangle_Throws_ArgumentException(int width,int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() => tagsCloud.PutNextRectangle(size));
        }

        [Test]
        public void TagsCloudVisualization_First_ElementPosition_Should_Be_Equal_To_Center()
        {
            tagsCloud.PutNextRectangle(new Size(4, 5));
            Assert.AreEqual(tagsCloud.Center, tagsCloud.WordPositions[0].Location);
        }

    }
}
