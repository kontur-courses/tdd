using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0));
        }
        
        [Test]
        public void RectanglesShouldBeEmpty_AfterLayouterInitialization()
        {
            layouter.Rectangles.Should().BeEmpty();
        }
        
        [Test]
        public void FirstRectangleShouldBeCentral()
        {
            layouter.PutNextRectangle(new Size(10, 10)).Location
                .Should().Be(layouter.Center);
        }
        
        [Test]
        public void SecondRectangleShouldBeHigherThanFirst()
        {
            var first = layouter.PutNextRectangle(new Size(20, 20));
            var second = layouter.PutNextRectangle(new Size(7, 10));
            var location = new Point(
                first.X,
                first.Y + first.Height + layouter.RectangleMargin
            );

            second.Location.Should().Be(location);
        }

        [Test]
        public void ThirdRectangleShouldBeOnTheRight()
        {
            layouter.PutNextRectangle(new Size(20, 20));
            var second = layouter.PutNextRectangle(new Size(30, 10));
            var third = layouter.PutNextRectangle(new Size(10, 25));
            var location = new Point(
                second.X + second.Width + layouter.RectangleMargin,
                second.Y - third.Height - layouter.RectangleMargin
            );

            third.Location.Should().Be(location);
        }
        
        [Test]
        public void FourthRectangleShouldBeOnTheBottom()
        {
            layouter.PutNextRectangle(new Size(20, 20));
            layouter.PutNextRectangle(new Size(30, 10));
            var third = layouter.PutNextRectangle(new Size(10, 25));
            var fourth = layouter.PutNextRectangle(new Size(35, 25));
            var location = new Point(
                third.X - layouter.RectangleMargin - fourth.Width,
                third.Y - fourth.Height - layouter.RectangleMargin
            );

            fourth.Location.Should().Be(location);
        }
        
        [Test]
        public void FifthRectangleShouldBeOnTheLeft()
        {
            layouter.PutNextRectangle(new Size(20, 20));
            layouter.PutNextRectangle(new Size(30, 10));
            layouter.PutNextRectangle(new Size(10, 25));
            var fourth = layouter.PutNextRectangle(new Size(35, 25));
            var fifth = layouter.PutNextRectangle(new Size(25, 25));
            var location = new Point(
                fourth.X - fifth.Width - layouter.RectangleMargin,
                fourth.Y + fourth.Height + layouter.RectangleMargin
            );

            fifth.Location.Should().Be(location);
        }

        [Test]
        public void RectanglesShouldntIntersect()
        {
            layouter.PutNextRectangle(new Size(20, 20));
            layouter.PutNextRectangle(new Size(30, 10));
            layouter.PutNextRectangle(new Size(10, 25));
            layouter.PutNextRectangle(new Size(35, 25));
            layouter.PutNextRectangle(new Size(25, 25));
            for (var i = 0; i < layouter.Rectangles.Count; i++)
            {
                for (var j = i + 1; j < layouter.Rectangles.Count; j++)
                    layouter.Rectangles[i].IntersectsWith(layouter.Rectangles[j]).Should().BeFalse();
            }
        }
    }
}