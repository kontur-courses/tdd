using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class TagsCloudShould
    {
        private CircularCloudLayouter basicLayouter;

        [TestCase(0, 0)]
        public void ShouldCorrectlyCreated_When_Create(int x, int y)
        {
            var circularLayouter = new CircularCloudLayouter(new Point(x, y));
            circularLayouter.Center.Should().Be(new Point(x, y));

        }

        [SetUp]
        public void SetUp()
        {
            basicLayouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void Layouter_Should_HaveRectanglesList_WhenCreated()
        {
            basicLayouter.Rectangles.Should().NotBeNull();
        }

        [TestCase(0, 10, TestName = "WhenWidthIsZero")]
        [TestCase(10, 0, TestName = "WhenHeightIsZero")]
        [TestCase(0, 0, TestName = "WhenWidthAndHeightIsZero")]
        [TestCase(-1, 10, TestName = "WhenWidthIsNegative")]
        [TestCase(10, -1, TestName = "WhenHeightIsNegative")]
        [TestCase(-2, -1, TestName = "WhenWidthAndHeightIsNegative")]
        public void ShouldThrowsArgumentException_When_PuttedRectangleHaveIncorrectDimension(int w, int h)
        {
            Action action = () => basicLayouter.PutNextRectangle(new Size(w, h));
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(20, 20, -10, 10)]
        [TestCase(13, 13, -6, 6)]
        public void GetCenterOfFirstRectangle(int w, int h, int x, int y)
        {
            basicLayouter.PutNextRectangle(new Size(w, h)).Location.Should().Be(new Point(x, y));
        }

        [TestCase(-5,5,10,10,-5,-6,10,10,ExpectedResult = false)]
        [TestCase(-5,5,10,10,-4,4,2,2,ExpectedResult = true)]
        [TestCase(-5,5,10,10,-10,3,30,2,ExpectedResult = true)]
        [TestCase(-5,5,10,10,5,-5,10,10,ExpectedResult = true)]
        public bool MethodAreIntersected_Should_WorkCorrectly(int x1,int y1,int w1,int h1,int x2,int y2,int w2,int h2)
        {
            return basicLayouter.AreIntersected(new Rectangle(x1, y1, w1, h1), new Rectangle(x2, y2, w2, h2));
        }
        [Test]
        public void AllRectangles_Should_BeInRectanglesList()
        {
            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                basicLayouter.PutNextRectangle(new Size(random.Next(1, 50), random.Next(1, 50)));
            }

            basicLayouter.Rectangles.Count.Should().Be(50);
        }

        [Test]
        public void RectanglesOfSameSize_ShouldNot_Intersect()
        {
            for (int i = 0; i < 50; i++)
                basicLayouter.PutNextRectangle(new Size(10, 10));
            for (int i = 0; i < basicLayouter.Rectangles.Count; i++)
                for (int j = 0; j < basicLayouter.Rectangles.Count; j++)
                    if (i != j)
                        Assert.IsFalse(basicLayouter.AreIntersected(basicLayouter.Rectangles[i], basicLayouter.Rectangles[j]));
        }

        [Test]
        public void RectanglesOfDifferentSize_ShouldNot_Intersect()
        {
            Random random = new Random();
            for (int i = 0; i < 50; i++)
                basicLayouter.PutNextRectangle(new Size(random.Next(1, 50), random.Next(1, 50)));
            for (int i = 0; i < basicLayouter.Rectangles.Count; i++)
                for (int j = 0; j < basicLayouter.Rectangles.Count; j++)
                    if (i != j)
                        Assert.IsFalse(basicLayouter.AreIntersected(basicLayouter.Rectangles[i], basicLayouter.Rectangles[j]));
        }
    }
}
