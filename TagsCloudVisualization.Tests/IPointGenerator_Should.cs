using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using FluentAssertions.Execution;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class IPointGenerator_Should
    {
        [TestCaseSource(nameof(TestCases))]
        public void GeneratePoints_MovingAwayFromTheStartFor(IPointGenerator pointGenerator)
        {
            var start = new Point(0, 0);
            var points = pointGenerator.GeneratePoints(start);
            var nearPoint = points.ElementAt(100);
            var farPoint = points.ElementAt(1000);

            DistanceBetween(start, nearPoint).Should().BeLessThan(DistanceBetween(start, farPoint));
        }

        [TestCaseSource(nameof(TestCases))]
        public void GeneratePoints_ReturnsStartAsFirstPointFor(IPointGenerator pointGenerator)
        {
            var start = new Point(100, 100);
            var firstReturned = pointGenerator.GeneratePoints(start)
                .First();

            firstReturned.Should().BeEquivalentTo(start);
        }

        public static IEnumerable<IPointGenerator> TestCases()
        {
            yield return new ArchemedianSpiral();
            yield return new HeartShaped();
            yield return new DeltaSHaped();
        }

        public int DistanceBetween(Point start, Point destination)
        {
            return (int)(Math.Sqrt((start.X - destination.X) * (start.X - destination.X) +
                                   (start.Y - destination.Y) * (start.Y - destination.Y)));
        }
    }
}
