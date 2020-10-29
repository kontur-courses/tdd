using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point CentrePoint { get; set; }
        private CircularCloudLayouter CloudLayouter { get; set; }

        [SetUp]
        public void SetUp()
        {
            CentrePoint = new Point(0, 0);
            CloudLayouter = new CircularCloudLayouter(CentrePoint);
        }

        [TestCase(0, 9, TestName = "Width is zero")]
        [TestCase(-1, 8, TestName = "Width is negative")]
        [TestCase(10, 0, TestName = "Height is zero")]
        [TestCase(3, -2, TestName = "Height is negative")]
        public void PutNextRectangle_ThrowExeption_When(int width, int height)
        {
            var size = new Size(width, height);
            Action putRectangle = () => CloudLayouter.PutNextRectangle(size);

            putRectangle.Should().Throw<ArgumentException>();
        }
    }
}