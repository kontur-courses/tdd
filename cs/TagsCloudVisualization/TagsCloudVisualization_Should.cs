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
        public void ThrowException_WhenSizeHaveNegativeNumber()
        {
            var rectangleSize = new Size(-1, 100);
            Action act = () => tagsCloud.PutNextRectangle(rectangleSize);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void FirstRectangle_MustBeNearCenter()
        {
            var rectangleSize = new Size(100, 100);
            var rectangle = tagsCloud.PutNextRectangle(rectangleSize);
            var deltaX = Math.Abs(rectangle.X - center.X);
            var deltaY = Math.Abs(rectangle.X - center.X);
            deltaX.Should().BeInRange(-100, 100);
            deltaY.Should().BeInRange(-100, 100);
        }

        [Test]
        public void Rectangles_Should_NotIntersectWithPrevious()
        {
            var rectangleSize = new Size(100, 100);
            var firstRectangle = tagsCloud.PutNextRectangle(rectangleSize);
            var secondRectangle = tagsCloud.PutNextRectangle(rectangleSize);
            firstRectangle.IntersectsWith(secondRectangle).Should().Be(false);
        }

        [Test]
        public void DifferentBetweenDeltaFirstAndLastYAndDeltaFirstAndLastX_Should_BeLessThanHalfSumHeightOrWidth()
        {
            var rectangleSize = new Size(100, 100);
            var maxY = -1;
            var minY = int.MaxValue;
            var maxX = -1;
            var minX = int.MaxValue;
            for (var i = 0; i < 100; i++)
            {
                var rectangle = tagsCloud.PutNextRectangle(rectangleSize);
                maxY = rectangle.Y > maxY ? rectangle.Y : maxY;
                minY = rectangle.Y < minY ? rectangle.Y : minY;
                maxX = rectangle.X > maxX ? rectangle.X : maxX;
                minX = rectangle.X < minX ? rectangle.X : minX;
            }
            ((maxY - minY) - (maxX-minX)).Should().BeLessThan(100 * 50);
        }
    }
}
