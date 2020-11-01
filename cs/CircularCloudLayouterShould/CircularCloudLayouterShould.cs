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
        private Random _random = new Random();
        
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
        
        [TestCase(10, TestName = "10 rectangles when put 10 rectangles")]
        [TestCase(100, TestName = "100 rectangles when put 100 rectangles")]
        [TestCase(10, TestName = "300 rectangles when put 300 rectangles")]
        [TestCase(0, TestName = "Zero when don't put rectangles")]
        public void PutNextRectangle_ManyRectangles(int countRectangles)
        {
            for (var i = 0; i < countRectangles; i++)
                _layouter.PutNextRectangle(new Size(_random.Next(50, 70), _random.Next(20, 40)));

            _layouter.GetRectangles.Count.Should().Be(countRectangles);
        }
    }
}