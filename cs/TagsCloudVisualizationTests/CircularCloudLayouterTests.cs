using System;
using System.Collections.Generic;
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
        private Rectangle[] cloudRectangles;
        private readonly Random random = new Random(0);

        [SetUp]
        public void Init()
        {
            center = new Point(1920 / 2, 1080 / 2);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
            {
                var rectangle = layouter.PutNextRectangle(new Size(random.Next(60, 100), random.Next(20, 50)));
                rectangles.Add(rectangle);
            }

            cloudRectangles = rectangles.ToArray();
        }

        [TearDown]
        public void SaveBitmapForFailedTest()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status != TestStatus.Passed)
            {
                var imageFormat = ImageFormat.Jpeg;
                var fileName = string.Format("{0}.{1}", context.Test.Name, imageFormat.ToString().ToLower());
                var outputDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fullPath = string.Format(@"{0}\{1}", outputDir, fileName);
                TestContext.Out.WriteLine("Tag cloud visualization saved to file {0}", fullPath);
                var visualizer = new RectanglesVisualizer(cloudRectangles);
                var bitmap = visualizer.RenderToBitmap();
                bitmap.Save(fullPath, imageFormat);
            }
        }

        [Test]
        public void Should_HaveNoIntersectingRectangles()
        {
            foreach (var rectangle in cloudRectangles)
            {
                var haveIntersections = cloudRectangles.Where(r => !r.Equals(rectangle)).Any(rectangle.IntersectsWith);
                haveIntersections.Should().BeFalse();
            }
        }

        [Test]
        public void Rectangles_ShouldBeClose_ToEachOther()
        {
            var totalArea = cloudRectangles.Sum(r => r.Width * r.Height);
            var radius = Math.Sqrt(totalArea / Math.PI);
            var rectanglesInside = cloudRectangles.Count(r => r.Center().DistanceTo(center) <= radius);

            // будем считать что прямоугольники расположены плотно, если
            // более чем для 80% их верно, что центр прямоугольника
            // лежит внутри круга вычисленного радиуса
            rectanglesInside.Should().BeGreaterThan((int)(cloudRectangles.Length * 0.8));
        }

        [Test]
        public void Rectangles_ShouldForm_CirleShape()
        {
            var centroid = GetCentroid(cloudRectangles);
            var coveringCircleRadius = cloudRectangles.Max(r => r.Center().DistanceTo(centroid));
            var convexHull = GetConvexHull(cloudRectangles);
            const int acceptableDelta = 60;
            foreach (var rectangle in convexHull)
            {
                var distanceFromCoveringCircleToConvexHull =
                    Math.Abs(coveringCircleRadius - rectangle.Center().DistanceTo(centroid));

                distanceFromCoveringCircleToConvexHull.Should().BeLessThan(acceptableDelta);
            }
        }

        private PointF GetCentroid(Rectangle[] rectangles)
        {
            var x = rectangles.Sum(r => r.Center().X) / rectangles.Length;
            var y = rectangles.Sum(r => r.Center().Y) / rectangles.Length;
            return new PointF(x, y);
        }

        // алгоритм Джарвиса https://en.wikipedia.org/wiki/Gift_wrapping_algorithm
        private Rectangle[] GetConvexHull(Rectangle[] rectangles)
        {
            var hull = new List<Rectangle>();
            var minX = rectangles.Min(r => r.Center().X);
            var rectInHull = rectangles.First(r => r.Center().X == minX);

            while (true)
            {
                hull.Add(rectInHull);
                var lastRect = rectangles[0];
                for (int i = 0; i < rectangles.Length; i++)
                {
                    if ((lastRect == rectInHull)
                        || (GetOrientation(rectInHull.Center(), lastRect.Center(), rectangles[i].Center()) == 1))
                    {
                        lastRect = rectangles[i];
                    }
                }

                rectInHull = lastRect;
                if (lastRect == hull.First())
                    break;
            }

            return hull.ToArray();
        }

        private int GetOrientation(PointF p, PointF q, PointF r)
        {
            float det = (q.X - p.X) * (r.Y - p.Y) - (r.X - p.X) * (q.Y - p.Y);

            if (det > 0)
                return -1;
            if (det < 0)
                return 1;

            return 0;
        }
    }
}
