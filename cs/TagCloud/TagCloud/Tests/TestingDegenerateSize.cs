using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    internal class TestingDegenerateSize : OnFailDrawer
    {
        public static readonly Size SingleSize = new Size(1, 1);
        public static readonly Point CenterPoint = new Point(0, 0);
        
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void Should_ThrowException_WhenNextRectangleIsDegenerate(int width, int height)
        {
            Action a = () => cloudLayouter.PutNextRectangle(new Size(width, height));
            a.Should().Throw<ArgumentException>();
        }
    }

}