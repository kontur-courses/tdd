using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using System.Linq;

namespace TagsCloudVisuallizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTest
    {
        private Point _center;
        private CircularCloudLayouter _layouter;
        private Random _random;
        private List<Rectangle> _rectangles;
        
        [SetUp]
        public void SetUp()
        {
            _center = new Point(200, 200);
            _layouter = new CircularCloudLayouter(_center);
            _random = new Random();
            _rectangles = new List<Rectangle>();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualisator = new RectangleVisualisator(_rectangles);
                
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
        
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void PutNextRectangle_ShouldNotIntersects(int amount)
        {
            var isIntersects = false;
            
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < amount; i++)
            {
                var size = new Size(_random.Next() % 255 + 1, _random.Next() % 255 + 1);
                var rectangle = _layouter.PutNextRectangle(size);
                if (rectangle.IsIntersects(rectangles))
                    isIntersects = true;
                rectangles.Add(rectangle);
            }
            isIntersects.Should().BeFalse();
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        public void PlacedRectangles_ShouldFillEllipseBy70Percent(int amount)
        {
            var center = new Point(200, 200);
            GenerateRectangles(amount);

            var yDiameter = _rectangles.Max(x => x.Bottom) - _rectangles.Min(x => x.Top);
            var xDiameter = _rectangles.Max(x => x.Right) - _rectangles.Min(x => x.Left);
            
            var filledArea = _rectangles.Sum(x => x.GetArea());

            var ellipseArea = Math.PI * yDiameter * xDiameter / 4;

            var filledPercent = Convert.ToInt32((filledArea / ellipseArea) * 100);
            
            //Чаще всего значения в районе 65 - 70
            (filledPercent).Should().BeGreaterThanOrEqualTo(70);
        }
        
        private void GenerateRectangles(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException();

            for (int i = 0; i < amount; i++)
            {
                var size = new Size(_random.Next() % 255 + 1, _random.Next() % 255 + 1);
                _rectangles.Add(_layouter.PutNextRectangle(size));
            }
        }
    }
}