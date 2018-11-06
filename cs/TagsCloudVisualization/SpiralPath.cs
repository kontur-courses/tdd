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
            for (var diameter = 3;; diameter += 2)
            {
                var step = diameter - 1;
                yield return startDirection;
                foreach (var d in Repeat(cycle[1], step-1)
                    .Concat(Repeat(cycle[2], step))
                    .Concat(Repeat(cycle[3], step))
                    .Concat(Repeat(cycle[0], step))
                    .Concat(Repeat(cycle[1], step)))
                {
                    yield return d;
                }
            }
        }
    }
}
