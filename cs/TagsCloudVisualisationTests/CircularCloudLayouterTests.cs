using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests
{
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class CircularCloudLayouterTests : LayouterTestBase
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

        [Test, MaxTime(15000)]
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
            while (sw.Elapsed.TotalMinutes <= 1)
            {
                Layouter.PutNextRectangle(new Size(random.Next(4, 20), random.Next(4, 20)));
                count++;
            }

            TestContext.Progress.WriteLine($"Created {count} rectangles per minute, test finished");
        }

        [Test, MaxTime(1000 * 60 * 2)]
        public void PutNextRectangle_PerformanceTesting_WithBigSizes()
        {
            TestBigSizes(100, 2);
        }

        [Test]
        public void PutNextRectangleShould_FirstRectangle_PutAtCenter()
        {
            var size = new Size(10, 10);
            Layouter.PutAndTest(size, new Point(-5, -5));
        }

        [Test, Ignore("Dumb test to visualise things")]
        public void TEST_TO_REMOVE() //TODO REMOVE
        {
            Layouter.Put(new Size(10, 10), out _)
                .Put(new Size(6, 5), out _)
                .Put(new Size(4, 5), out _)
                .Put(new Size(4, 4), out _)
                .Put(new Size(7, 5), out _)
                .Put(new Size(29, 10), out _)
                .Put(new Size(10, 8), out _)
                .Put(new Size(100, 20), out _)
                .Put(new Size(4, 5), out _)
                .Put(new Size(4, 4), out _)
                .Put(new Size(7, 5), out _)
                .Put(new Size(29, 10), out _)
                .Put(new Size(5, 6), out _);
            this.Should().Be("NOT EXISTING", because: "THIS TEST SHOULD BE REMOVED");
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