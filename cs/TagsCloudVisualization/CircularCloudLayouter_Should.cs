using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var rnd = new Random();
            var center = new Point(rnd.Next(-100, 100), rnd.Next(-100, 100));
            var count = rnd.Next(10, 50);
            layouter = new CircularCloudLayouter(center);
            layouter.PutRectangles(Enumerable.Range(0, count).Select(_ => rnd.GenerateRandomSize()));
        }

        [Test]
        public void AllocateRectanglesWithoutIntersects()
        {
            var rectangles = layouter.GetRectangles().ToList();
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void BeLikeCircle()
        {
            var rectangles = layouter.GetRectangles();

            const int rayCount = 72;
            var dists = Enumerable
                .Range(0, rayCount)
                .Select(i => i * Math.PI / 36)
                .Select(angle => rectangles.Select(r => r.IsIntersectsByRay(angle, out double intersectionPointDistance) ? intersectionPointDistance : 0).Max());
            var mid = dists.Sum() / rayCount;

            dists.Any(dist => Math.Abs(dist - mid) / mid > 0.5)
                .Should().BeFalse("deviation of rectangles distance and circle radius should be less than 50% of circle radius");
        }

        [Test]
        public void BeCompact()
        {
            var rectangles = layouter.GetRectangles();

            const int rayCount = 72;
            var dists = Enumerable
                .Range(0, rayCount)
                .Select(i => i * Math.PI / 36)
                .Select(angle => rectangles.Select(r => r.IsIntersectsByRay(angle, out double intersectionPointDistance) ? intersectionPointDistance : 0).Max());
            var mid = dists.Sum() / rayCount;
            var circleSquare = Math.PI * mid * mid;
            var rectanglesSquare = rectangles.Sum(r => r.Square());

            double unusedPercent = (circleSquare - rectanglesSquare) / circleSquare * 100;
            unusedPercent.Should().BeLessThan(40, "unused space should be less than 40% of circle square");
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var filename = $"{Path.GetTempPath()}{TestContext.CurrentContext.Test.Name}-Failed_{(int)DateTime.Now.TimeOfDay.TotalSeconds}.bmp";
                layouter.SaveToFile(filename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }
    }
}
