using System.Drawing;
using System.Reflection;
using FluentAssertions;
using TagCloud;

namespace TagCloudTests;

[TestFixture]
public class SpiralTests
{
    private static double GetCurrentAngle(Spiral spiral)
    {
        var field = typeof(Spiral).GetField("currentAngle",
            BindingFlags.Instance | BindingFlags.NonPublic);
        field.Should().NotBeNull("Spiral should have private field currentAngle");
        return (double)field.GetValue(spiral)!;
    }

    [Test]
    public void GetNextPoint_ShouldIncreaseAngleByBaseIncrement()
    {
        const double angleIncrement = 1.0;
        var spiral = new Spiral(new Point(0, 0), angleIncrement);
        var angleBefore = GetCurrentAngle(spiral);
        
        spiral.GetNextPoint();
        var angleAfter = GetCurrentAngle(spiral);
        
        angleAfter.Should().BeApproximately(angleBefore + angleIncrement, 1e-10);
    }

    [Test]
    public void RetryGetNextPoint_ShouldIncreaseAngleWithGrowingMultiplier()
    {
        const double angleIncrement = 1.0;
        var spiral = new Spiral(new Point(0, 0), angleIncrement);

        spiral.GetNextPoint();
        var angleAfterFirst = GetCurrentAngle(spiral);
        spiral.RetryGetNextPoint();
        var angleAfterRetry1 = GetCurrentAngle(spiral);
        spiral.RetryGetNextPoint(); 
        var angleAfterRetry2 = GetCurrentAngle(spiral);
        var delta1 = angleAfterRetry1 - angleAfterFirst;
        var delta2 = angleAfterRetry2 - angleAfterRetry1;

        delta1.Should().BeApproximately(2 * angleIncrement, 1e-10);
        delta2.Should().BeApproximately(4 * angleIncrement, 1e-10);
    }

    [Test]
    public void GetNextPoint_ShouldResetFailureStreakAndMultiplier()
    {
        const double angleIncrement = 1.0;
        var spiral = new Spiral(new Point(0, 0), angleIncrement);
        
        spiral.GetNextPoint();
        spiral.RetryGetNextPoint();
        spiral.RetryGetNextPoint();
        var angleBefore = GetCurrentAngle(spiral);
        
        spiral.GetNextPoint();
        var angleAfter = GetCurrentAngle(spiral);
        
        var delta = angleAfter - angleBefore;
        delta.Should().BeApproximately(angleIncrement, 1e-10);
    }

    [Test]
    public void Points_OnSubsequentCalls_ShouldMoveOutwardsFromCenter()
    {
        var center = new Point(10, 20);
        var spiral = new Spiral(center, angleIncrement: 0.5);

        var p1 = spiral.GetNextPoint();
        var p2 = spiral.GetNextPoint();
        var p3 = spiral.GetNextPoint();
        var d1 = PreviousDistanceSquared(p1);
        var d2 = PreviousDistanceSquared(p2);
        var d3 = PreviousDistanceSquared(p3);
        d2.Should().BeGreaterThanOrEqualTo(d1);
        d3.Should().BeGreaterThanOrEqualTo(d2);
        return;

        double PreviousDistanceSquared(Point p) =>
            (p.X - center.X) * (p.X - center.X) + (p.Y - center.Y) * (p.Y - center.Y);
    }

    [Test]
    public void GetNextPoint_ThatOnAverageMoveAwayFromCenter_ShouldProducePoints()
    {
        var center = new Point(0, 0);
        var spiral = new Spiral(center, angleIncrement: 0.1);

        var distances = new List<double>();
        
        const int steps = 100;
        for (var i = 0; i < steps; i++)
        {
            var p = spiral.GetNextPoint();
            distances.Add(DistanceSquared(p, center));
        }
        
        var max = distances[0];
        var maxIndex = 0;
        for (var i = 1; i < distances.Count; i++)
        {
            if (!(distances[i] > max)) continue;
            max = distances[i];
            maxIndex = i;
        }
        
        maxIndex.Should().BeGreaterThan(steps / 2);
    }

    [Test]
    public void RetryGetNextPoint_FromCenter_ShouldMoveFurtherFromCenter()
    {
        var center = new Point(10, 20);
        var spiral = new Spiral(center, angleIncrement: 0.2);
        
        var first = spiral.GetNextPoint();
        var distFirst = DistanceSquared(first, center);
        var last = first;
        const int retries = 5;
        for (var i = 0; i < retries; i++)
        {
            last = spiral.RetryGetNextPoint();
        }
        var distLast = DistanceSquared(last, center);
        
        distLast.Should().BeGreaterThan(distFirst);
    }
    
    private static double DistanceSquared(Point p, Point center) =>
        (p.X - center.X) * (p.X - center.X) + (p.Y - center.Y) * (p.Y - center.Y);
}
