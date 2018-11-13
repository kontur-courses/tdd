using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public abstract class ICloudLayouter_Should
    {
        protected abstract ICloudLayouter CreateLayouterInstance(Point center);
        
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
        private ICloudLayouter _layouter;

        [SetUp]
        public void SetUp()
        {
            _layouter = CreateLayouterInstance(_layouterCenter);
        }

        [TearDown]
        public void TearDown()
        {
            SaveLayoutToImageIfCurrentTestFailed();
        }
        
        [Test]
        public void Should_PutFirstRectangleWithCorrectPos()
        {
            var rectSize = new Size(100, 10);
            var expectedRect = new Rectangle(new Point(50, 5), rectSize);
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
                    rectA.IntersectsWith(rectB).Should().BeFalse();
                }
            }
        }

        private void SaveLayoutToImageIfCurrentTestFailed()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (Equals(testResult, ResultState.Failure) || Equals(testResult == ResultState.Error))
            {
                var fileName = $"{TestContext.CurrentContext.Test.Name}_failed.png";
                var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, fileName);
                CloudLayoutVisualizer.SaveAsPngImage(_layouter.GetLayout(), filePath);
                Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
            }
        }
        
    }
}