using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud.Tests
{
    internal class TileTestingAt100OneSideSquares : OnFailDrawer
    {
        private CircularCloudLayouter cloudLayouter;
        private List<Rectangle> tiledRectangles;
        private const int ElementsAmount = 100;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(TestingDegenerateSize.CenterPoint);
            tiledRectangles = Enumerable
                .Range(0, ElementsAmount)
                .Select(number => cloudLayouter.PutNextRectangle(TestingDegenerateSize.SingleSize))
                .ToList();
        }

        [Test, Category("Simple Behaviour")]
        public void Should_TileSpace_AndNotSkipRectangles()
        {
            tiledRectangles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(ElementsAmount);
        }
     
        [Test, Category("Simple Behaviour")]
        public void Should_GenerateNonOverlappingRectangles()
        {
            tiledRectangles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(ElementsAmount);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_GenerateRectangles_WithSpecifiedSize()
        {
            tiledRectangles
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
            tiledRectangles
                .GetRange(0, 10)
                .Where(rectangle => setPoints.Contains(rectangle.Location))
                .Sum(rectangle => 1)
                .Should()
                .Be(setPoints.Count);
        }       
    }
}