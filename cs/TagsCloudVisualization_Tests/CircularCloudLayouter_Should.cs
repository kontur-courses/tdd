using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;
using System.Collections.Immutable;

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

        [TearDown]
        public void TearDown()
        {
            var testContext = TestContext.CurrentContext;
            var visualizer = new CircularCloudVisualizer();
            var filename = $"{testContext.WorkDirectory}/{testContext.Test.Name}.png";
            if (testContext.Result.FailCount != 0)
            {
                var image = visualizer.DrawRectangles(cloudLayouter.Rectangles.ToList(), cloudLayouter.Radius);
                image.Save(filename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }

        [Test]
        public void SetValidCenterPoint_AfterCreation() => cloudLayouter.Center.Should().BeEquivalentTo(center);
        
        [Test]
        public void BeEmpty_AfterCreation() => cloudLayouter.Rectangles.Should().BeEmpty();

        [Test]
        public void PutNextRectangle_ThrowArgumentException_OnInvalidSize()
        {
            var size = new Size(-1, -1);
            cloudLayouter.Invoking(obj => obj.PutNextRectangle(size)).Should().Throw<ArgumentException>().WithMessage("*?positive");
        }

        [Test]
        public void PutNextRectangle_PutSingleRectangleInCenter_SingleRectangleCenterShifted()
        {
            var expectedRectangle = new Rectangle(new Point(-100, -50), defaultSize);
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
        public void GetRadius_OnSingleRectangle_ReturnCorrectRadius()
        {
            var rectangle = cloudLayouter.PutNextRectangle(defaultSize);
            var expectedRadius = new Point(MathHelper.MaxAbs(rectangle.Left, rectangle.Right),
                MathHelper.MaxAbs(rectangle.Top, rectangle.Bottom)).GetDistanceTo(center);
            cloudLayouter.Radius.Should().Be(expectedRadius);
        }
        [Test]
        public void GetRadius_OnTwoRectangles_ReturnCorrectRadius()
        {
            cloudLayouter.PutNextRectangle(defaultSize);
            var rectangle = cloudLayouter.PutNextRectangle(defaultSize);
            var expectedRadius = new Point(MathHelper.MaxAbs(rectangle.Left, rectangle.Right),
                MathHelper.MaxAbs(rectangle.Top, rectangle.Bottom)).GetDistanceTo(center);
            cloudLayouter.Radius.Should().Be(expectedRadius);
        }

        [Test]
        public void GetRadius_OnSeveralRectangles_ReturnCorrectRadius()
        {
            for (var i=0; i < 999; i++)
                cloudLayouter.PutNextRectangle(defaultSize);
            var rectangle = cloudLayouter.PutNextRectangle(defaultSize);
            var expectedRadius = new Point(MathHelper.MaxAbs(rectangle.Left, rectangle.Right), MathHelper.MaxAbs(rectangle.Top, rectangle.Bottom))
                .GetDistanceTo(center);
            cloudLayouter.Radius.Should().Be(expectedRadius);
        }

        [Test]
        public void PutNextRectangle_PutsRectanglesTightEnough()
        {
            var totalCloudArea = 0;
            var random = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var nextRectangle = cloudLayouter.PutNextRectangle(new Size(random.Next(1, 200), random.Next(1, 200)));
                totalCloudArea += nextRectangle.Width * nextRectangle.Height;
            }

            var totalCircleCloudRadius = cloudLayouter.Radius;
            var totalCircleCloudArea = Math.PI * Math.Pow(totalCircleCloudRadius, 2);
            totalCircleCloudArea.Should().BeGreaterOrEqualTo(totalCloudArea);
        }
    }
}
