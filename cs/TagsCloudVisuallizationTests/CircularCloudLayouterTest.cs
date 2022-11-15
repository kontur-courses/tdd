using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisuallizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTest
    {
        private CircularCloudLayouter _layouter;
        
        [SetUp]
        public void SetUp()
        {
            _layouter = new CircularCloudLayouter();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualisator = new RectangleVisualisator(_layouter);
                
                visualisator.Paint();
                visualisator.Save($"{TestContext.CurrentContext.Test.Name}.png");
            }
        }
        
        [TestCase(0, 0)]
        [TestCase(-1, -1)]
        public void PutNextRectangle_EmptySize_ThrowArgumentException(int sizeX, int sizeY)
        {
             Action action = () => _layouter.PutNextRectangle(new Size(sizeX, sizeY));
             action.Should().Throw<ArgumentException>().WithMessage("The size must not be equal to or less than 0");
        }

        [TestCase(1, 1)]
        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(50, 50)]
        [TestCase(25, 25)]
        public void PutNextRectangle_ShouldNotIntersects(int sizeX, int sizeY)
        {
            for (int i = 0; i < 10; i++)
                _layouter.PutNextRectangle(new Size(sizeX, sizeY));

            Rectangle exampleRectangle = _layouter.PutNextRectangle(new Size(sizeX, sizeY));
            var isIntersects = false;
            foreach (var rectangle in _layouter.Rectangles)
            {
                if(exampleRectangle == rectangle)
                    continue;
                if (rectangle.IntersectsWith(exampleRectangle))
                    isIntersects = true;
            }
            isIntersects.Should().BeFalse();
        }
    }
}