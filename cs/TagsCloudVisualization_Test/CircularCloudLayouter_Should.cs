using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization_Test
{
    public class CircularCloudLayouter_Should
    {
        private BitmapDrawer drawer;
        private int count = 0;

        [SetUp]
        public void Setup()
        {
            count++;
        }

        [TearDown]
        public void TearDown()
        {
            if (drawer == null)
                return;
            var context = TestContext.CurrentContext;
            //if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            drawer.Draw();
            drawer.Save($"{count:00.}_{context.Result.Outcome.Status}");
            Console.WriteLine($"{context.Test.Name} {context.Result.Outcome.Status} - Image Saved");
        }

        [Test]
        public void SetCenter_InConstructor_ByParameter()
        {
            var center = new Point(10, 10);
            GetLayouter(center).Center.Should().Be(center);
        }

        [Test]
        public void PutRectangle_WithCorrectSize_ByParameter()
        {
            var rectSize = new Size(50, 20);
            GetLayouter(Point.Empty).PutNextRectangle(rectSize).Size.Should().Be(rectSize);
        }

        [Test]
        public void PutFirstRectangle_InCenter()
        {
            var center = new Point(13, 17);
            var rectSize = new Size(50, 20);
            GetLayouter(center).PutNextRectangle(rectSize).IntersectsWith(new Rectangle(center, Size.Empty)).Should().BeTrue();
        }

        [Test]
        public void Keep_LaidRectangles()
        {
            var rectSises = TestHelper.GenerateSizes(10);
            GetLayouter(null, rectSises).Rectangles.Select(r => r.Size).Should().BeEquivalentTo(rectSises);
        }

        [Test]
        public void SecondRectangle_NotInterceptWithFirst()
        {
            var rectSises = TestHelper.GenerateSizes(2);
            var rects = GetLayouter(null, rectSises).Rectangles;
            rects.First().IntersectsWith(rects.Last()).Should().BeFalse();
        }

        [Test]
        public void PutManyRectangles_And_HasNoIntersects()
        {
            var rectSises = TestHelper.GenerateSizes(300);
            var layouter = GetLayouter(null, rectSises);
            TestHelper.CheckIntersects(layouter.Rectangles).Should().BeEmpty();
        }

        [Test]
        public void PutRectangles_WithHighAverageDensityCoeff()
        {
            var targetFactor = 0.8;
            var testAmount = 100;
            double sumFactor = 0;
            for (int i = 0; i < testAmount; i++)
            {
                var rectSises = TestHelper.GenerateSizes(50);
                var layouter = GetLayouter(null, rectSises);
                sumFactor += TestHelper.GetDensityFactor(layouter.Rectangles, layouter.Center);
            }
            var actualFactor = Math.Round(sumFactor / testAmount, 2);
            actualFactor.Should().BeGreaterThan(targetFactor);
        }

        [Test, Timeout(10_000)]
        public void Put500Rectangles_TakesLess5seconds()
        {
            var rectSises = TestHelper.GenerateSizes(500);
            GetLayouter(null, rectSises);
        }

        [Test]
        public void PutVeryBigRectangleWithNoIntersect()
        {
            var rectSises = TestHelper.GenerateSizes(3);
            rectSises.Add(new Size(2000, 1000));
            rectSises.AddRange(TestHelper.GenerateSizes(100));
            var layouter = GetLayouter(null, rectSises);

            TestHelper.CheckIntersects(layouter.Rectangles).Should().BeEmpty();
        }

        [Test]
        public void PutSquaresOptimally()
        {
            var rectSises = new List<Size>();
            var size = new Size(50, 50);
            var count = 9;
            Enumerable.Range(0, count).ToList().ForEach( _ => rectSises.Add(size));
            var layouter = GetLayouter(null, rectSises);
            var expectedSquare = size.Width * size.Height * count;
            var union = TestHelper.UnionAll(layouter.Rectangles);
            var square = union.Width * union.Height;
            square.Should().Be(expectedSquare);
        }

        private CircularCloudLayouter GetLayouter(Point? center) =>
            new CircularCloudLayouter(center ?? Point.Empty);

        private CircularCloudLayouter GetLayouter(Point? center,
            List<Size> rectSises)
        {
            center = center ?? Point.Empty;
            var layouter = new CircularCloudLayouter(center.Value);
            rectSises.ForEach(s => layouter.PutNextRectangle(s));
            drawer = new BitmapDrawer(layouter);
            return layouter;
        }
    }
}