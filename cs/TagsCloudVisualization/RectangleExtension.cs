using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static bool IntersectsWithAny(this Rectangle rect, List<Rectangle> rectangles)
        {
            foreach (var shape in rectangles)
                if (rect.IntersectsWith(shape))
                    return true;
            return false;
        }
    }

    [TestFixture]
    public class RectangleExtension_Should
    {
        Rectangle mainRect;
        private List<Rectangle> rects;

        [SetUp]
        public void SetUp()
        {
            mainRect = new Rectangle(0, 0, 50, 50);
            rects = new List<Rectangle>();
        }

        [Test]
        public void ShouldNotIntersect_WhenEmptyList()
        {
            mainRect.IntersectsWithAny(rects).Should().BeFalse();
        }

        [Test]
        public void ShouldIntersects_WhenOneIntersectRectangleInList()
        {
            rects.Add(new Rectangle(0, 0, 100, 100));

            mainRect.IntersectsWithAny(rects).Should().BeTrue();
        }

        [Test]
        public void ShouldIntersects_WhenManyIntersectRectangles()
        {
            for (int i = 0; i < 100; i++)
                rects.Add(new Rectangle(0, 0, i, i * 2));

            mainRect.IntersectsWithAny(rects).Should().BeTrue();
        }

        [Test]
        public void ShouldNotIntersect_WhenNoIntersectRectangles()
        {
            for (int i = 60; i < 160; i++)
                rects.Add(new Rectangle(60, 60, i, i * 2));

            mainRect.IntersectsWithAny(rects).Should().BeFalse();
        }
    }
}
