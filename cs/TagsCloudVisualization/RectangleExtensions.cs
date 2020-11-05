using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Rectangle Displaced(this Rectangle rectangle, Size offset) =>
            new Rectangle(rectangle.Location + offset, rectangle.Size);

        public static Rectangle Displaced(this Rectangle rectangle, Point offset) => rectangle.Displaced(new Size(offset));

        public static int SizeInDirection(this Rectangle rectangle, Directions direction) =>
            direction switch
            {
                Directions.Up => rectangle.Height,
                Directions.Left => rectangle.Width,
                Directions.Down => rectangle.Height,
                Directions.Right => rectangle.Width,
                _ => 0
            };
        
        public static int TopInDirection(this Rectangle rectangle, Directions direction) =>
            direction switch
            {
                Directions.Up => rectangle.Top,
                Directions.Left => rectangle.Left,
                Directions.Down => rectangle.Bottom,
                Directions.Right => rectangle.Right,
                _ => 0
            };
        
        public static Rectangle ResizedToTopInDirection(this Rectangle rectangle, Directions direction, int value)
        {
            var result = new Rectangle(rectangle.Location, rectangle.Size);
            switch (direction)
            {
                case Directions.Up:
                    var offset = direction.GetOffset(value - result.Top);
                    result.Location -= offset;
                    result.Size += offset;
                    break;
                case Directions.Left:
                    offset = direction.GetOffset(value - result.Left);
                    result.Location -= offset;
                    result.Size += offset;
                    break;
                case Directions.Down: result.Size += direction.GetOffset(value - result.Bottom); break;
                case Directions.Right:result.Size += direction.GetOffset(value - result.Right); break;
            }

            return result;
        }
        
        public static Rectangle DisplacedToTopInDirection(this Rectangle rectangle, Directions direction, int value)
        {
            var result = new Rectangle(rectangle.Location, rectangle.Size);
            result.Location += direction.GetOffset((value - result.TopInDirection(direction))
                                                   * (direction.IsPositive() ? 1 : -1));
            return result;
        }

        public static bool IntersectInDirection(this Rectangle r1, Rectangle r2, Directions direction)
        {
            if (direction == Directions.Up || direction == Directions.Down)
            {
                r1.Location = new Point(0, r1.Location.Y);
                r2.Location = new Point(0, r2.Location.Y);
            }
            else
            {
                r1.Location = new Point(r1.Location.X, 0);
                r2.Location = new Point(r2.Location.X, 0);
            }

            return r1.IntersectsWith(r2);
        }
    }
}