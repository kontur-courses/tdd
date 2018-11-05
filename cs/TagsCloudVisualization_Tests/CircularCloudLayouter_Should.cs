using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloudLayouter;
        private Point center;
        private Size defaultSize;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0,0);
            cloudLayouter = new CircularCloudLayouter(center);
            defaultSize = new Size(200, 100);
        }

        [Test]
        public void SetValidCenterPoint_AfterCreation()
        {
            cloudLayouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void BeEmpty_AfterCreation()
        {
            cloudLayouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void PutNextRectangle_ThrowArgumentException_OnInvalidSize()
        {
            var size = new Size(-1, -1);
            cloudLayouter.Invoking(obj => obj.PutNextRectangle(size)).Should().Throw<ArgumentException>().WithMessage("*?positive");
        }

        [Test]
        public void PutNextRectangle_PutSingleRectangleInCenter_SingleRectangleCenterShifted()
        {
            var expectedRectangle = new Rectangle(new Point(100, 50), defaultSize);
            cloudLayouter.PutNextRectangle(defaultSize).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_PutOneRectangle_OneRectangleInList()
        {
            cloudLayouter.PutNextRectangle(defaultSize);
            cloudLayouter.Rectangles.Should().HaveCount(1);
        }

        [Test]
        public void PutNextRectangle_PutTwoRectangles_RectanglesDoNotIntersect()
        {
            var random = new Random();
            cloudLayouter.PutNextRectangle(new Size(random.Next(1, 200), random.Next(1, 200)));
            cloudLayouter.PutNextRectangle(defaultSize);
            cloudLayouter.Rectangles[0].IntersectsWith(cloudLayouter.Rectangles[1]).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_PutSeveralRectangles_RectanglesDoNotIntersect()
        {
            var testRectangles = new List<Rectangle>();
            var random = new Random();
            for (var i = 0; i < 500; i++)
            {
                var nextRectangle = cloudLayouter.PutNextRectangle(new Size(random.Next(1, 200), random.Next(1, 200)));
                testRectangles.Any(nextRectangle.IntersectsWith).Should().BeFalse();
                testRectangles.Add(nextRectangle);
            }
        }

        [Test]
        public void PutNextRectangle_PutsRectanglesTightEnough()
        {
            var maxRadius = 0;
            var totalCloudArea = 0;
            var random = new Random();
            for (var i = 0; i < 500; i++)
            {
                var nextRectangle = cloudLayouter.PutNextRectangle(new Size(random.Next(1, 200), random.Next(1, 200)));
                totalCloudArea += nextRectangle.Width * nextRectangle.Height;
            }

            var totalCircleCloudRadius = GetMostDistantPointRadiusFromCenter(cloudLayouter.Rectangles);
            var totalCircleCloudArea = Math.PI * Math.Pow(totalCircleCloudRadius, 2);
            (totalCloudArea / totalCircleCloudArea).Should().BeApproximately(0.2, 0.1);
        }

        public int GetMostDistantPointRadiusFromCenter(List<Rectangle> rectangles)
        {
            return rectangles
                .Select(rect => new Point(MaxAbs(rect.Left, rect.Right), MaxAbs(rect.Top, rect.Bottom)))
                .Select(point => (int)Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2))).Max();
        }

        public int MaxAbs(int val1, int val2)
        {
            return Math.Abs(val1) == Math.Max(Math.Abs(val1), Math.Abs(val2)) ? val1 : val2;
        }
    }
}
