﻿using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        [Test]
        public void InitializeFieldsAfterInstanceCreation()
        {
            var center = new Point(10, 10);

            var layouter = new CircularCloudLayouter(center);

            layouter.Rectangles.Should().NotBeNull();
            layouter.CloudCenter.Should().Equals(center);
        }

        [TestCase(-1, 20, TestName = "width or height is negative")]
        [TestCase(10, 0, TestName = "width or height equals zero")]
        public void ThrowExceptionWhenRectangleSizeIsIncorrect(int width, int height)
        {
            var rectangleSize = new Size(-1, 20);
            var center = new Point(10, 10);
            var layouter = new CircularCloudLayouter(center);
            Action act = () => layouter.PutNextRectangle(rectangleSize);

            act.Should().Throw<ArgumentException>();
        }
    }
}
