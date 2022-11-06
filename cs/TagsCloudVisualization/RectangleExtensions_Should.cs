using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class RectangleExtensions_Should
    {
        [Test]
        public void AreIntersected_ShouldReturnTrue_WhenRectangleEqual()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(50, 50, 50, 50);
            rect1.AreIntersected(rect2).Should().BeTrue();
        }
        
        [Test]
        public void AreIntersected_ShouldReturnTrue_WhenRectangleInsideRectangle()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(60, 60, 30, 30);
            rect1.AreIntersected(rect2).Should().BeTrue();
        }

        [Test] 
        public void AreIntersected_ShouldReturnTrue_WhenRectanglesHaveCommonSideX()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(100, 50, 50, 50);
            rect1.AreIntersected(rect2).Should().BeTrue();
        }

        [Test]
        public void AreIntersected_ShouldReturnTrue_WhenRectanglesHaveCommonSideY()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(50, 100, 50, 50);
            rect1.AreIntersected(rect2).Should().BeTrue();
        }

        [Test]
        public void AreIntersected_ShouldReturnTrue_WhenRectanglesHaveCommonPoint()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(100, 100, 50, 50);
            rect1.AreIntersected(rect2).Should().BeTrue();
        }

        [Test]
        public void AreIntersected_ShouldReturnFalse_WhenRectangleLeftFromRectangle()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(101, 50, 50, 50);
            rect1.AreIntersected(rect2).Should().BeFalse();
        }

        [Test]
        public void AreIntersected_ShouldReturnFalse_WhenRectangleRightFromRectangle()
        {
            var rect1 = new Rectangle(151, 100, 50, 50);
            var rect2 = new Rectangle(100, 100, 50, 50);
            rect1.AreIntersected(rect2).Should().BeFalse();
        }

        [Test]
        public void AreIntersected_ShouldReturnFalse_WhenRectangleTopFromRectangle()
        {
            var rect1 = new Rectangle(50, 50, 50, 50);
            var rect2 = new Rectangle(50, 101, 50, 50);
            rect1.AreIntersected(rect2).Should().BeFalse();
        }

        [Test]
        public void AreIntersected_ShouldReturnFalse_WhenRectangleBottomFromRectangle()
        {
            var rect1 = new Rectangle(100, 151, 50, 50);
            var rect2 = new Rectangle(100, 100, 50, 50);
            rect1.AreIntersected(rect2).Should().BeFalse();
        }

        [Test]
        public void AreIntersected_ShouldReturnTrue_WhenRectangleHaveIntersectedRectangle()
        {
            var rect = new Rectangle(50, 50, 50, 50);
            var rectangles = new List<Rectangle>();
            rectangles.Add(new Rectangle(101, 50, 50, 50));
            rectangles.Add(new Rectangle(0, 50, 49, 50));
            rectangles.Add(new Rectangle(50, 101, 50, 50));
            rectangles.Add(new Rectangle(50, 0, 50, 49));
            rectangles.Add(new Rectangle(100, 100, 50, 50));
            rect.AreIntersected(rectangles).Should().BeTrue();
        }

        [Test]
        public void AreIntersected_ShouldReturnFalse_WhenRectangleNotIntersectedWithRectangles()
        {
            var rect = new Rectangle(50, 50, 50, 50);
            var rectangles = new List<Rectangle>();
            rectangles.Add(new Rectangle(101, 50, 50, 50));
            rectangles.Add(new Rectangle(0, 50, 49, 50));
            rectangles.Add(new Rectangle(50, 101, 50, 50));
            rectangles.Add(new Rectangle(50, 0, 50, 49));
            rect.AreIntersected(rectangles).Should().BeFalse();
        }

        [Test]
        public void AreIntersected_ShouldReturnTrue_WhenHaveIntersectedRectanglesInList()
        {
            var rectangles = new List<Rectangle>();
            rectangles.Add(new Rectangle(101, 50, 50, 50));
            rectangles.Add(new Rectangle(0, 50, 50, 50));
            rectangles.Add(new Rectangle(50, 101, 50, 50));
            rectangles.Add(new Rectangle(50, 0, 50, 49));
            rectangles.Add(new Rectangle(50, 50, 50, 50));
            rectangles.AreIntersected().Should().BeTrue();
        }

        [Test]
        public void AreIntersected_ShouldReturnFalse_WhenNotIntersectedRectanglesInList()
        {
            var rectangles = new List<Rectangle>();
            rectangles.Add(new Rectangle(101, 50, 50, 50));
            rectangles.Add(new Rectangle(0, 50, 49, 50));
            rectangles.Add(new Rectangle(50, 101, 50, 50));
            rectangles.Add(new Rectangle(50, 0, 50, 49));
            rectangles.AreIntersected().Should().BeFalse();
        }
    }
}
