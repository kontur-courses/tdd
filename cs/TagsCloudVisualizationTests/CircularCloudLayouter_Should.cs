using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void OnOneRectangle_ReturnsRectangleWithHisCenterAsLocation()
        {
            Point center = new Point(20, 10);
            Size size = new Size(50, 100);
            CircularCloudLayouter ccl = new(center, 0.1, 1, 0);
            ccl.PutNextRectangle(size).Should().Be(new Rectangle(new Point(-5, -40), size));
        }
        
        [TestCase(-10, 20)]
        [TestCase(20, -10)]
        [TestCase(0, 0)]
        [TestCase(-10, -20)]
        public void OnNonPositiveWidthAndHeight_ThrowsArgumentException(int width, int height)
        {
            Point center = new Point(20, 10);
            Size size = new Size(width, height);
            CircularCloudLayouter ccl = new(center, 0.1, 10, 0);
            Action act = () => ccl.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>().WithMessage("Width and Height must be positive!");
        }

        [Test]
        public void OnFewRectangleSizes_ReturnsRectanglesWithRightLocation()
        {
            CircularCloudLayouter ccl = new(new Point(0, 0), 0.1, 10, 0);
            var actual = new List<Rectangle>();
            Size s1, s2, s3, s4;
            s1 = new Size(20, 30);
            s2 = new Size(20, 30);
            s3 = new Size(50, 20);
            s4 = new Size(10, 20);
            actual.Add(ccl.PutNextRectangle(s1));
            actual.Add(ccl.PutNextRectangle(s2));
            actual.Add(ccl.PutNextRectangle(s3));
            actual.Add(ccl.PutNextRectangle(s4));

            var expected = new List<Rectangle>();
            expected.Add(new Rectangle(new Point(-10, -15), s1));
            expected.Add(new Rectangle(new Point(-30, 0), s2));
            expected.Add(new Rectangle(new Point(-53, -37), s3));
            expected.Add(new Rectangle(new Point(-6, -57), s4));
            
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNoPlaceForRectangle_ThrowsException()
        {
            CircularCloudLayouter ccl = new(new Point(0, 0), 0.1, 10, 0);
            ccl.PutNextRectangle(new Size(1_000_000_000, 1_000_000_000));
            Action act = () => ccl.PutNextRectangle(new Size(100, 100));
            act.Should().Throw<ArgumentException>();
        }
    }
}

