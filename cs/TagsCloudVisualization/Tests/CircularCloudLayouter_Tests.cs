﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter cloudLayouter;
        private string failedTestsData = "TestsImg";

        public CircularCloudLayouter_Tests()
        {
            var dir = failedTestsData;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(new Spiral(0.01f, 1, new PointF()));
        }

        [TearDown]
        public void CreateBitmapImageOnFail()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var visualizer = new Visualizer(cloudLayouter);
            var fileToSave = failedTestsData + "/" + TestContext.CurrentContext.Test.FullName + ".png";
            visualizer.DrawRectangles(fileToSave);
            var path = Path.GetFullPath(fileToSave);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
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
            {
                cloudLayouter.PutNextRectangle(new Size(50, 15));
            }

            var cloud = cloudLayouter.GetCloud();
            cloud
                .Where(r => CloudIntersectWith(r, cloud))
                .Should()
                .BeEmpty();
        }

        private Boolean CloudIntersectWith(RectangleF r, IEnumerable<RectangleF> cloud)
        {
            foreach (var tag in cloud.Where(t => t != r))
                if (tag.IntersectsWith(r))
                    return true;
            return false;
        }


        [TestCase(0, 0, 2, 2, TestName = "Even width and height in zero center")]
        [TestCase(0, 0, 9, 9, TestName = "Odd width and height in zero center")]
        [TestCase(0, 1, 2, 5, TestName = "Different width and height")]
        [TestCase(3, 3, 1, 1, TestName = "Width and height greater than center coordinates")]
        [TestCase(3, 6, 9, 1, TestName = "Different parameters and different center coordinates")]
        [TestCase(-4, -3, 2, 6, TestName = "Negative center coordinates")]
        public void PutNextRectangle_ReturnFirstTagInCenter_IfAddOneTag(int xCloudPosition, int yCloudPosition,
            int width,
            int height)
        {
            cloudLayouter = new CircularCloudLayouter(new Spiral(0.2f, 1, new Point(xCloudPosition, yCloudPosition)));

            var tag = cloudLayouter.PutNextRectangle(new Size(width, height));

            var xCenter = (tag.Left + tag.Right) / 2;
            var yCenter = (tag.Top + tag.Bottom) / 2;
            xCenter.Should().Be(xCloudPosition);
            yCenter.Should().Be(yCloudPosition);
        }

        [TestCase(1, 1, 100, 0.9, TestName = "Very tightly if small 100 1x1 squares")]
        [TestCase(2, 5, 50, 0.8, TestName = "Rectangles with different dimensions")]
        [TestCase(9, 9, 60, 0.8, TestName = "Big squares")]
        public void PutNextRectangle_ShouldPutEnoughTight(int width, int height, int count, double densityCoefficient)
        {
            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(width, height));

            var density = GetDensity(cloudLayouter);
            density.Should().BeGreaterThan((Math.PI / 4) * densityCoefficient).And.BeLessThan(Math.PI / 4);
        }

        [TestCase(1, 1, 5000, TestName = "1x1 5000 rectangles in 5 second")]
        [TestCase(2, 5, 5000, TestName = "The execution time should not depend on the size dimensions")]
        [TestCase(15, 15, 5000, TestName = "Same execution time for large sizes")]
        public void PutNextRectangle_ShouldWorkFast(int width, int height, int count)
        {
            cloudLayouter = new CircularCloudLayouter(new Spiral(0.1f, 0.65, new PointF()));
            var size = new Size(width, height);

            Action act = () =>
            {
                for (var i = 0; i < count; i++)
                    cloudLayouter.PutNextRectangle(size);
            };

            act.ExecutionTime().Should().BeLessThan(1000.Milliseconds());
        }

        private double GetDensity(CircularCloudLayouter cloudLayouter)
        {
            var union = cloudLayouter.Unioned;
            var unionRectsArea = union.Height * union.Width;
            var sumOfAreas = cloudLayouter.GetCloud().Sum(rectangle => rectangle.Height * rectangle.Width);
            return sumOfAreas / unionRectsArea;
        }
    }
}