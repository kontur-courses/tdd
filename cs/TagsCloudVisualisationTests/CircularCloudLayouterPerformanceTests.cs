using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests
{
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class CircularCloudLayouterPerformanceTests : LayouterTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test, MaxTime(1000)]
        public void PutNextRectangle_PerformanceTesting_100()
        {
            TestWithRandomSizes(100, 4, 20);
        }

        [Test, MaxTime(10000)]
        public void PutNextRectangle_PerformanceTesting_1000()
        {
            TestWithRandomSizes(1000, 4, 20);
        }

        [Test]
        public void PutNextRectangle_PerformanceTesting_RectanglesPerMinute()
        {
            var random = Randomizer.CreateRandomizer();
            var sw = Stopwatch.StartNew();
            var count = 0;
            var currentRank = 1;
            while (sw.Elapsed.TotalMinutes <= 1)
            {
                count++;
                if (count % currentRank == 0)
                {
                    PrintTestingMessage($"Created {count} rectangles, elapsed {sw.Elapsed}");
                    currentRank *= 10;
                }

                Layouter.PutNextRectangle(new Size(random.Next(4, 20), random.Next(4, 20)));
            }

            PrintTestingMessage($"Created {count} rectangles per minute");
        }


        [Test, MaxTime(1000 * 60 * 2)]
        public void PutNextRectangle_PerformanceTesting_WithBigSizes()
        {
            TestBigSizes(100, 2);
        }

        private void TestWithRandomSizes(int testsCount, int minSize, int maxSize)
        {
            var random = Randomizer.CreateRandomizer();
            var randomSizes = Enumerable.Range(0, testsCount)
                .Select(x => new Size(random.Next(minSize, maxSize), random.Next(minSize, maxSize)))
                .ToArray();

            foreach (var size in randomSizes)
                Layouter.PutNextRectangle(size);
        }

        private void TestBigSizes(int testsCount, byte scale)
        {
            var random = Randomizer.CreateRandomizer();
            var randomSizes = Enumerable.Range(0, testsCount)
                .Select(_ => CreateBigRectangle(random, scale))
                .ToArray();

            foreach (var rect in randomSizes)
                Layouter.PutNextRectangle(rect);
        }

        private Size CreateBigRectangle(Random random, byte scale)
        {
            var height = random.Next(100, 500);
            var width = random.Next(height, height * 3);
            return new Size(width, height) * scale;
        }
    }
}