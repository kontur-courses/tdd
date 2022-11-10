using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization.Tests
{
    public class CircularCloudLayouterTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TestCase(0,1, TestName = "zero width")]
        [TestCase(1, 0, TestName = "zero height")]
        public void PutNextRectangle_Should_Fail_On(int width,int height)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0,0));
            Size size = new Size(width, height);
            Action putNextRectangle = () => circularCloudLayouter.PutNextRectangle(size);

            putNextRectangle.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_Should_ReturnTwoNotIntersectsRectangles()
        {
            CircularCloudLayouter circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 1));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(10,1));

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }
    }
}