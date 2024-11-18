namespace TagsCloudVisualization.SpiralLayouter;

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
}

internal static class DirectionExtensions
{
    public static Direction AntiClockwiseRotate(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Left,
            Direction.Left => Direction.Down,
            Direction.Down => Direction.Right,
            Direction.Right => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}