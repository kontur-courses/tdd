using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    // todo: Выделить в группы и сделать ААА
    // todo: задача 2 и задача 3
    public class TetrisTests
    {
        private static readonly Size SingleSize = new Size(1, 1);
        private static readonly Point CenterPoint = new Point(0, 0);
        private CircularCloudLayouter _cloudLayouter;
        
        [SetUp]
        public void CreateInstance()
        {
            _cloudLayouter = new CircularCloudLayouter(CenterPoint);
        }
        
        [Test] 
        public void Should_DontCreateRectangle_WhenItsDegenerate()
        {
            Action a = () => _cloudLayouter.PutNextRectangle(new Size(0, 0));
            a.Should().Throw<ArgumentException>(
                "Should throws exceptions when next rectangle is degenerate");
        }

        [Test, Category("Simple Behaviour")]
        public void Should_CreateCenterRectangle_WhenItFirstCreated()
        {
            var rect = _cloudLayouter.PutNextRectangle(SingleSize);
            rect.Should().Be(new Rectangle(CenterPoint, SingleSize));
        }

        [Test, Category("Simple Behaviour")]
        public void Should_CreteSecondRectangle_LeftFromFirst()
        {
            var rect1 = _cloudLayouter.PutNextRectangle(SingleSize);
            var rect2 = _cloudLayouter.PutNextRectangle(SingleSize);
            rect1
                .Should().BeOfType<Rectangle>().Which.Y
                .Should().Be(rect2.Y);
            rect1
                .Should().BeOfType<Rectangle>().Which.X
                .Should().BeGreaterThan(rect2.X);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_CreateThirdRectangle_AboveFromFirst()
        {
            var rect1 = _cloudLayouter.PutNextRectangle(SingleSize);
            var rect2 = _cloudLayouter.PutNextRectangle(SingleSize);
            var rect3 = _cloudLayouter.PutNextRectangle(SingleSize);
            rect1
                .Should().BeOfType<Rectangle>().Which.Y
                .Should().BeGreaterThan(rect3.Y);
            rect1
                .Should().BeOfType<Rectangle>().Which.X
                .Should().Be(rect3.X);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_TileSpace_WithNonEquivalentRectangles()
        {
            const int elementsAmount = 100;
            var rectangles = from number in Enumerable.Range(0, elementsAmount) select _cloudLayouter.PutNextRectangle(SingleSize);
            rectangles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(elementsAmount) ;
        }

        [Test, Category("Simple Behaviour")]
        public void Should_GenerateNonOverlappingRectangles_AtLeastOn1000Requests()
        {
            const int elementAmount = 1000;
            var rectangles = from number in Enumerable.Range(0, elementAmount)
                select _cloudLayouter.PutNextRectangle(SingleSize);
            rectangles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(elementAmount);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_GenerateRectangles_WithSpecifiedSize()
        {
            const int elementAmount = 1000;
            var rectangles = from number in Enumerable.Range(0, elementAmount)
                select _cloudLayouter.PutNextRectangle(SingleSize);
            rectangles
                .Select(rectangle => rectangle.Size)
                .Distinct()
                .Count()
                .Should()
                .Be(1);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_TileCoordinateCenterIn1Depth_AtLeastOn10Rectangles()
        {
            var rectangles = from number in Enumerable.Range(0, 10) 
                select _cloudLayouter.PutNextRectangle(SingleSize);
            var setPoints = new HashSet<Point>
            {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(-1, -1),
                new Point(0, -1)
            };
            rectangles
                .Where(rectangle => setPoints.Contains(rectangle.Location))
                .Sum(rectangle => 1)
                .Should()
                .Be(4);
        }

    }

    public class CenterStarts
    {
        [TestCaseSource(nameof(_nonCoordinateCenter))]
        public void Should_CorrectCreateInstance_WithDifferentCenterPoints(Point center)
        {
            Action a = () => new CircularCloudLayouter(center);
            a.Should().NotThrow();
        }

        private static IEnumerable<TestCaseData> _nonCoordinateCenter = new List<TestCaseData>
        {
            new TestCaseData(new Point(-1, 0)).SetName("x: -1, y = 0"),
            new TestCaseData(new Point(-1, 1)).SetName("x: -1, y = 1"),
            new TestCaseData(new Point(-1, -1)).SetName("x: -1, y = -1"),
            new TestCaseData(new Point(0, 0)).SetName("x: 0, y = 0"),
            new TestCaseData(new Point(0, 1)).SetName("x: 0, y = 1"),
            new TestCaseData(new Point(0, -1)).SetName("x: 0, y = -1"),
            new TestCaseData(new Point(1, 0)).SetName("x: 1, y = 0"),
            new TestCaseData(new Point(1, 1)).SetName("x: 1, y = 1"),
            new TestCaseData(new Point(1, -1)).SetName("x: 1, y = -1"),
        };
    }
}