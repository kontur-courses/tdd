using System;

namespace TagsCloudVisualization
{
    public static class DirectionExtensions
    {
        public static Direction GetReversed(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Top => Direction.Bottom,
                Direction.Right => Direction.Left,
                Direction.Bottom => Direction.Top,
                _ => throw new ArgumentException($"Can't get reversed of {direction}", nameof(direction)),
            };
        }

        public static readonly Direction All = GetAllFlag();

        private static Direction GetAllFlag()
        {
            var enumType = typeof(Direction);
            long newValue = 0;
            var enumValues = Enum.GetValues(enumType);
            foreach (var value in enumValues)
            {
                var v = (long)Convert.ChangeType(value, TypeCode.Int64);
                newValue |= v;
            }

            return (Direction)Enum.ToObject(enumType, newValue);
        }
    }
}
