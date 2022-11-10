using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using TagsCloudVisualization.TagCloud;


namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudTests
    {
        private static Point _center;
        private CircularCloudLayouter ccl;
        private Spiral spiral;

        [SetUp]
        public void SetUP()
        {
            _center = new(1920 / 2, 1080 / 2);
            ccl = new CircularCloudLayouter(_center);
            spiral = new Spiral(_center, 1, 2);
        }

        [Test]
        public void Spiral_GetNextPoint_OnCorrectInput()
        {
            spiral.GetNextPoint().Should().BeEquivalentTo(_center);
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(961, 541));
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(959, 543));
        }

        [Test]
        public void CircularCloudLayouter_HaveNoOneRectangles()
        {
            ccl.rectangles.Count.Should().Be(0);
        }

        [Test]
        public void CircularCloudLayouter_CheckRectangleSize()
        {
            var size = new Size(20, 70);
            ccl.PutNextRectangle(size).Size.Should().Be(size);
        }

        [Test]
        public void CircularCloudLayouter_HaveOneRectangle()
        {
            ccl.PutNextRectangle(new Size(20, 10));
            ccl.rectangles.Count.Should().Be(1);
        }

        [TestCase(1920 / 2, 1080 / 2, TestName = "X,Y is positive")]
        [TestCase(0, 0, TestName = "X,Y is zero")]
        [TestCase(-1920 / 2, -1080 / 2, TestName = "X,Y is negative")]
        public void CircularCloudLayouter_Constructor_ShouldNotThrow(int x, int y)
        {
            Action act = () => new CircularCloudLayouter(new Point(x, y));
            act.Should().NotThrow();
        }

        [TestCase(0, 0, TestName = "Size is zero")]
        [TestCase(-1920 / 2, -1080 / 2, TestName = "X,Y is negative")]
        public void CircularCloudLayouter_PutNextRectangle_ShouldThrow(int width, int height)
        {
            var size = new Size(width, height);
            Action act = () => ccl.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(null, TestName = "Size is null")]
        public void CircularCloudLayouter_PutNextRectangle_ShouldThrow(Size size)
        {
            Action act = () => ccl.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>();
        }
    }
}
