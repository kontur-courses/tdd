using System;
using TagCloudVisualization;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TagCloudVisualizationTests
{
    public class CloudLayouterShould
    {
        private CloudLayouter cloud;
        private static readonly Size ScreenSize = Screen.PrimaryScreen.Bounds.Size;
        private static readonly Point Center = new Point(ScreenSize.Width / 2, ScreenSize.Height / 2);
        private static readonly Spiral Spiral = new Spiral(1, Center);

        [SetUp]
        public void SetUp()
        {
            cloud = new CloudLayouter(Spiral);
        }

        public static bool AreThereAnyIntersections(Rectangle[] rectangles)
        {
            for (var i = 0; i < rectangles.Length; i++)
            {
                for (var j = i + 1; j < rectangles.Length; j++)
                {
                    if (rectangles[i].AreIntersected(rectangles[j]))
                        return true;
                }
            }

            return false;
        }

        [Test]
        public void FirstRectangleInCenter()
        {
            var sizeRectangle = new Size(110, 110);
            var r = cloud.PutNextRectangle(sizeRectangle);
            var expected = new Point(Center.X - sizeRectangle.Width / 2,
                Center.Y - sizeRectangle.Height / 2);

            r.Location
                .Should()
                .Be(expected);
        }

        [Test]
        public void TwoLocationRectanglesAreNotEqual()
        {
            var sizeRectangle = new Size(1000, 1000);
            var r1 = cloud.PutNextRectangle(sizeRectangle);
            var r2 = cloud.PutNextRectangle(sizeRectangle);

            r1.Should().NotBe(r2);
        }

        [TestCase(10, 10, 10, 10)]
        [TestCase(20, 10, 10, 20)]
        [TestCase(100, 100, 200, 200)]
        [TestCase(1, 100, 100, 1)]
        [TestCase(100, 50, 50, 100)]
        public void TwoRectanglesAreNotIntersected(
            int firstWidth, int firstHeight,
            int secondWidth, int secondHeight)
        {
            var r1 = cloud.PutNextRectangle(new Size(firstWidth, firstHeight));
            var r2 = cloud.PutNextRectangle(new Size(secondWidth, secondHeight));

            r1.AreIntersected(r2).Should().BeFalse();
        }

        [TestCase(10, 10, 25)]
        [TestCase(20, 10, 25)]
        [TestCase(10, 20, 25)]
        [TestCase(100, 100, 205)]
        [TestCase(1, 100, 3)]
        [TestCase(100, 1, 3)]
        [TestCase(100, 50, 150)]
        [TestCase(50, 100, 150)]
        public void TwoRectanglesClose(int width, int height, double expectedDistance)
        {
            var sizeRectangle = new Size(width, height);
            var r1 = cloud.PutNextRectangle(sizeRectangle);
            var r2 = cloud.PutNextRectangle(sizeRectangle);

            var distance = Math.Sqrt(Math.Pow(r1.Location.X - r2.Location.X, 2)
                                     + Math.Pow(r1.Location.Y - r2.Location.Y, 2));

            distance.Should().BeLessOrEqualTo(expectedDistance);
        }

        [TestCase(10, 10)]
        [TestCase(20, 10)]
        [TestCase(10, 20)]
        [TestCase(100, 100)]
        [TestCase(1, 100)]
        [TestCase(100, 1)]
        [TestCase(100, 50)]
        [TestCase(50, 100)]
        public void ManyRectanglesAreNotIntersected(int width, int height, int count = 100)
        {
            var sizeRectangle = new Size(width, height);

            for (var i = 0; i < count; i++)
            {
                cloud.PutNextRectangle(sizeRectangle);
            }

            AreThereAnyIntersections(cloud.Rectangles).Should().BeFalse();
        }


        [TestCase(200, 100, 10, 10, 0.99)]
        [TestCase(200, 100, 20, 10, 0.99)]
        [TestCase(200, 100, 10, 20, 0.99)]
        [TestCase(1000, 10, 100, 100, 0.9)]
        [TestCase(300, 100, 1, 100, 0.99)]
        [TestCase(300, 100, 100, 1, 0.99)]
        [TestCase(1000, 100, 100, 50, 0.99)]
        [TestCase(1000, 100, 50, 100, 0.99)]
        public void CorrespondToDistributionDensity(int areaRadius,
            int countRectangles, int weightRectangle, int heightRectangle, double density)
        {
            var area = new Rectangle(Center.X - areaRadius / 2,
                Center.Y - areaRadius / 2,
                areaRadius,
                areaRadius);
            var countIn = 0;
            var size = new Size(weightRectangle, heightRectangle);
            for (var i = 0; i < countRectangles; i++)
            {
                countIn += cloud.PutNextRectangle(size).NestedIn(area) ? 1 : 0;
            }

            ((double)countIn / countRectangles).Should().BeGreaterThan(density);
        }


        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;

            var path = TestContext.CurrentContext.WorkDirectory
                       + TestContext.CurrentContext.Test.MethodName
                       + ".png";

            using (var bitMap = new Bitmap(ScreenSize.Width, ScreenSize.Height))
            {
                var drawer = new CloudLayouterDrawerForTests(cloud);

                drawer.Draw(Graphics.FromImage(bitMap));
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
                bitMap.Save(path, ImageFormat.Png);
            }
        }
    }
}