using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests
{
    [SaveResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class CircularCloudLayouterTests : LayouterTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_PerformanceTesting_100()
        {
            TestWithRandomSizes(100, 1, 20);
        }

        [Test, Timeout(2000)]
        public void PutNextRectangle_PerformanceTesting_WithBigSizes()
        {
            TestWithRandomSizes(100, 10, 1000);
        }

        [Test, Timeout(10000)]
        public void PutNextRectangle_PerformanceTesting_1000()
        {
            TestWithRandomSizes(1000, 1, 20);
        }

        [Test]
        public void PutNextRectangleShould_FirstRectangle_PutAtCenter()
        {
            var size = new Size(10, 10);
            Layouter.PutAndTest(size, new Point(-5, -5));
        }

        [Test]
        public void PutNextRectangleShould_() //TODO test name
        {
            Layouter.Put(new Size(10, 10), out _)
                .Put(new Size(2, 3), out _)
                .Put(new Size(4, 3), out _)
                .Put(new Size(1, 3), out _)
                .Put(new Size(5, 2), out _)
                .Put(new Size(6, 1), out _)
                .Put(new Size(6, 7), out _)
                .Put(new Size(2, 7), out _)
                .Put(new Size(4, 10), out _)
                .Put(new Size(10, 10), out _);
            Assert.Fail();
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
    }
}