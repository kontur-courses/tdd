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

        [Test]
        public void FirstRectangle_MustBeNearCenter()
        {
            var rectangleSize = new Size(100, 100);
            var rectangle = tagsCloud.PutNextRectangle(rectangleSize);
            var deltaX = Math.Abs(rectangle.X - center.X);
            var deltaY = Math.Abs(rectangle.X - center.X);
            deltaX.Should().BeLessOrEqualTo(100);
            deltaY.Should().BeLessOrEqualTo(100);
        }

        [Test]
        public void Rectangles_Should_NotIntersectWithPrevious()
        {
            var rectangleSize = new Size(100, 100);
            var firstRectangle = tagsCloud.PutNextRectangle(rectangleSize);
            var SecondRectangle = tagsCloud.PutNextRectangle(rectangleSize);
            firstRectangle.IntersectsWith(SecondRectangle).Should().Be(false);
        }
    }
}
