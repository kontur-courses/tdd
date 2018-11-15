using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagCloudVisualizationTest
{
    [TestFixture]
    class CircularCloudLayouterTest
    {
        private CircularCloudLayouter testLayouter;
        private static Random random = new Random();

        static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(Enumerable.Range(1, 100).Select(i => new Size(20,20)).ToList()).SetName("LargeAmountSmallRectangles");
            yield return new TestCaseData(Enumerable.Range(10,50).Select(i => new Size(i*10, i*10)).ToList()).SetName("Squares");
            yield return new TestCaseData(Enumerable.Range(1,20).Select(i => new Size(random.Next(5,9)*100, random.Next(3) * 100)).ToList()).SetName("SimpleTest");
            yield return new TestCaseData(Enumerable.Range(1, 100).Select(i => new Size(random.Next(5,9) * 100, random.Next(3) * 100)).ToList()).SetName("LargeNumberOfRectangles");         
        }

        [SetUp]
        public void SetUp()
        {
            var center = new Point(0, 0);
            testLayouter = new CircularCloudLayouter(center);
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        [Category("NotIntersects")]
        public void PutNextRectangle_ShouldNotCrossRectangles(List<Size> sizes)
        {
            var rectangles = GetRectangles(sizes);
            if (ContainsIntersectedRectangles(rectangles))
                ActionThenTestFailed(rectangles.ToArray(), testLayouter.Size);
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        [Category("TagCloudHasCircleShape")]
        public void PutNextRectangle_ShouldCreateCircleShape(List<Size> sizes)
        {
            var rectangles = GetRectangles(sizes);
            var cloudSize = testLayouter.Size;
            var expectedArea = CalculateTotalExpectedArea(sizes);
            var actualArea = cloudSize.Width / 2 * cloudSize.Width / 2 * Math.PI;
            var delta = expectedArea * 0.2;            
            if((actualArea - expectedArea) > delta)
                ActionThenTestFailed(rectangles.ToArray(), cloudSize,
                    $"TagCloud shape isn't a circle. Delta = {delta}");
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        [Category("TagCloudIsDense")]
        public void PutNextRectangle_ShouldCreateDenseCircle(List<Size> sizes)
        {
            var delta = 0.5;
            var rectangles = GetRectangles(sizes);
            var cloudSize = testLayouter.Size;
            var counterEmptySpaces = 0;
            for (var i = -cloudSize.Width / 2 ; i < cloudSize.Width/2; i += 10)
                for (var j = -cloudSize.Height/2; j < cloudSize.Height/2; j +=10)
                    if (!AreIntersects(new Rectangle(i, j, 10, 10), rectangles))
                        counterEmptySpaces++;         
            var totalAmaunt = (cloudSize.Width / 10 * cloudSize.Height / 10);
            double densityСoefficient = (double)counterEmptySpaces / totalAmaunt;
            if (densityСoefficient > delta)
                ActionThenTestFailed(rectangles.ToArray(), cloudSize,
                    $"TagCloud isn't dense. Required density = {delta}, but was {densityСoefficient}");
        }

        private List<Rectangle> GetRectangles(List<Size> sizes)
        {
            return sizes.Select(size => testLayouter.PutNextRectangle(size)).ToList();
        }

        private void ActionThenTestFailed(Rectangle[] rectangles, Size cloudSize, string mes = "")
        {
            var bitmap = TagCloudImage.GenerateTagCloudBitmap(rectangles.ToArray(),
                cloudSize);
            var testName = TestContext.CurrentContext.Test.Name;
            var fullFileName = AppContext.BaseDirectory + $" {testName} test was failed.bmp";
            bitmap.Save(fullFileName, ImageFormat.Bmp);
            Assert.Fail(mes);
        }       

        private int CalculateTotalExpectedArea(List<Size> sizes)
        {
            var area = 0;
            foreach (var size in sizes)
                area += size.Height * size.Width;
            return area;
        }        

        private bool AreIntersects(Rectangle rect, List<Rectangle> otherRectangles)
        {
            return otherRectangles.Any(r => r.IntersectsWith(rect));
        }
       
        private static bool ContainsIntersectedRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
            return false;
        }
    }
}
