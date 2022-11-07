using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;


// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {



        private CircularCloudLayouter cloud;
        private static readonly Size ScreenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
        private readonly Point center = new Point(ScreenSize.Width / 2, ScreenSize.Height / 2);

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(center);
        }

        public static bool AreThereAnyIntersectionsBetweenRectangles(Rectangle[] rectangles)
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

        [TestCase(110, 110)]
        public void Test_CircularCloudLayouter_FirstRectangleInCenter(int width, int height)
        {
            var sizeRectangle = new Size(width, height);
            var r = cloud.PutNextRectangle(sizeRectangle);


            r.Size.Should().Be(sizeRectangle);
            r.Location.Should().Be(new Point(center.X - width / 2, center.Y - height / 2));

        }

        [Test]
        public void Test_CircularCloudLayouter_TwoLocationRectanglesAreNotEqual()
        {
            var sizeRectangle = new Size(1000, 1000);
            var r1 = cloud.PutNextRectangle(sizeRectangle);
            var r2 = cloud.PutNextRectangle(sizeRectangle);

            r1.Should().NotBe(r2);
        }

        [Test]
        public void Test_CircularCloudLayouter_TwoRectanglesAreNotIntersected()
        {
            var sizeRectangle = new Size(10, 10);
            var r1 = cloud.PutNextRectangle(sizeRectangle);
            var r2 = cloud.PutNextRectangle(sizeRectangle);

            r1.AreIntersected(r2).Should().BeFalse();
        }

        [Test]
        public void Test_CircularCloudLayouter_TwoRectanglesClose()
        {
            var sizeRectangle = new Size(10, 10);
            var r1 = cloud.PutNextRectangle(sizeRectangle);
            var r2 = cloud.PutNextRectangle(sizeRectangle);
            var length = Math.Sqrt(Math.Pow(r1.Location.X - r2.Location.X, 2)
                                   + Math.Pow(r1.Location.Y - r2.Location.Y, 2));
            length.Should().BeLessOrEqualTo(20);
        }

        [Test]
        public void Test_CircularCloudLayouter_FixedSize_ManyRectanglesAreNotIntersected()
        {
            var sizeRectangle = new Size(10, 10);
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(sizeRectangle));
            }

            AreThereAnyIntersectionsBetweenRectangles(rectangles.ToArray()).Should().BeFalse();
        }

        [Repeat(100)]
        [Test]
        public void Test_CircularCloudLayouter_WithRandomSize_ManyRectanglesAreNotIntersected()
        {
            var random = new Random();

            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
            {
                var sizeRectangle = new Size(random.Next(30, 100), 10);
                rectangles.Add(cloud.PutNextRectangle(sizeRectangle));
            }

            AreThereAnyIntersectionsBetweenRectangles(rectangles.ToArray()).Should().BeFalse();
        }

        [TestCase(1000, 100, 10, 10, 0.99)]
        [TestCase(2000, 100, 20, 10, 0.99)]
        [TestCase(2000, 100, 10, 20, 0.99)]
        [TestCase(1000, 10, 100, 100, 0.9)]
        [TestCase(1000, 500, 1, 100, 0.99)]
        public void Test_CircularCloudLayouter_WithFixedSizeRectangles_CheckDistributionDensity(int areaRadius, int countRectangles, int weightRectangle, int heightRectangle, double density)
        {
            var area = new Rectangle(center.X - areaRadius / 2, center.Y - areaRadius / 2, areaRadius, areaRadius);
            var countIn = 0;
            var size = new Size(weightRectangle, heightRectangle);
            for (var i = 0; i < countRectangles; i++)
            {
                countIn += cloud.PutNextRectangle(size).NestedIn(area) ? 1 : 0;
            }
            ((double)countIn / countRectangles).Should().BeGreaterThan(density);

        }

        [Repeat(100)]
        [TestCase(1000, 100, 10, 10, 0.99)]
        [TestCase(2000, 100, 10, 20, 0.99)]
        [TestCase(1000, 100, 80, 100, 0.80)]
        [TestCase(1000, 100, 50, 100, 0.95)]
        public void Test_CircularCloudLayouter_WithRandomSizeRectangles_CheckDistributionDensity(int areaRadius, int countRectangles, int lowerBound, int upperBound, double density)
        {
            var random = new Random();
            var area = new Rectangle(center.X - areaRadius / 2, center.Y - areaRadius / 2, areaRadius, areaRadius);
            var countIn = 0;
            for (var i = 0; i < countRectangles; i++)
            {
                var size = new Size(random.Next(lowerBound, upperBound), random.Next(lowerBound, upperBound));
                countIn += cloud.PutNextRectangle(size).NestedIn(area) ? 1 : 0;
            }
            ((double)countIn / countRectangles).Should().BeGreaterOrEqualTo(density);

        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;

            var path = TestContext.CurrentContext.WorkDirectory + TestContext.CurrentContext.Test.MethodName + ".bmp";
            var bitMap = new Bitmap(1980, 1080);
            var g = Graphics.FromImage(bitMap);
            var drawer = new DrawerCloudLayouterForTests();
            drawer.Draw(g, cloud.Rectangles);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
            bitMap.Save(path);
        }

    }
}