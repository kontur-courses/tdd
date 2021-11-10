using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        [Test]
        public void AddRectangle()
        {
            var layouter = new CircularCloudLayouter(new Point());
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.Rectangles.Should().HaveCount(1);
        }

        [Test]
        public void FirstRectangleInCenter()
        {
            var layouter = new CircularCloudLayouter(new Point());
            var rect = layouter.PutNextRectangle(new Size(2, 2));
            var expected = new RectangleF(-1, -1, 2, 2);
            rect.Should().Be(expected);
        }
        
        [Test]
        public void AddTwoRectangles()
        {
            var layouter = new CircularCloudLayouter(new Point());
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.Rectangles.Should().HaveCount(2);
        }
        
        [Test]
        public void SecondRectangleContactsFirst()
        {
            var layouter = new CircularCloudLayouter(new Point());
            var rect1 = layouter.PutNextRectangle(new Size(2, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            rect1.Contacts(rect2).Should().BeTrue();
        }

        [Test]
        public void SecondRectangleOnCloseSide()
        {
            var layouter = new CircularCloudLayouter(new Point());
            var rect1 = layouter.PutNextRectangle(new Size(4, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            rect2.X.Should().Be(-1);
        }

        [Test]
        public void SecondAndThirdRectangleDifferent()
        {
            var layouter = new CircularCloudLayouter(new Point());
            layouter.PutNextRectangle(new Size(2, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            var rect3 = layouter.PutNextRectangle(new Size(2, 2));
            rect2.Should().NotBe(rect3);
        }
        
        [Test]
        public void SecondAndThirdRectangleOnDifferentSides()
        {
            var layouter = new CircularCloudLayouter(new Point());
            layouter.PutNextRectangle(new Size(4, 2));
            var rect2 = layouter.PutNextRectangle(new Size(2, 2));
            var rect3 = layouter.PutNextRectangle(new Size(2, 2));
            rect2.Y.Should().BeNegative();
            rect3.Y.Should().BePositive();
        }

        [Test]
        public void CanPlaceInCorner()
        {
            var layouter = new CircularCloudLayouter(new Point());
            for(var i = 0; i < 6; i++)
            {
                layouter.PutNextRectangle(new Size(2, 2));
            }
            var middleRectangle = layouter.Rectangles[0];
            var cornerRectangle = layouter.Rectangles[5];
            middleRectangle.Should().NotBe(cornerRectangle);
        }
    }
}