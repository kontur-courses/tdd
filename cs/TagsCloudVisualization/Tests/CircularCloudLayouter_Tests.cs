﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(new PointF());
        }

        [TestCase(0, 1, TestName = "Width is zero")]
        [TestCase(-1, 1, TestName = "Width is negative")]
        [TestCase(1, 0, TestName = "Height is zero")]
        [TestCase(1, -1, TestName = "Height is negative")]
        public void PutNextRectangle_ShouldThrow_IfIncorrectSize(int width, int height)
        {
            Action act = () => cloudLayouter.PutNextRectangle(new Size(width, height));

            act.Should().Throw<ArgumentException>()
                .WithMessage("Size parameters should be positive");
        }

        [Test]
        public void PutNextRectangle_ShouldPutWithoutIntersecting()
        {
            for (var i = 0; i < 20; i++)
                cloudLayouter.PutNextRectangle(new Size(50, 15));

            var cloud = cloudLayouter.GetCloud().ToArray();

            cloud
                .Where(r => cloudIsIntersectWith(r, cloud))
                .Should().BeEmpty();
        }

        private Boolean cloudIsIntersectWith(RectangleF r, IEnumerable<RectangleF> cloud)
        {
            foreach (var tag in cloud.Where(t => t != r))
                if (tag.IntersectsWith(r))
                    return true;
            return false;
        }

        [TestCase(0, TestName = "return empty cloud? if we dont put")]
        [TestCase(1, TestName = "Return one tag, if we put only one")]
        [TestCase(10, TestName = "Return all tags, if we put multiple")]
        public void GetCloud_ReturnAll_IfPutMultipleTags(int countOfTags)
        {
            for (var i = 0; i < countOfTags; i++)
                cloudLayouter.PutNextRectangle(new Size(1, 1));

            var tagCloud = cloudLayouter.GetCloud();

            tagCloud.Count().Should().Be(countOfTags);
        }

        [TestCase(0, 0, 2, 2, TestName = "Even width and height in zero center")]
        [TestCase(0, 0, 9, 9, TestName = "Odd width and height in zero center")]
        [TestCase(0, 1, 2, 5, TestName = "Different width and height")]
        [TestCase(3, 3, 1, 1, TestName = "Width and height greater than center coordinates")]
        [TestCase(3, 6, 9, 1, TestName = "Different parameters and different center coordinates")]
        [TestCase(-4, -3, 2, 6, TestName = "Negative center coordinates")]
        public void GetCloud_ReturnFirstTagInCenter_IfAddOneTag(int xCloudPosition, int yCloudPosition, int width,
            int height)
        {
            cloudLayouter = new CircularCloudLayouter(new Point(xCloudPosition, yCloudPosition));
            cloudLayouter.PutNextRectangle(new Size(width, height));

            var tag = cloudLayouter.GetCloud().First();

            var xCenter = (tag.Left + tag.Right) / 2;
            var yCenter = (tag.Top + tag.Bottom) / 2;
            xCenter.Should().Be(xCloudPosition);
            yCenter.Should().Be(yCloudPosition);
        }

        [TestCase(0, 0, 1, 1, 4, TestName = "Two squares in zero position")]
        [TestCase(5, -9, 2, 6, 7, TestName = "Many rectangles with different width and height")]
        public void GetCloud_СenterLiesWithinBoundariesOfTheCloud(int xCloudPosition,
            int yCloudPosition, int sizeX, int sizeY, int count)
        {
            var center = new Point(xCloudPosition, yCloudPosition);
            cloudLayouter = new CircularCloudLayouter(center);
            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(sizeX, sizeY));

            var bounds = cloudLayouter.GetBounds();

            bounds.Contains(center).Should().BeTrue();
        }
    }
}