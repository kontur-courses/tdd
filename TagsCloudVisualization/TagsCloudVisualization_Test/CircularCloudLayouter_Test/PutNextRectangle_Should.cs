using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System;
using NUnit.Framework.Interfaces;
using System.Diagnostics;
using System.IO;

namespace TagsCloudVisualization_Test
{
    public class PutNextRectangle_Should
    {
        private CircularCloudLayouter layout;
        private int count = 0;

        [SetUp]
        public void Setup()
        {
            count++;
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            //if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var directory = Directory.GetCurrentDirectory();
                var path = Path.Combine(directory, $"{count:00.}_{context.Result.Outcome.Status}");

                using (var bitmap = BitmapDrawer.Draw(layout))
                    BitmapDrawer.Save(bitmap, path);

                Console.WriteLine($"{ context.Test.Name} {context.Result.Outcome.Status} - Image Saved");
            }
        }

        [TestCase(0, 0)]
        [TestCase(10, 10)]
        [TestCase(-10, 10)]
        [TestCase(-10, -10)]
        [TestCase(10, -10)]
        public void SetCenter_InConstructor_ByParameter(int x, int y)
        {
            var center = new Point(x, y);
            GetLayouter(center).Center.Should().Be(center);
        }

        [Test]
        public void PutRectangle_WithCorrectSize_ByParameter()
        {
            var rectSize = new Size(50, 20);
            GetLayouter(Point.Empty).PutNextRectangle(rectSize).Size.Should().Be(rectSize);
        }

        [Test]
        public void Throw_OnNegativeSize()
        {
            var rectSize = new Size(-50, -20);
            Action putRect = () => GetLayouter(Point.Empty).PutNextRectangle(rectSize).Size.Should().Be(rectSize);
            putRect.Should().Throw<ArgumentException>().Which.Message.Should().StartWith("Отрицательный размер прямоугольника");
        }



        [TestCase(0, 0)]
        [TestCase(10, 10)]
        [TestCase(-10, 10)]
        [TestCase(-10, -10)]
        [TestCase(10, -10)]
        public void PutFirstRectangle_InCenter(int x, int y)
        {
            var center = new Point(x, y);
            var rectSize = new Size(50, 20);
            GetLayouter(center).PutNextRectangle(rectSize).IntersectsWith(new Rectangle(center, Size.Empty)).Should().BeTrue();
        }

        [Test]
        public void KeepLaidRectangles()
        {
            var rectSises = TestHelper.GenerateSizes(10);
            GetLayouter(null, rectSises).GetLaidRectangles().Select(r => r.Size).Should().BeEquivalentTo(rectSises);
        }

        [Test]
        public void SecondRectangle_NotInterceptWithFirst()
        {
            var rectSises = TestHelper.GenerateSizes(2);
            var rects = GetLayouter(null, rectSises).GetLaidRectangles();
            rects.First().IntersectsWith(rects.Last()).Should().BeFalse();
        }

        [Test]
        public void PutManyRectangles_And_HasNoIntersects()
        {
            var rectSises = TestHelper.GenerateSizes(300);
            var layouter = GetLayouter(null, rectSises);
            TestHelper.CheckIntersects(layouter.GetLaidRectangles().ToList()).Should().BeEmpty();
        }

        [Test]
        public void Put1000Rectangles_TakesLess2SecondsAverage()
        {
            long elapsed = 0;
            var mesuresCount = 10;
            for (int i = 0; i < mesuresCount; i++)
            {
                var watch = new Stopwatch();
                watch.Start();
                var rectSises = TestHelper.GenerateSizes(1000);
                GetLayouter(null, rectSises);
                watch.Stop();
                elapsed += watch.ElapsedMilliseconds;
            }
            elapsed.Should().BeLessThan(2_000 * mesuresCount);
        }

        [Test, Timeout(2_000)]
        public void Put1000Rectangles_TakesLess2Seconds()
        {
            var rectSises = TestHelper.GenerateSizes(1000);
            GetLayouter(null, rectSises);
        }

        [Test]
        public void PutRectangles_WithOver75PercentAverageDensity()
        {
            var targetFactor = 0.75;
            var testAmount = 100;
            double sumFactor = 0;
            for (int i = 0; i < testAmount; i++)
            {
                var rectSises = TestHelper.GenerateSizes(100);
                var layouter = GetLayouter(null, rectSises);
                sumFactor += TestHelper.GetDensityFactor(layouter.GetLaidRectangles().ToList(), layouter.Center);
            }
            var actualFactor = Math.Round(sumFactor / testAmount, 2);
            actualFactor.Should().BeGreaterThan(targetFactor);
        }

        [Test]
        public void PutVeryBigRectangle_WithOver75PercentAverageDensity()
        {
            var targetFactor = 0.75;
            var testAmount = 10;
            double sumFactor = 0;
            for (int i = 0; i < testAmount; i++)
            {
                var rectSises = TestHelper.GenerateSizes(30);
                rectSises.Add(new Size(2000, 1500));
                rectSises.AddRange(TestHelper.GenerateSizes(100));

                var layouter = GetLayouter(null, rectSises);
                sumFactor += TestHelper.GetDensityFactor(layouter.GetLaidRectangles().ToList(), layouter.Center);
            }
            var actualFactor = Math.Round(sumFactor / testAmount, 2);
            actualFactor.Should().BeGreaterThan(targetFactor);
        }

        [TestCase(50, 30)]
        [TestCase(25, 3)]
        [TestCase(100, 3)]
        public void PutSquares_TakesMinArea(int width, int side)
        {
            var rectSises = new List<Size>();
            var size = new Size(width, width);
            count = side * side;
            Enumerable.Range(0, count).ToList().ForEach(_ => rectSises.Add(size));
            var layouter = GetLayouter(null, rectSises);
            var expectedSquare = size.Width * size.Height * count;
            var union = TestHelper.UnionAll(layouter.GetLaidRectangles().ToList());
            double square = union.Width * union.Height;
            square.Should().BeApproximately(expectedSquare, expectedSquare * 0.1);
        }

        private CircularCloudLayouter GetLayouter(Point? center) =>
            new CircularCloudLayouter(center ?? Point.Empty);

        private CircularCloudLayouter GetLayouter(Point? center,
            List<Size> rectSises)
        {
            center = center ?? Point.Empty;
            var layouter = new CircularCloudLayouter(center.Value);
            rectSises.ForEach(s => layouter.PutNextRectangle(s));
            layout = layouter;
            return layouter;
        }
    }
}