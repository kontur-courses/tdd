using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter _circularCloudLayouter;

        [SetUp]
        public void StartUp()
        {
            _circularCloudLayouter = new CircularCloudLayouter(new Point());
        }

        [TestCase(0, 0, 2, 2, -1, 1, TestName = "Center at (0, 0)")]
        [TestCase(1, 1, 2, 2, 0, 2, TestName = "Center at (1, 1)")]
        [TestCase(0, 0, 3, 5, -1, 2, TestName = "Odd width and height")]
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

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void PutNextRectangle_ThrowsExceptionOnNegativeSizeValues(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            Action action = () => _circularCloudLayouter.PutNextRectangle(rectangleSize);
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(1, 1, 0, 0, TestName = "Square")]
        [TestCase(3, 5, -1, 2, TestName = "Simple rectangle")]
        public void CenterPosition_RoundsDownOddSizeValues(int width, int height, int expectedX, int expectedY)
        {
            var expectedPosition = new Point(expectedX, expectedY);
            var rectangleSize = new Size(width, height);

            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;
            var centerPosition = typeof(CircularCloudLayouter).GetMethod("CenterPosition", flags);
            var parameters = new object[] {new Point(0, 0), rectangleSize};
            var actualPosition = (Point) centerPosition.Invoke(null, parameters);
            
            actualPosition.Should().Be(expectedPosition);
        }

        [Test]
        public void RectanglesShouldNotIntersect()
        {
            var rectangleSize = new Size(5, 2);
            for (var i = 0; i < 5; i++)
                _circularCloudLayouter.PutNextRectangle(rectangleSize);
            var expectedResult = new bool[10];

            var filledAreas = _circularCloudLayouter.FilledAreas;
            var actualResult = filledAreas
                .SelectMany((area, i) => filledAreas
                    .Skip(i + 1)
                    .Select(area.IntersectsWith));
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}