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
            basicLayouter = new CircularCloudLayouter(new Point(20, 20));
        }

        [Test]
        public void Layouter_Should_HaveMap_WhenCreated()
        {
            basicLayouter.Map.Should().NotBeNull();
        }

        [Test]
        public void Layouter_Should_HaveRectanglesList_WhenCreated()
        {
            basicLayouter.Rectangles.Should().NotBeNull();
        }

        [Test]
        public void FirstRectangle_Should_HaveACornerInCenter()
        {
            var firstRectangle = basicLayouter.PutNextRectangle(new Size(20, 20));
            firstRectangle.X.Should().Be(basicLayouter.Center.X);
            firstRectangle.Y.Should().Be(basicLayouter.Center.Y);
        }

        [Test]
        public void RectanglesOfSameSize_ShouldNot_Intersect()
        {
            for (int i = 0; i < 50; i++)
                basicLayouter.PutNextRectangle(new Size(10, 10));
            for(int i=0;i<basicLayouter.Rectangles.Count;i++)
                for(int j=0;j<basicLayouter.Rectangles.Count;j++)
                    if(i!=j)
                        Assert.IsFalse(AreIntersected(basicLayouter.Rectangles[i],basicLayouter.Rectangles[j]));
                
            
        }
        public static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            return r1.Width + r2.Width >= Math.Max(r2.Left + r2.Width - r1.Left, r1.Left + r1.Width - r2.Left) &&
                   (r1.Height + r2.Height >= Math.Max(r2.Height + r2.Top - r1.Top, r1.Height + r1.Top - r2.Top));
        }
    }
}
