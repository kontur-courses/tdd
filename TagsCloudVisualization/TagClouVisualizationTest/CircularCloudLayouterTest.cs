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
        private ICloudLayouter testLayouter;
        static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new List<Size>
            {
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),
                new Size(10,10), new Size(10,10), new Size(10,10), new Size(10,10),

            }).SetName("LargeAmountSmollRectangles");
            yield return new TestCaseData(new List<Size>
            {
                new Size(650, 650), new Size(400, 400), new Size(250, 250), new Size(100, 100),
                new Size(50, 50), new Size(300, 300), new Size(200, 200), new Size(17, 17),

            }).SetName("Squares");
            yield return new TestCaseData(new List<Size>
            {
                new Size(400, 70), new Size(370, 65), new Size(330, 65), new Size(270, 40),
                new Size(250, 30), new Size(250, 25), new Size(230, 30), new Size(225, 25),
                new Size(225, 20), new Size(210, 20), new Size(190, 30), new Size(150, 20),
                new Size(160, 20), new Size(140, 20), new Size(135, 20), new Size(120, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),

            }).SetName("SimpleTest");
            yield return new TestCaseData(new List<Size>
            {
                new Size(250, 35), new Size(170, 27), new Size(160, 65), new Size(150, 23),
                new Size(130, 20), new Size(120, 20), new Size(110, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(190, 20), new Size(10, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 30), new Size(120, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 23), new Size(100, 20),
                new Size(130, 20), new Size(130, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(200, 30), new Size(180, 20),
                new Size(100, 30), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(170, 20), new Size(100, 20), new Size(100, 20),
                new Size(150, 20), new Size(100, 20), new Size(100, 30), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(60, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(140, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(200, 20), new Size(70, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(200, 30), new Size(100, 20), new Size(100, 20),
                new Size(160, 20), new Size(100, 20), new Size(100, 20), new Size(100, 30),
                new Size(100, 20), new Size(100, 20), new Size(120, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 30), new Size(100, 20),
                new Size(170, 30), new Size(100, 20), new Size(50, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 30), new Size(156, 23), new Size(100, 20),
                new Size(100, 40), new Size(100, 20), new Size(100, 20), new Size(100, 30),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(70, 10), new Size(100, 20), new Size(100, 20), new Size(100, 20),
                new Size(100, 20), new Size(100, 20), new Size(100, 20), new Size(100, 20),

            }).SetName("LargeNumberOfRectangles");
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
            var rectangles = sizes.Select(size => testLayouter.PutNextRectangle(size)).ToList();
            var testName = TestContext.CurrentContext.Test.Name;
            if (ContainsIntersectsRectangles(rectangles))
            {               
                SaveImage(testName, rectangles.ToArray(), testLayouter.GetSizeTagCloud());
                Assert.Fail($"Rectangles are intersect in test {testName}");
            }
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        [Category("TagCloudHasCircleShape")]
        public void PutNextRectangle_ShouldCreateCircleShape(List<Size> sizes)
        {
            
            var rectangles = sizes.Select(size => testLayouter.PutNextRectangle(size)).ToList();
            var cloudSize = testLayouter.GetSizeTagCloud();
            var expectedArea = CalculateTotalExpectedArea(sizes);
            var actualArea = cloudSize.Width / 2 * cloudSize.Width / 2 * Math.PI;
            var delta = expectedArea / 2;            
            if((actualArea - expectedArea) > delta)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                SaveImage(testName, rectangles.ToArray(), cloudSize);
                Assert.Fail($"TagCloud shape isn't a circle in test {testName}. Delta = {delta}");
            }                    
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        [Category("TagCloudIsDense")]
        public void PutNextRectangle_ShouldCreateDenseCircle(List<Size> sizes)
        {
            var delta = 0.5;
            var rectangles = sizes.Select(size => testLayouter.PutNextRectangle(size)).ToList();
            var cloudSize = testLayouter.GetSizeTagCloud();
            var counterEmptySpaces = 0;
            for (var i = -cloudSize.Width / 2 ; i < cloudSize.Width/2; i += 10)
                for (var j = -cloudSize.Height/2; j < cloudSize.Height/2; j +=10)
                    if (!AreIntersects(new Rectangle(i, j, 10, 10), rectangles))
                        counterEmptySpaces++;
            var testName = TestContext.CurrentContext.Test.Name;
            var totalAmaunt = (cloudSize.Width / 10 * cloudSize.Height / 10);
            double densityСoefficient = (double)counterEmptySpaces / totalAmaunt;
            if (densityСoefficient > delta)
            {
                SaveImage(testName, rectangles.ToArray(), cloudSize);
                Assert.Fail($"TagCloud isn't dense in test {testName}. Required density = {delta}, but was {densityСoefficient}");
            }
        }

        private static void SaveImage(string fileName, Rectangle[] rectangles, Size cloudSize)
        {
            var bitmap = TagCloudImage.GenerateTagCloudBitmap(rectangles.ToArray(),
                cloudSize);
            var fullFileName = AppContext.BaseDirectory + $" {fileName} test was failed.bmp";
            bitmap.Save(fullFileName, ImageFormat.Bmp);
        }

        private static int CalculateTotalExpectedArea(List<Size> sizes)
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

        private static bool ContainsIntersectsRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
            return false;
        }
    }
}
