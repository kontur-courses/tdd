using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_should
    {
        public CircularCloudLayouter cloud;

        [SetUp]
        public void Init()
        {
            var center = new Point(500, 500);
            cloud = new CircularCloudLayouter(center);
        }

        [Test]
        public void InitializeCloudCenter_AfterInitialization()
        {
            var center = new Point(400, 400);
            var cloudLayouter = new CircularCloudLayouter(center);
            cloudLayouter.center.ShouldBeEquivalentTo(center);
        }

        [Test]
        public void NotChangeRectangleSize_AfterAddition()
        {
            var wordBox = new Size(100, 100);

            var size = cloud.PutNextRectangle(wordBox).Size;

            size.ShouldBeEquivalentTo(wordBox);
        }

        [TestCase(1, 450, 450, TestName = "Single addition")]
        [TestCase(2, 550, 350, TestName = "Double addition")]
        [TestCase(3, 450, 350, TestName = "Triple addition")]
        [TestCase(4, 350, 350, TestName = "Quadruple addition")]
        [TestCase(5, 350, 450, TestName = "Multiple addition")]
        public void PutRectangleToTheCorrectPlace_AfterAddition(int count, int expectedX, int expectedY)
        {
            var wordBox = new Size(100, 100);

            for (var i = 0; i < count - 1; i++)
                cloud.PutNextRectangle(wordBox);
            var fourthRectangle = cloud.PutNextRectangle(wordBox);

            var expectedLocation = new Point(expectedX, expectedY);
            fourthRectangle.Location.Should().Be(expectedLocation);
        }

        [TestCase(2, TestName = "When only two rectangles")]
        [TestCase(10, TestName = "When myltiple rectangles")]
        [TestCase(100, TestName = "When a lot of rectangles")]
        public void PutNextRectangleWithoutIntersectionOfOtherRectangle_AfterAddition(int count)
        {
            var wordBox = new Size(100, 100);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < count; i++)
                rectangles.Add(cloud.PutNextRectangle(wordBox));

            rectangles.Select(x => rectangles.Any(y => x != y && y.IntersectsWith(x))).ShouldAllBeEquivalentTo(false);
        }

        [TestCase(1000), Timeout(1000)]
        public void PutNextRectangle_LessThanOneSecond_OnMultipleAddition(int count)
        {
            var wordBox = new Size(100, 100);
            for (var i = 0; i < count; i++)
                cloud.PutNextRectangle(wordBox);
        }

        [Test]
        public void ThrowArgumetException_AfterAdditionTheNotPositiveSize()
        {
            var incorrectSize = new Size(-100, 100);
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(incorrectSize),
                "Rectangle size must be a positive");
        }
    }
}
