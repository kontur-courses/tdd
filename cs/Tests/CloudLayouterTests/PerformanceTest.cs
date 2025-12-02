using System.Diagnostics;
using FluentAssertions;

namespace Tests.CloudLayouterTests;

public class PerformanceTest : CircularCloudLayouterTestBase
{
    [Test]
    public void TimeGrowth_ShouldNotBeWorseThanQuadratic()
    {
        var testSizesCounts = new[] { 1000, 2500, 5000, 10000 };
        var executionTimes = new List<long>();

        foreach (var count in testSizesCounts)
        {
            SetUp();

            var sw = Stopwatch.StartNew();
            foreach (var size in SizesGenerator.GenerateRandomRectangles(count, TestSeed))
                Layouter.PutNextRectangle(size);
            sw.Stop();

            executionTimes.Add(sw.ElapsedMilliseconds);

            TestContext.WriteLine($"N = {count}: {sw.ElapsedMilliseconds}ms");
        }

        AnalyzeTimeGrowth(testSizesCounts, executionTimes);
    }

    private void AnalyzeTimeGrowth(int[] sizes, List<long> times)
    {
        TestContext.WriteLine("\nTime Growth Analysis:");

        var maxAllowedExponent = 2;

        for (var i = 1; i < sizes.Length; i++)
        {
            var n1 = sizes[i - 1];
            var n2 = sizes[i];
            var t1 = times[i - 1];
            var t2 = times[i];

            var exponent = Math.Log((double)t2 / t1) / Math.Log((double)n2 / n1);

            TestContext.WriteLine($"N = {n1}->{n2}, time {t1}->{t2}, exponent = {exponent}");

            exponent.Should().BeLessThan(maxAllowedExponent);
        }
    }
}