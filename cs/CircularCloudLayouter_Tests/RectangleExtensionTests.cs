using System.Collections.Generic;
using System.Drawing;
using CloudLayouter;
using FluentAssertions;
using NUnit.Framework;

namespace CircularCloudLayouter_Tests
{
    [TestFixture]
    public class RectangleExtensionTests
    {
        private List<Rectangle> rectangles = new();

        [SetUp]
        public void SetUp()
        {
            AddRectangle(0, 5, 10, 10);
            AddRectangle(11, 16, 5, 10);
            AddRectangle(17, 16, 10, 5);
        }

        private void AddRectangle(int x, int y, int width, int height)
        {
            rectangles.Add(new Rectangle(x, y, width, height));
        }
        
        [Test]
        public void IntersectsWith_IntersectingRectangles_returnsTrue()
        {
            new Rectangle(5, 5, 10, 10).IntersectsWith(rectangles).Should().BeTrue();
        }

        [Test]
        public void IntersectsWith_NotIntersectingRectangles_returnsFalse()
        {
            new Rectangle(17, 21, 5, 5).IntersectsWith(rectangles).Should().BeFalse();
        }
    }
}