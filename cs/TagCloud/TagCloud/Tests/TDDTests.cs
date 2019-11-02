using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    // todo: задача 2
    // todo: задача 3
    public class OrientationTests
    {
        public static readonly Size SingleSize = new Size(1, 1);
        public static readonly Point CenterPoint = new Point(0, 0);
        private CircularCloudLayouter _cloudLayouter;
        private List<Rectangle> _rectangles;
        
        [SetUp]
        public void CreateInstance()
        {
            _cloudLayouter = new CircularCloudLayouter(CenterPoint);
            _rectangles = Enumerable
                .Range(0, 4)
                .Select(num => _cloudLayouter.PutNextRectangle(SingleSize))
                .ToList();
        }
        
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void Should_DontCreateRectangle_WhenItsDegenerate(int width, int height)
        {
            Action a = () => _cloudLayouter.PutNextRectangle(new Size(width, height));
            a.Should().Throw<ArgumentException>();
        }

        [Test, Category("Simple Behaviour")]
        public void Should_CreateCenterRectangle_WhenItFirstCreated()
        {
            _rectangles
                .First()
                .Should()
                .Be(new Rectangle(CenterPoint, SingleSize));
        }

        [Test, Category("Simple Behaviour")]
        public void Should_CreteSecondRectangle_LeftFromFirst()
        {
            _rectangles[0]
                .Should().BeOfType<Rectangle>().Which.Y
                .Should().Be(_rectangles[1].Y);
            _rectangles[0]
                .Should().BeOfType<Rectangle>().Which.X
                .Should().BeGreaterThan(_rectangles[1].X);
        }

        [Test, Category("Simple Behaviour")]
        public void Should_CreateThirdRectangle_AboveFromFirst()
        {
            _rectangles[0]
                .Should().BeOfType<Rectangle>().Which.Y
                .Should().BeGreaterThan(_rectangles[2].Y);
            _rectangles[0]
                .Should().BeOfType<Rectangle>().Which.X
                .Should().Be(_rectangles[2].X);
        }

    }

    public class TileTetrisTesting
    {
        private CircularCloudLayouter _cloudLayouter;
        private List<Rectangle> _tiledRectagnles;
        private const int ElementsAmount = 1000;

        [SetUp]
        public void SetUp()
        {
            _cloudLayouter = new CircularCloudLayouter(OrientationTests.CenterPoint);
            _tiledRectagnles = Enumerable
                .Range(0, ElementsAmount)
                .Select(number => _cloudLayouter.PutNextRectangle(OrientationTests.SingleSize))
                .ToList();
        }

        [Test, Category("Simple Behaviour")]
        public void Should_TileSpace_WithNonEquivalentRectangles()
        {
            _tiledRectagnles
                .Select(rectangle => rectangle.Location)
                .Distinct()
                .Count()
                .Should()
                .Be(ElementsAmount) ;
        }
     
        [Test, Category("Simple Behaviour")]
        public void Should_GenerateNonOverlappingRectangles_AtLeastOn1000Requests()
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
        public void Should_TileCoordinateCenterIn1Depth_AtLeastOn10Rectangles()
        {
            var setPoints = new HashSet<Point>
            {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(-1, -1),
                new Point(0, -1)
            };
            _tiledRectagnles
                .GetRange(0, 10)
                .Where(rectangle => setPoints.Contains(rectangle.Location))
                .Sum(rectangle => 1)
                .Should()
                .Be(4);
        }       
    }

    public class DenseTetrisTesting
    {
        private CircularCloudLayouter _cloudLayouter;
        
        [SetUp]
        public void CreateInstance()
        {
            _cloudLayouter = new CircularCloudLayouter(OrientationTests.CenterPoint);
        }
        
        [TestCaseSource(nameof(_sizesForXDenseTesting))]
        public void Should_DenseLocateRectanglesWithDifferentShape_ByXCoordinate(List<Size> sizes)
        {
            var rectangles = sizes.Select(size => _cloudLayouter.PutNextRectangle(size)).ToList();
            var width = Math.Abs(rectangles.Select(rect => rect.X).Min() - rectangles.Select(rect => rect.X).Max());
            width.Should().BeLessThan(rectangles.Select(rect => rect.Width).Sum());
        }
        
        [TestCaseSource(nameof(_sizesForYDenseTesting))]
        public void Should_DenseLocateRectanglesWithDifferentShape_ByYCoordinate(List<Size> sizes)
        {
            var rectangles = sizes.Select(size => _cloudLayouter.PutNextRectangle(size)).ToList();
            var width = Math.Abs(rectangles.Select(rect => rect.X).Min() - rectangles.Select(rect => rect.X).Max());
            width.Should().BeLessThan(rectangles.Select(rect => rect.Width).Sum());
        }

        private static IEnumerable<TestCaseData> _sizesForYDenseTesting = new List<TestCaseData>
        {
            new TestCaseData(new List<Size>
            {
                new Size(4, 2),
                new Size(3, 4),
                new Size(5, 1),
                new Size(2, 7)
            }).SetName("4 rectangle near (0, 0) (y)"),
            new TestCaseData(new List<Size>
            {
                new Size(3, 2),
                new Size(9, 2)
            }).SetName("2 rectangles along Y-axis")
        };

        private static IEnumerable<TestCaseData> _sizesForXDenseTesting = new List<TestCaseData>
        {
            new TestCaseData(new List<Size>
            {
                new Size(4, 2),
                new Size(3, 4),
                new Size(5, 1),
                new Size(2, 7)
            }).SetName("4 rectangle near (0, 0) (x)"),
            new TestCaseData(new List<Size>
            {
                new Size(4, 2),
                new Size(3, 4)
            }).SetName("2 rectangles along X-axis")
        };
    }

    public class CenterStarts
    {
        [TestCaseSource(nameof(_nonCoordinateCenter))]
        public void Should_CorrectCreateInstance_WithDifferentCenterPoints(Point center)
        {
            Action a = () => new CircularCloudLayouter(center);
            a.Should().NotThrow();
        }

        [TestCaseSource(nameof(_nonCoordinateCenter))]
        public void Should_LocateFirstRectangle_OnSpecifiedByCenterXCoord(Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(2, 2));
            (rect.X + rect.Width / 2)
                .Should()
                .Be(center.X);
        }
        
        
        [TestCaseSource(nameof(_nonCoordinateCenter))]
        public void Should_LocateFirstRectangle_OnSpecifiedByCenterYCoord(Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(2, 2));
            (rect.Y + rect.Height / 2)
                .Should()
                .Be(center.Y);
        }
        

        private static IEnumerable<TestCaseData> _nonCoordinateCenter = new List<TestCaseData>
        {
            new TestCaseData(new Point(-1, 0)).SetName("{m}: x: -1, y = 0"),
            new TestCaseData(new Point(-1, 1)).SetName("{m}: x: -1, y = 1"),
            new TestCaseData(new Point(-1, -1)).SetName("{m}: x: -1, y = -1"),
            new TestCaseData(new Point(0, 0)).SetName("{m}: x: 0, y = 0"),
            new TestCaseData(new Point(0, 1)).SetName("{m}: x: 0, y = 1"),
            new TestCaseData(new Point(0, -1)).SetName("{m}: x: 0, y = -1"),
            new TestCaseData(new Point(1, 0)).SetName("{m}: x: 1, y = 0"),
            new TestCaseData(new Point(1, 1)).SetName("{m}: x: 1, y = 1"),
            new TestCaseData(new Point(1, -1)).SetName("{m}: x: 1, y = -1"),
        };

    }
}