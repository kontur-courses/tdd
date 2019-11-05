using System;
using System.Linq;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter _circularCloudLayouter;

        [SetUp]
        public void StartUp() => _circularCloudLayouter = new CircularCloudLayouter(new Point());

        [Test]
        public void PutNextRectangle_SavesPutRectangles()
        {
            const int expectedRectanglesCount = 5;
                
            for (var i = 0; i < expectedRectanglesCount; i++)
                _circularCloudLayouter.PutNextRectangle(new Size());
            var actualRectangles = _circularCloudLayouter.Rectangles;
            actualRectangles.Count.Should().Be(expectedRectanglesCount);
        }
        
        [TestCase(0, 0, 2, 2, -1, 1, TestName = "Center at (0, 0)")]
        [TestCase(1, 1, 2, 2, 0, 2, TestName = "Center at (1, 1)")]
        [TestCase(0, 0, 3, 5, -1, 2, TestName = "Odd width and height")]
        [TestCase(0, 0, 0, 0, 0, 0, TestName = "Zero size")]
        public void PutNextRectangle_PutsFirstRectangleInCenter(int xCenter, 
                                                                int yCenter,
                                                                int width,
                                                                int height,
                                                                int expectedX,
                                                                int expectedY)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(xCenter, yCenter));
            var rectangleSize = new Size(width, height);
            var expectedLocation = new Point(expectedX, expectedY);
            var actualRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
            actualRectangle.Location.Should().Be(expectedLocation);
        }

        [TestCase(-1, 0, TestName = "Negative width")]
        [TestCase(0, -1, TestName = "Negative height")]
        [TestCase(-1, -1, TestName = "Negative width and height")]
        public void PutNextRectangle_ThrowsExceptionOnNegativeSizeValues(int width, int height)
        {
            var firstRectangleSize = new Size(width, height);
            Action action = () => _circularCloudLayouter.PutNextRectangle(firstRectangleSize);
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, TestName = "Zero size")]
        [TestCase(2, 2, TestName = "Square")]
        [TestCase(5, 3, TestName = "Horizontal rectangle")]
        [TestCase(3, 5, TestName = "Vertical rectangle")]
        public void InitializeSpiral_InitSpiralWithCorrectData(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            const double angleStep = Math.PI / 4;
            var firstLayerRadius = rectangleSize.Width / 2 + 1;
            var initialDensity = Spiral.CalculateDensity(rectangleSize);
            var expectedSpiral = new Spiral(angleStep, firstLayerRadius, initialDensity, new Point());
            
            _circularCloudLayouter.PutNextRectangle(rectangleSize);
            var actualSpiral = _circularCloudLayouter._spiral;
            actualSpiral.Should().Be(expectedSpiral);
        }
        
        [TestCase(2, 2, TestName = "Squares")]
        [TestCase(5, 3, TestName = "Horizontal rectangles")]
        [TestCase(3, 5, TestName = "Vertical rectangles")]
        [TestCase(0, 0, TestName = "Zero size")]
        public void RectanglesShouldNotIntersect(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            for (var i = 0; i < 20; i++)
                _circularCloudLayouter.PutNextRectangle(rectangleSize);
            var expectedResult = new bool[190];

            var filledAreas = _circularCloudLayouter.Rectangles;
            var actualResult = filledAreas
                .SelectMany((area, i) => filledAreas
                    .Skip(i + 1)
                    .Select(area.IntersectsWith));
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}