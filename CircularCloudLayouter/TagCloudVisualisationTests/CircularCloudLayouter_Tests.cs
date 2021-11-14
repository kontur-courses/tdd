using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Drawing;
using System.IO;

namespace TagCloudVisualisation
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter CCL;

        [SetUp]
        public void SetUpCCL()
        {
            CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(500, 500)));
        }

        [TestCase(new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { 4, 3, 3, 4 })]
        [TestCase(new int[] { 10, 20, 1, 1 })]
        [TestCase(new int[] { 1, 1, 2, 50 })]
        [TestCase(new int[] { 64, 23, 42, 12 })]
        public void PutNewRectangle_WithTwoRectangles_MustNotIntersect(int[] sizes)
        {
            var rectangle1 = CCL.PutNewRectangle(new Size(sizes[0], sizes[1]));
            var rectangle2 = CCL.PutNewRectangle(new Size(sizes[2], sizes[3]));

            rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
        }

        [TestCase(new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { 4, 3, 3, 4 })]
        [TestCase(new int[] { 10, 20, 1, 1 })]
        [TestCase(new int[] { 1, 1, 2, 50 })]
        [TestCase(new int[] { 64, 23, 42, 12 })]
        public void PutNewRectangle_WithTwoRectangles_MustLocatedTightly(int[] sizes)
        {
            var rectangle1 = CCL.PutNewRectangle(new Size(sizes[0], sizes[1]));
            var rectangle2 = CCL.PutNewRectangle(new Size(sizes[2], sizes[3]));

            var distance = rectangle1.Location.GetDistanceTo(rectangle2.Location);
            var maxDistance = GetMaximalTightDistanceBetweenRectangles(rectangle1, rectangle2);

            distance.Should().BeLessThanOrEqualTo(maxDistance);
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(new int[] { 4, 3, 3, 4, 1, 1 })]
        [TestCase(new int[] { 10, 20, 1, 1, 20, 12 })]
        [TestCase(new int[] { 1, 1, 2, 50, 33, 43 })]
        [TestCase(new int[] { 64, 23, 42, 12, 42, 42 })]
        public void PutNewRectangle_WithThreeRectangles_MustLocatedTightly(int[] sizes)
        {
            var rectangle1 = CCL.PutNewRectangle(new Size(sizes[0], sizes[1]));
            var rectangle2 = CCL.PutNewRectangle(new Size(sizes[2], sizes[3]));
            var rectangle3 = CCL.PutNewRectangle(new Size(sizes[4], sizes[5]));

            var distance12 = rectangle1.Location.GetDistanceTo(rectangle2.Location);
            var distance13 = rectangle1.Location.GetDistanceTo(rectangle3.Location);
            var maxDistance12 = GetMaximalTightDistanceBetweenRectangles(rectangle1, rectangle2);
            var maxDistance13 = GetMaximalTightDistanceBetweenRectangles(rectangle1, rectangle3);

            distance12.Should().BeLessThanOrEqualTo(maxDistance12);
            distance13.Should().BeLessThanOrEqualTo(maxDistance13);
        }

        [TestCase(-1, -1)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void PutNewRectangle_WithNonPositiveSize_ShouldThrow(int x, int y)
        {
            Action t = () => CCL.PutNewRectangle(new Size(x, y));
            t.Should().Throw<ArgumentException>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.FailCount> 0)
            {
                var myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Directory.CreateDirectory("TagCloudVisualizer");
                var path = "\\TagCloudVisualizer\\";
                var fullPath = myDocsPath + path + TestContext.CurrentContext.Test.FullName + ".bmp";
                BitmapSaver.SaveRectangleRainbowBitmap(CCL.GetRectangles(), fullPath);
            }
        }

        private static double GetMaximalTightDistanceBetweenRectangles(Rectangle rect1, Rectangle rect2)
        {
            return rect1.GetDiagonalLength() + rect2.GetDiagonalLength();
        }
    }
}
