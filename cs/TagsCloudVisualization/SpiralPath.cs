using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

namespace TagsCloudVisualization
{
    public static class SpiralPath
    {
        public static IEnumerable<Direction> Clockwise(Direction startDirection)
        {
            return GetPath(startDirection, true);
        }

        public static IEnumerable<Direction> CounterClockwise(Direction startDirection)
        {
            return GetPath(startDirection, false);
        }

        private static IEnumerable<Direction> GetPath(Direction startDirection, bool isClockwise)
        {
            var cycle = isClockwise ? startDirection.ToClockwiseCycle() : startDirection.ToCounterClockwiseCycle();
            for (var stepLength = 3;; stepLength += 2)
            {
                var halfStep = (stepLength - 1) / 2;
                yield return startDirection;
                foreach (var d in Repeat(cycle[1],halfStep)
                    .Concat(Repeat(cycle[2], stepLength-1))
                    .Concat(Repeat(cycle[3], stepLength-1))
                    .Concat(Repeat(cycle[0], stepLength-1))
                    .Concat(Repeat(cycle[1], halfStep)))
                {
                    yield return d;
                }
            }
        }
    }
}
