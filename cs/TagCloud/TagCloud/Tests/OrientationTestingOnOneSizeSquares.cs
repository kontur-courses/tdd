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
        private CircularCloudLayouter cloudLayouter;
        private List<Rectangle> rectangles;
        
        [SetUp]
        public void PlaceInstance()
        {
            cloudLayouter = new CircularCloudLayouter(CenterPoint);
            rectangles = Enumerable
                .Range(0, 5)
                .Select(num => cloudLayouter.PutNextRectangle(SingleSize))
                .ToList();
        }
        
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void Should_ThrowException_WhenPlacedRectangleIsDegenerate(int width, int height)
        {
            Action a = () => cloudLayouter.PutNextRectangle(new Size(width, height));
            a.Should().Throw<ArgumentException>();
        }

        [Test, Category("Simple Behaviour")]
        public void Should_PlaceCenterRectangle_WhenItFirstPlaced()
        {
            rectangles
                .First()
                .Should()
                .Be(new Rectangle(CenterPoint, SingleSize));
        }

        [Test, Category("Simple Behaviour")]
        public void Should_PlaceSecondRectangle_RightFromFirst()
        {
            rectangles[0]
                .X
                .Should()
                .BeLessThan(rectangles[1].X);
            rectangles[0]
                .Y
                .Should()
                .Be(rectangles[1].Y);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_PlaceThirdRectangle_UnderFirst()
        {
            rectangles[0]
                .X
                .Should()
                .Be(rectangles[2].X);
            rectangles[0]
                .Y
                .Should()
                .BeLessThan(rectangles[2].Y);
        }
        
        [Test, Category("Simple Behaviour")]
        public void Should_PlaceFourRectangle_LeftFromFirst()
        {
            rectangles[0]
                .X
                .Should()
                .BeGreaterThan(rectangles[3].X);
            rectangles[0]
                .Y
                .Should()
                .Be(rectangles[3].Y);
        }
        
        [Test, Category("Simple Behaviour")]
        public void Should_PlaceFifthRectangle_AboveFromFirst()
        {
            rectangles[0]
                .X
                .Should()
                .Be(rectangles[4].X);
            rectangles[0]
                .Y
                .Should()
                .BeGreaterThan(rectangles[4].Y);
        }

    }

}