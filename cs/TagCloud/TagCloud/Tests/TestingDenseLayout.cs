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
            var width = Math.Abs(rectangles.Min(rect => rect.X) - rectangles.Max(rect => rect.X + rect.Width));
            width.Should().BeLessOrEqualTo(rectangles.Select(rect => rect.Width).Sum());
        }
        
        [TestCaseSource(nameof(sizesForYDenseTesting))]
        public void Should_DenselyPlaceRectanglesWithDifferentShape_ByYCoordinate(IEnumerable<Size> sizes)
        {
            var rectangles = cloudLayouter.PutNextRectangles(sizes).ToList();
            var height = Math.Abs(rectangles.Min(rect => rect.Y) - rectangles.Max(rect => rect.Y + rect.Height));
            height.Should().BeLessOrEqualTo(rectangles.Select(rect => rect.Height).Sum());
        }

        [TestCaseSource(nameof(oneHundredSizesForCircularTesting))]
        public void Should_DenselyPlaceRectanglesWithDifferentShape_InCircle(IEnumerable<Size> sizes)
        {
            var rectangles = cloudLayouter.PutNextRectangles(sizes).ToList();
            var outerXWidth = Math.Abs(rectangles.Min(rect => rect.X) - rectangles.Max(rect => rect.X + rect.Width));
            var outerYHeight = Math.Abs(rectangles.Min(rect => rect.Y) - rectangles.Max(rect => rect.Y + rect.Height));
            var minOuterCircle = Math.PI * Math.Pow(Math.Min(outerXWidth, outerYHeight) / 2, 2);
            var maxOuterCircle = Math.PI * Math.Pow(Math.Max(outerXWidth, outerYHeight) / 2, 2);
            var rectSpace = rectangles.Select(rect => rect.Width * rect.Height).Sum();
            Math.Abs(minOuterCircle - rectSpace)
                .Should()
                .BeLessThan(Math.Abs(maxOuterCircle - minOuterCircle));
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