namespace TagCloud
{
    public class Vector
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsZeroVector()
        {
            return X == 0 && Y == 0;
        }

        public void SetToZeroVector()
        {
            X = 0;
            Y = 0;
        }

        public Vector GetOppositeVector()
        {
            return new Vector(-this.X, -this.Y);
        }

    }
}