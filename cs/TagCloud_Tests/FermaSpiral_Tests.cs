using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloud_Tests
{
    class FermaSpiral_Tests
    {
        [TestCase(100, 5, TestName = "AtHundredPoints")]
        [TestCase(1000, 3, TestName = "AtThousandPoints")]
        public void ContainsCountOfPointsInQuartersThatAlmostEqual(int countPoints, int accuracy)
        {
            var counts = GetPointCountsInQuaters(countPoints);
            var isEql = IsElementsAlmostEqual(counts, accuracy);

            isEql.Should().BeTrue();
        }

        private static List<int> GetPointCountsInQuaters(int countPoints)
        {
            var points = new List<Point>();
            var spiral = new FermaSpiral(1, new Point(0, 0));

            for (var i = 0; i < countPoints; i++)
                points.Add(spiral.GetSpiralNext());
            var firstQuarterPointsCount = points.Count(p => p.X > 0 && p.Y > 0);
            var secondQuarterPointsCount = points.Count(p => p.X < 0 && p.Y > 0);
            var thirdQuarterPointsCount = points.Count(p => p.X > 0 && p.Y < 0);
            var fourthQuarterPointsCount = points.Count(p => p.X > 0 && p.Y > 0);
            var counts = new List<int>
            {
                firstQuarterPointsCount,
                secondQuarterPointsCount,
                thirdQuarterPointsCount,
                fourthQuarterPointsCount
            };
            return counts;
        }

        private static bool IsElementsAlmostEqual(List<int> counts, int accuracy)
        {
            var isAlmostEql = true;
            counts.ForEach(first => counts.ForEach(second =>
            {
                if (Math.Abs(first - second) > accuracy)
                    isAlmostEql = false;
            }));
            return isAlmostEql;
        }

    }
}
