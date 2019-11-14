using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var rnd = new Random();
            var center = rnd.NextPoint(-100, 100, -100, 100);
            layouter = new CircularCloudLayouter(center);
            layouter.PutRectangles(rnd.NextSizes(10, 30, 2, 5, 10, 50));
        }

        [Test]
        public void AllocateRectanglesWithoutIntersects()
        {
            var rectangles = layouter.GetRectangles().ToList();
            Enumerable.Range(0, rectangles.Count - 1)
                .SelectMany(i => Enumerable.Range(i + 1, rectangles.Count - i - 1)
                    .Select(j => (i, j)))
                    .Any(k => rectangles[k.i].IntersectsWith(rectangles[k.j]))
                    .Should().BeFalse();
        }

        [Test]
        public void BeLikeCircle()
        {
            var rectangles = layouter.GetRectangles();

            const int rayCount = 72;
            var dists = Enumerable
                .Range(0, rayCount)
                .Select(i => i * Math.PI / 36)
                .Select(angle => rectangles.Max(r => r.GetDistanceIfIntersectsByRay(angle)));
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
                .Select(angle => rectangles.Max(r => r.GetDistanceIfIntersectsByRay(angle)));
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
                var filename = $"{Path.GetTempPath()}{TestContext.CurrentContext.Test.Name}-Failed {DateTime.Now:yyyy-MM-dd HH-mm-ss}.png";
                Utils.SaveRectanglesToPngFile(layouter.GetRectangles(), filename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }
    }
}
