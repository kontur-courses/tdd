using System;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Point center;
        [SetUp]
        public void SetUp()
        {
            center = new Point(0,0);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void BeEmpty_OnCreate()
        {
            var layout = new CircularCloudLayouter(center);
            layout.GetCloud().Should().BeEmpty();
        }

        [Test]

        public void HaveRectangle_AfterPutting()
        {
            layouter.PutNextRectangle(new Size(10, 10));
            layouter.GetCloud().Count.Should().Be(1);
        }

        [Test]
        public void ReturnValidSizeRectangle_AfterPut()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Size.ShouldBeEquivalentTo(size);
        }
        [Test]
        public void ReturnValidPointRectangle_AfterPut()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Size.ShouldBeEquivalentTo(size);
        }

        [Test]
        public void HaveZeroPointCenter_FirstPut()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Point.ShouldBeEquivalentTo(center);
        }
        [Test]
        public void HaveNonZeroPointCenter_SecondPut()
        {
            var size = new Size(10, 10);
            layouter.PutNextRectangle(size);
            var secondRectangle = layouter.PutNextRectangle(size);
            Assert.AreNotEqual(secondRectangle.Point.X, center.X);
            Assert.AreNotEqual(secondRectangle.Point.Y, center.Y);

        }

        [Test]
        public void NewRectangleHaveValidPoint_SecondPut()
        {
            var firstSize = new Size(10,10);
            var firstRectangle = layouter.PutNextRectangle(firstSize);
            var secondSize = new Size(6,6);
            var secondRectangle = layouter.PutNextRectangle(secondSize);
            Assert.That(Math.Abs(firstRectangle.Point.X-secondRectangle.Point.X)==firstRectangle.Size.Width/2+secondRectangle.Size.Width/2);
            Assert.That(Math.Abs(firstRectangle.Point.Y - secondRectangle.Point.Y) == firstRectangle.Size.Height / 2 + secondRectangle.Size.Height/ 2);

        }

    }
}