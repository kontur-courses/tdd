using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace CircularCloudLayouterShould
{
    public class Tests
    {
        private CircularCloudLayouter.CircularCloudLayouter _layouter;
        private List<Rectangle> _rectangles;
        
        [SetUp]
        public void CreateLayouter()
        {
            _layouter = new CircularCloudLayouter.CircularCloudLayouter(new Point(0, 0));
        }

        [SetUp]
        public void CreateRectangles()
        {
            _rectangles = new List<Rectangle>
            {
                new Rectangle(new Point(0, 0), new Size(5, 5)),
                new Rectangle(new Point(7, -5), new Size(1, 3)),
                new Rectangle(new Point(-7, -5), new Size(5, 10))
            };
        }

        [TestCase(0, 1, TestName = "WhenZeroWidth")]
        [TestCase(1, 0, TestName = "WhenZeroHeight")]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            var act = new Action(() => _layouter.PutNextRectangle(size));

            act.Should().Throw<ArgumentException>();
        }
        
        [TestCase(1, 1, TestName = "WhenSimplePositiveSize")]
        public void PutNextRectangle_DoNotThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            var act = new Action(() => _layouter.PutNextRectangle(size));

            act.Should().NotThrow<ArgumentException>();
        }
        
        [TestCase(10, 5, TestName = "When rectangleWidth > height")]
        [TestCase(3, 7, TestName = "When rectangleWidth < height")]
        [TestCase(23, 23, TestName = "When rectangleWidth = height")]
        public void PutNextRectangle_LocationIsEquivalentToSpiralCenterPosition(int widthRectangle, int heightRectangle)
        {
            var size = new Size(widthRectangle, heightRectangle);
            
            _layouter.PutNextRectangle(size);

            _layouter.GetCurrentRectangle.Location.Should().Be(_layouter.Center);
        }
    }
}