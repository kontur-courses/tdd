using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests
{
    public class Tests
    {
        private Point center;
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        private void SetUp()
        {
            center = new Point(50, 50);
            cloudLayouter = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void CircularCloudLayouter_IsFirstRectInCenter()
        {
            var resultRect = cloudLayouter.PutNextRectangle(new Size(5, 5));
            
            resultRect.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect_AfterAddition()
        {
            var sizes = CreateSizesCollection(5);
            
            foreach (var size in sizes) cloudLayouter.PutNextRectangle(size);

            for (var i = 0; i < cloudLayouter.Rectangles.Count - 1; i++)
            for (var j = i + 1; j < cloudLayouter.Rectangles.Count; j++)
                cloudLayouter.Rectangles[i].IntersectsWith(cloudLayouter.Rectangles[j]).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_PutRectanglesInCircleForm_AfterAddition()
        {
            var sizes = CreateSizesCollection(5);
            foreach (var size in sizes) cloudLayouter.PutNextRectangle(size);
            var mostTopRect = cloudLayouter.Rectangles.OrderBy(rect => rect.Top).First();
            var mostDownRect = cloudLayouter.Rectangles.OrderByDescending(rect => rect.Bottom).First();
            var mostLeftRect = cloudLayouter.Rectangles.OrderBy(rect => rect.Left).First();
            var mostRightRect = cloudLayouter.Rectangles.OrderByDescending(rect => rect.Right).First();
            const int expectedRadius = 15;
            var dy = Math.Max(center.Y - mostTopRect.Top, mostDownRect.Bottom - center.Y);
            var dx = Math.Max(center.X - mostLeftRect.Left, mostRightRect.X - center.X);
            var difBetweenDeltas = Math.Abs(dx - dy);
            
            dy.Should().BeLessThan(expectedRadius);
            dx.Should().BeLessThan(expectedRadius);
            difBetweenDeltas.Should().BeLessThan(expectedRadius / 2);
        }

        private static IEnumerable<Size> CreateSizesCollection(int amount)
        {
            var sizes = new Size[amount];
            for (var i = 0; i < amount; i++) sizes[i] = new Size(amount + 2 - i, amount - i);

            return sizes;
        }
    }
}