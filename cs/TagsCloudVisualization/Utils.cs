namespace TagsCloudVisualization
{
    public static class Utils
    {
        public static DirectionToMove[] GetAllDirections()
        {
            return new[]
            {
                DirectionToMove.Up,
                DirectionToMove.Down,
                DirectionToMove.Left,
                DirectionToMove.Right
            };
        }
    }

    public enum DirectionToMove
    {
        Up,
        Down,
        Left,
        Right
    }
}
