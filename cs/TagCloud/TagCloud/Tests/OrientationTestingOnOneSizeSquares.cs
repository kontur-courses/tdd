using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    internal class OrientationTestingOnOneSizeSquares
    {
        public static readonly Size SingleSize = new Size(1, 1);
        public static readonly Point CenterPoint = new Point(0, 0);
        private CircularCloudLayouter _cloudLayouter;
        private List<Rectangle> _rectangles;
        
        [SetUp]
        public void PlaceInstance()
        {
            _cloudLayouter = new CircularCloudLayouter(CenterPoint);
            _rectangles = Enumerable
                .Range(0, 5)
                .Select(num => _cloudLayouter.PutNextRectangle(SingleSize))
                .ToList();
        }
        
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void Should_ThrowException_WhenPlacedRectangleIsDegenerate(int width, int height)
        {
            Action a = () => _cloudLayouter.PutNextRectangle(new Size(width, height));
            a.Should().Throw<ArgumentException>();
        }

        [Test, Category("Simple Behaviour")]
        public void Should_PlaceCenterRectangle_WhenItFirstPlaced()
        {
            _rectangles
                .First()
                .Should()
                .Be(new Rectangle(CenterPoint, SingleSize));
        }

        [Test, Category("Simple Behaviour")]
        public void Should_PlaceSecondRectangle_RightFromFirst()
        {
            _rectangles[0]
                .X
                .Should()
                .BeLessThan(_rectangles[1].X);
            _rectangles[0]
                .Y
                .Should()
                .Be(_rectangles[1].Y);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_PlaceThirdRectangle_UnderFirst()
        {
            _rectangles[0]
                .X
                .Should()
                .Be(_rectangles[2].X);
            _rectangles[0]
                .Y
                .Should()
                .BeLessThan(_rectangles[2].Y);
        }
        
        [Test, Category("Simple Behaviour")]
        public void Should_PlaceFourRectangle_LeftFromFirst()
        {
            _rectangles[0]
                .X
                .Should()
                .BeGreaterThan(_rectangles[3].X);
            _rectangles[0]
                .Y
                .Should()
                .Be(_rectangles[3].Y);
        }
        
        [Test, Category("Simple Behaviour")]
        public void Should_PlaceFifthRectangle_AboveFromFirst()
        {
            _rectangles[0]
                .X
                .Should()
                .Be(_rectangles[4].X);
            _rectangles[0]
                .Y
                .Should()
                .BeGreaterThan(_rectangles[4].Y);
        }

    }

}