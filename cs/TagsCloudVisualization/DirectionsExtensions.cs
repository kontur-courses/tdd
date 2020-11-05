using System.Drawing;

namespace TagsCloudVisualization
{
    public static class DirectionsExtensions
    {
        public static Directions Next(this Directions direction) =>
            (Directions) ((int) (direction + 1) % 4);

        public static Directions Previous(this Directions direction)
        {
            var result = (int) direction - 1;
            if (result == -1) result = 3;
            return (Directions) result;
        }

        public static Directions Opposite(this Directions direction) => direction.Next().Next();

        public static bool IsNormal(this Directions direction) =>
            direction == Directions.Right || direction == Directions.Down;

        public static Size GetOffset(this Directions direction, int offset = 1) =>
            direction switch
            {
                Directions.Up => new Size(0, -offset),
                Directions.Left => new Size(-offset, 0),
                Directions.Down => new Size(0, offset),
                Directions.Right => new Size(offset, 0),
                _ => new Size(0, 0)
            };
    }
}