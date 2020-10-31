using System.Drawing;
using System.Linq;
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
                cloudLayouter.Rectangles[i].IntersectsWith(cloudLayouter.Rectangles[j]).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_RectanglesAreNotFarFromCenter_AfterAddition()
        {
            var center = new Point(50, 50);
            var cloudLayouter = new CircularCloudLayouter(center);
            var sizes = SizeCreator(5);
            foreach (var size in sizes) cloudLayouter.PutNextRectangle(size);
            var mostTopRect = cloudLayouter.Rectangles.OrderBy(rect => rect.Top).First();
            var mostDownRect = cloudLayouter.Rectangles.OrderByDescending(rect => rect.Bottom).First();
            var mostLeftRect = cloudLayouter.Rectangles.OrderBy(rect => rect.Left).First();
            var mostRightRect = cloudLayouter.Rectangles.OrderByDescending(rect => rect.Right).First();
            const int expectedRadius = 15;
            (center.Y - mostTopRect.Top).Should().BeLessThan(expectedRadius);
            (mostDownRect.Bottom - center.Y).Should().BeLessThan(expectedRadius);
            (center.X - mostLeftRect.Left).Should().BeLessThan(expectedRadius);
            (mostRightRect.X - center.X).Should().BeLessThan(expectedRadius);
        }

        private static Size[] SizeCreator(int amount)
        {
            var sizes = new Size[amount];
            for (var i = 0; i < amount; i++) sizes[i] = new Size(amount + 2 - i, amount - i);

            return sizes;
        }
    }
}