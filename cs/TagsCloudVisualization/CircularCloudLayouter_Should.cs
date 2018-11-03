using System;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private static readonly Size[] _testRectsSizes =
        {
            new Size(100, 30), 
            new Size(150, 50), 
            new Size(80, 50), 
            new Size(200, 70), 
            new Size(144, 32), 
            new Size(100, 60), 
            new Size(123, 30), 
            new Size(200, 40), 
            new Size(248, 100), 
        };

        private readonly Point _layouterCenter = new Point(0, 0);
        
        [Test]
        public void Should_PutFirstRectangleWithCorrectPos()
        {
            var layouter = new CircularCloudLayouter(_layouterCenter);
            var rectSize = new Size(100, 10);
            var expectedRect = new Rectangle(_layouterCenter, rectSize);
            layouter.PutNextRectangle(rectSize).Should().BeEquivalentTo(expectedRect);
        }

        [Test]
        public void Should_PlaceManyRectanglesWithoutOverlaps()
        {
            var layouter = new CircularCloudLayouter(_layouterCenter);
            
            foreach (var rect in _testRectsSizes)
            {
                layouter.PutNextRectangle(rect);
            }
            
            foreach (var rectA in layouter.Rects)
            {
                foreach (var rectB in layouter.Rects)
                {
                    if (rectA == rectB) break;
                    Rectangle.IsOverlap(rectA, rectB).Should().BeFalse();
                }
            }
        }
    }
}