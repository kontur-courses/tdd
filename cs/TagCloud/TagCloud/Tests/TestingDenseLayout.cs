using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagCloud.Tests
{
    internal class TestingDenseLayout: OnFailDrawer
    {
        [TestCaseSource(nameof(sizesForXDenseTesting))]
        public void Should_DenselyPlaceRectanglesWithDifferentShape_ByXCoordinate(IEnumerable<Size> sizes)
        {
            var rectangles = cloudLayouter.PutNextRectangles(sizes).ToList();
            var width = Math.Abs(rectangles.Select(rect => rect.X).Min() - rectangles.Select(rect => rect.X).Max());
            width.Should().BeLessThan(rectangles.Select(rect => rect.Width).Sum());
        }
        
        [TestCaseSource(nameof(sizesForYDenseTesting))]
        public void Should_DenselyPlaceRectanglesWithDifferentShape_ByYCoordinate(IEnumerable<Size> sizes)
        {
            var rectangles = cloudLayouter.PutNextRectangles(sizes).ToList();
            var height = Math.Abs(rectangles.Select(rect => rect.Y).Min() - rectangles.Select(rect => rect.Y).Max());
            height.Should().BeLessThan(rectangles.Select(rect => rect.Height).Sum());
        }

        [TestCaseSource(nameof(oneHundredSizesForCircularTesting))]
        public void Should_DenselyPlaceRectanglesWithDifferentShape_InCircle(IEnumerable<Size> sizes)
        {
            var rectangles = cloudLayouter.PutNextRectangles(sizes).ToList();
            var maxSide = rectangles
                .SelectMany(rect => new List<int> {rect.Width, rect.Height})
                .Max();
            var xWidth = rectangles.Select(rect => rect.X).Max() - rectangles.Select(rect => rect.X).Min();
            var yHeight = rectangles.Select(rect => rect.Y).Max() - rectangles.Select(rect => rect.Y).Min();
            
            Math.Abs(xWidth - maxSide).Should().BeLessOrEqualTo(yHeight, "X greatly less then Y");
            Math.Abs(yHeight - maxSide).Should().BeLessOrEqualTo(xWidth, "Y greatly less then X");
        }

        private static IEnumerable<TestCaseData> sizesForYDenseTesting = new List<TestCaseData>
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

        private static IEnumerable<TestCaseData> sizesForXDenseTesting = new List<TestCaseData>
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