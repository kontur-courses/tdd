using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private readonly Random random = new Random();

        [SetUp]
        public void Init()
        {
            center = new Point(random.Next(1920), random.Next(1080));
            layouter = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void SaveBitmapForFailedTest()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status != TestStatus.Passed)
            {
                var imageFormat = ImageFormat.Jpeg;
                string filename = string.Format("{0}.{1}", context.Test.Name, imageFormat.ToString().ToLower());
                var outputDir = context.WorkDirectory;
                TestContext.Out.WriteLine(@"Tag cloud visualization saved to file {0}\{1}", outputDir, filename);
                var visualizator = new Visualizator(layouter.Rectangles);
                var bitmap = visualizator.CreateBitmap(new Size(1920, 1080));
                bitmap.Save(filename, imageFormat);
            }
        }

        [Test]
        public void Should_HaveSpecifiedCenter()
        {
            layouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void Should_HaveNoRectangles_AfterCreation()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void Should_HaveNoIntersectingRectangles()
        {
            for (var i = 0; i < random.Next(10, 100); i++)
            {
                var rectangle = layouter.PutNextRectangle(new Size(random.Next(60, 100), random.Next(20, 50)));
                var haveIntersections = layouter.Rectangles.Where(r => !r.Equals(rectangle)).Any(rectangle.IntersectsWith);
                haveIntersections.Should().BeFalse();
            }
        }

        [Test]
        public void Rectangles_ShouldBeClose_ToEachOther()
        {
            var rectangles = layouter.Rectangles;
            for (var i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(random.Next(60, 100), random.Next(20, 50)));
            }
            var totalArea = rectangles.Sum(GetArea);
            var radius = Math.Sqrt(totalArea / Math.PI); // радиус окружности с такой площадью
            var rectanglesInside = rectangles.Count(r => GetDistanceToCenter(r) <= radius);
            TestContext.Out.WriteLine("Rectangles: {0}", rectangles.Count);
            TestContext.Out.WriteLine("Inside: {0}", rectanglesInside);

            // будем считать что прямоугольники расположены плотно, если
            // более чем для 90% их верно, что верхний левый угол прямоугольника
            // лежит внутри круга вычисленного радиуса
            rectanglesInside.Should().BeGreaterThan((int)(rectangles.Count * 0.9));
        }

        private int GetArea(Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }

        private double GetDistanceToCenter(Rectangle rectangle)
        {
            return Math.Sqrt(Math.Pow(center.X - rectangle.X, 2) + Math.Pow(center.Y - rectangle.Y, 2));
        }
    }
}
