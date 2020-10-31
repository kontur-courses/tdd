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
            var center = new Point(50, 50);
            var cloudLayouter = new CircularCloudLayouter(center);
            var resultRect = cloudLayouter.PutNextRectangle(new Size(5, 5));
            resultRect.Location.Should().Be(center);
        }
        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect_AfterAddition()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var sizes = SizeCreator(5);
            foreach (var size in sizes) cloudLayouter.PutNextRectangle(size);

            for (var i = 0; i < cloudLayouter.Rectangles.Count - 1; i++)
            for (var j = i + 1; j < cloudLayouter.Rectangles.Count; j++)
                Assert.False(cloudLayouter.Rectangles[i].IntersectsWith(cloudLayouter.Rectangles[j]),
                    $"First rect coords: {cloudLayouter.Rectangles[i]}\n" +
                    $"Second rect coords: {cloudLayouter.Rectangles[j]}");
        }

        private static Size[] SizeCreator(int amount)
        {
            var sizes = new Size[amount];
            for (var i = 0; i < amount; i++) sizes[i] = new Size(amount + 2 - i, amount - i);

            return sizes;
        }
    }
}