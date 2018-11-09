using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Classes;
namespace TagCloudTests
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter layouter ;
        
        [SetUp]
        public void SetUp()
        {
            var center = new Point(100, 100);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void LayouterIsEmpty_AfterCreation()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        public void Constructor_ThrowsArgumentException_OnNegativeCoordinates(int x, int y)
        {
            var center = new Point(x, y);
            Action act = () => new CircularCloudLayouter(center);;
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_SetrectangleToCenter_OnFirstTime()
        {
            var rectangle = layouter.PutNextRectangle(new Size(50, 40));
            var expectedCenter = new Point(100, 100);
            
            var firstRectangleCenter = rectangle.Location + new Size(rectangle.Width / 2, rectangle.Height / 2);
            
            firstRectangleCenter.Should().Be(expectedCenter);
        }
    }
}
