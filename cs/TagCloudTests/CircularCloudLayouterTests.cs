using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests
{
    public class Tests
    {
        [Test]
        public void CircularCloudLayouter_IsFirstRectInCenter()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var resultRect = cloudLayouter.PutNextRectangle(new Size(5, 5));
            resultRect.Location.Should().Be(new Point(50, 50));
        }
    }
}