using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TagsCloudVisualization;

namespace TagClouVisualizationTest
{
    [TestFixture]
    class CircularCloudLayouterTest
    {
        private ICloudLayouter testLayouter;
        static IEnumerable<TestCaseData> GetTestCases()
        {
            //yield return new TestCaseData(new List<Size>()
            //{
            //    new Size(4, 1), new Size(1, 1),
            //}).SetName("TwoRectangles");
            //yield return new TestCaseData(new List<Size>()
            //{
            //    new Size(2, 5), new Size(1, 3), new Size(1, 7), new Size(3, 9),
            //    new Size(2, 6), new Size(1, 4), new Size(1, 2), new Size(1, 8),

            //}).SetName("VerticalRectangles");
            //yield return new TestCaseData(new List<Size>()
            //{
            //    new Size(2, 5), new Size(1, 3), new Size(1, 7), new Size(3, 9),
            //    new Size(2, 6), new Size(1, 4), new Size(1, 2), new Size(1, 8),

            //}).SetName("BigHorizontalRectangles");
            //yield return new TestCaseData(new List<Size>()
            //{
            //    new Size(100, 100), new Size(300, 150), new Size(590, 500), new Size(900, 900),

            //}).SetName("Square");
            yield return new TestCaseData(new List<Size>()
            {
                new Size(100, 20), new Size(150, 30), new Size(500, 20), new Size(300, 50),
                new Size(50, 15), new Size(200, 30), new Size(300, 25), new Size(270, 25),
                new Size(100, 30), new Size(70, 35), new Size(40, 18), new Size(300, 30),

            }).SetName("SimpleTest");

        }

        [SetUp]
        public void SetUp()
        {
            var center = new Point(0, 0);
            testLayouter = new CircularCloudLayouter(center);
        }

        [Test, TestCaseSource("GetTestCases")]
        public void PutNextRectangle_ShouldNotCrossRectangles(List<Size> sizes)
        {
            var rectangles = sizes.Select(size => testLayouter.PutNextRectangle(size)).ToList();
            var testName = TestContext.CurrentContext.Test.Name;
            if (AreIntersects(rectangles))
            {
                var image = TagCloudImage.GenerateTagCloudBitmap(rectangles.ToArray(),
                    testLayouter.GetSizeTagCloud());
                SaveImage(image, testName);
                Assert.Fail($"Rectangles are intersect in test {testName}");
            }
            var test = TagCloudImage.GenerateTagCloudBitmap(rectangles.ToArray(),
                testLayouter.GetSizeTagCloud());
            SaveImage(test, testName);
        }

        private void SaveImage(Bitmap bitmap, string fileName)
        {
            var fullFileName = @"C:\Users\Дария\Desktop\учеба\shpora\tagclaud\tdd\" + $" {fileName} test was failed.bmp";
            bitmap.Save(fullFileName, ImageFormat.Bmp);
        }

        private bool AreIntersects(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
            return false;
        }
    }
}
