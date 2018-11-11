using System;
using System.IO;
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

        private CircularCloudLayouter _layouter;

        [SetUp]
        public void SetUp()
        {
            _layouter = new CircularCloudLayouter(_layouterCenter);
        }

        [TearDown]
        public void TearDown()
        {
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, Path.GetTempFileName());
            CloudLayoutVisualizer.SaveAsPngImage(_layouter.GetLayout(), filePath);
            Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
        
        [Test]
        public void Should_PutFirstRectangleWithCorrectPos()
        {
            var rectSize = new Size(100, 10);
            var expectedRect = new Rectangle(_layouterCenter, rectSize);
            _layouter.PutNextRectangle(rectSize).Should().BeEquivalentTo(expectedRect);
        }
        
        [Test]
        public void Should_PlaceManyRectanglesWithoutOverlaps()
        {
            foreach (var rect in _testRectsSizes)
            {
                _layouter.PutNextRectangle(rect);
            }

            var layout = _layouter.GetLayout();
            
            foreach (var rectA in layout)
            {
                foreach (var rectB in layout)
                {
                    if (rectA == rectB) break;
                    rectA.OverlapsWith(rectB).Should().BeFalse();
                }
            }
        }
    }
}