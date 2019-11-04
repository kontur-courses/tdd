using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud.Tests
{
    internal class TileTestingAt100OneSideSquares
    {
        private CircularCloudLayouter _cloudLayouter;
        private List<Rectangle> _tiledRectagnles;
        private const int ElementsAmount = 100;

        [SetUp]
        public void SetUp()
        {
            _cloudLayouter = new CircularCloudLayouter(OrientationTestingOnOneSizeSquares.CenterPoint);
            _tiledRectagnles = Enumerable
                .Range(0, ElementsAmount)
                .Select(number => _cloudLayouter.PutNextRectangle(OrientationTestingOnOneSizeSquares.SingleSize))
                .ToList();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var fname = $"{TestContext.CurrentContext.Test.FullName}.png";
            OnFailDrawer.DrawOriginOrientedRectangles(
                new Size(1200, 1200),
                _cloudLayouter
                    .GetAllRectangles()
                    .Select(rect => new Rectangle(600 + rect.X * 45, 600 + rect.Y * 45, 40, 40)),
                fname);
            TestContext.WriteLine($"Tag cloud visualisation saved to file: '{fname}'");
        }

        [Test, Category("Fall test")]
        public void Should_Fall_AndCreateImgWith100OneSidedSquares()
        {
            Assert.Fail();
        }

        [Test, Category("Simple Behaviour")]
        public void Should_TileSpace_AndNotSkipRectangles()
        {
            _tiledRectagnles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(ElementsAmount);
        }
     
        [Test, Category("Simple Behaviour")]
        public void Should_GenerateNonOverlappingRectangles()
        {
            _tiledRectagnles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(ElementsAmount);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_GenerateRectangles_WithSpecifiedSize()
        {
            _tiledRectagnles
                .Select(rectangle => rectangle.Size)
                .Distinct()
                .Count()
                .Should()
                .Be(1);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_TileCoordinateCenterIn1Depth_WithNoMoreThen9Rectangles()
        {
            var setPoints = new HashSet<Point>();
            for (var x=-1; x<=1; ++x)
            for (var y = -1; y <= 1; ++y)
                setPoints.Add(new Point(x, y));
            _tiledRectagnles
                .GetRange(0, 10)
                .Where(rectangle => setPoints.Contains(rectangle.Location))
                .Sum(rectangle => 1)
                .Should()
                .Be(setPoints.Count);
        }       
    }
}