using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud.Tests
{
        internal class DenseTesting
    {
        private CircularCloudLayouter cloudLayouter;
        
        [SetUp]
        public void CreateInstance()
        {
            cloudLayouter = new CircularCloudLayouter(OrientationTestingOnOneSizeSquares.CenterPoint);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var fname = $"{TestContext.CurrentContext.Test.FullName}.png";
            OnFailDrawer.DrawOriginOrientedRectangles(
                new Size(1200, 1200),
                cloudLayouter
                    .GetAllRectangles()
                    .Select(rect => new Rectangle(600 + rect.X  , 600 + rect.Y , rect.Width, rect.Height)),
                fname);
            TestContext.WriteLine($"Tag cloud visualisation saved to file: '{fname}'");
        }

        [TestCaseSource(nameof(_sizesForXDenseTesting))]
        public void Should_DensePlaceRectanglesWithDifferentShape_ByXCoordinate(IEnumerable<Size> sizes)
        {
            var rectangles = sizes.Select(size => cloudLayouter.PutNextRectangle(size)).ToList();
            var width = Math.Abs(rectangles.Select(rect => rect.X).Min() - rectangles.Select(rect => rect.X).Max());
            width.Should().BeLessThan(rectangles.Select(rect => rect.Width).Sum());
        }
        
        [TestCaseSource(nameof(_sizesForYDenseTesting))]
        public void Should_DensePlaceRectanglesWithDifferentShape_ByYCoordinate(IEnumerable<Size> sizes)
        {
            var rectangles = sizes.Select(size => cloudLayouter.PutNextRectangle(size)).ToList();
            var height = Math.Abs(rectangles.Select(rect => rect.Y).Min() - rectangles.Select(rect => rect.Y).Max());
            height.Should().BeLessThan(rectangles.Select(rect => rect.Height).Sum());
        }

        [TestCaseSource(nameof(oneHundredSizesForCircularTesting))]
        public void Should_DensePlaceRectanglesWithDifferentShape_InCircle(IEnumerable<Size> sizes)
        {
            var rectangles = sizes.Select(size => cloudLayouter.PutNextRectangle(size)).ToList();
            var maxSide = rectangles
                .SelectMany(rect => new List<int> {rect.Width, rect.Height})
                .Max();
            var xWidth = rectangles.Select(rect => rect.X).Max() - rectangles.Select(rect => rect.X).Min();
            var yHeight = rectangles.Select(rect => rect.Y).Max() - rectangles.Select(rect => rect.Y).Min();
            
            Math.Abs(xWidth - maxSide).Should().BeLessOrEqualTo(yHeight, "X greatly less then Y");
            Math.Abs(yHeight - maxSide).Should().BeLessOrEqualTo(xWidth, "Y greatly less then X");
        }

        [TestCaseSource(nameof(oneHundredSizesForCircularTesting))]
        public void Should_Fall_AndCreateImgWith100Rectangles(IEnumerable<Size> sizes)
        {
            sizes.Select(size => cloudLayouter.PutNextRectangle(size)).ToList();
            Assert.Fail();
        }


        private static IEnumerable<TestCaseData> _sizesForYDenseTesting = new List<TestCaseData>
        {
            new TestCaseData(new List<Size>
            {
                new Size(4, 2),
                new Size(3, 4),
                new Size(5, 1),
                new Size(2, 7)
            }).SetName("{m}: 4 rectangle near (0, 0) (y)"),
            new TestCaseData(new List<Size>
            {
                new Size(3, 2),
                new Size(9, 2)
            }).SetName("{m}: 2 rectangles along Y-axis")
        };

        private static IEnumerable<TestCaseData> _sizesForXDenseTesting = new List<TestCaseData>
        {
            new TestCaseData(new List<Size>
            {
                new Size(4, 2),
                new Size(3, 4),
                new Size(5, 1),
                new Size(2, 7)
            }).SetName("{m}: 4 rectangle near (0, 0) (x)"),
            new TestCaseData(new List<Size>
            {
                new Size(4, 2),
                new Size(3, 4)
            }).SetName("{m}: 2 rectangles along X-axis")
        };
        
        private static Random circularRandomGenerator = new Random();
        private static IEnumerable<TestCaseData> oneHundredSizesForCircularTesting = new List<TestCaseData>
        {
            new TestCaseData(Enumerable
                .Range(0, 100)
                .Select(num => new Size(circularRandomGenerator.Next(1, 30), circularRandomGenerator.Next(1, 30)))
                .ToList()
            ).SetName("{m}: random generated 100 rectangles")
        };
    }
}