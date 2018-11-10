using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using System.Numerics;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private readonly Random random = new Random(0);

        [SetUp]
        public void Init()
        {
            center = new Point(1920 / 2, 1080 / 2);
            layouter = new CircularCloudLayouter(center);
            for (var i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(random.Next(60, 100), random.Next(20, 50)));
            }
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
                var visualizer = new RectanglesVisualizer(layouter.Rectangles);
                var bitmap = visualizer.RenderToBitmap();
                bitmap.Save(filename, imageFormat);
            }
        }

        [Test]
        public void Should_HaveNoRectangles_AfterCreation()
        {
            new CircularCloudLayouter(center).Rectangles.Should().BeEmpty();
        }

        [Test]
        public void Should_HaveNoIntersectingRectangles()
        {
            foreach (var rectangle in layouter.Rectangles)
            {
                var haveIntersections = layouter.Rectangles.Where(r => !r.Equals(rectangle)).Any(rectangle.IntersectsWith);
                haveIntersections.Should().BeFalse();
            }
        }

        [Test]
        public void Rectangles_ShouldBeClose_ToEachOther()
        {
            var rectangles = layouter.Rectangles;
            var totalArea = rectangles.Sum(r => r.Width * r.Height);
            var radius = Math.Sqrt(totalArea / Math.PI); // радиус окружности с такой площадью
            var rectanglesInside = rectangles.Count(r => GetDistanceToCloudCenter(r) <= radius);
            TestContext.Out.WriteLine("Rectangles: {0}", rectangles.Length);
            TestContext.Out.WriteLine("With center inside: {0}", rectanglesInside);

            // будем считать что прямоугольники расположены плотно, если
            // более чем для 80% их верно, что центр прямоугольника
            // лежит внутри круга вычисленного радиуса
            rectanglesInside.Should().BeGreaterThan((int)(rectangles.Length * 0.8));
        }

        private double GetDistanceToCloudCenter(Rectangle rectangle)
        {
            var rectCenter = FindCenterOfRectangle(rectangle);
            return Math.Sqrt(Math.Pow(center.X - rectCenter.X, 2) + Math.Pow(center.Y - rectCenter.Y, 2));
        }

        [Test]
        public void TagCloud_ShouldHave_CirleShape()
        {
            var vector = new Vector2();
            foreach (var rectangle in layouter.Rectangles)
            {
                var rectCenter = FindCenterOfRectangle(rectangle);
                vector += new Vector2(rectCenter.X - center.X, rectCenter.Y - center.Y);
                TestContext.Out.WriteLine(vector);
            }

            vector.Length().Should().BeLessThan(int.MaxValue);
        }

        private PointF FindCenterOfRectangle(Rectangle rectangle)
        {
            var x = rectangle.Left + rectangle.Width / 2.0f;
            var y = rectangle.Top + rectangle.Height / 2.0f;
            return new PointF(x, y);
        }
    }
}
