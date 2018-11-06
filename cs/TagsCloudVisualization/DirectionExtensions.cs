using System.Collections.Generic;
using System.Linq;
using static TagsCloudVisualization.Direction;

namespace TagsCloudVisualization
{
    public static class DirectionExtensions
    {
        private static readonly Direction[] Clockwise = {Up, Right, Down, Left};

        private static readonly Dictionary<Direction, (int dx, int dy)> DirectionToOffset =
            new Dictionary<Direction, (int, int)>
            {
                [Up] = (0, -1),
                [Right] = (1, 0),
                [Down] = (0, 1),
                [Left] = (-1, 0)
            };

        public static (int dx, int dy) GetOffset(this Direction direction)
        {
            return DirectionToOffset[direction];
        }

        public static List<Direction> ToClockwiseCycle(this Direction direction)
        {
            return Clockwise.Cycle().SkipWhile(d => d != direction).Take(4).ToList();
        }

        public static List<Direction> ToCounterClockwiseCycle(this Direction direction)
        {
            return Clockwise.Reverse().Cycle().SkipWhile(d => d != direction).Take(4).ToList();
        }
    }
}
