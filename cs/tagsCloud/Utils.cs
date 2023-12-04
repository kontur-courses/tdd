using System.Drawing;

namespace tagsCloud;

public class Utils
{
    private static readonly Random Random = new();
    private const int MinSize = 1;
    private const int MaxSize = 50;
    private const int MinCoordinate = 0;
    private const int MaxCoordinate = 5000;


    public static readonly Func<Color> GetRandomColor =
        () => Color.FromArgb(GetShade(), GetShade(), GetShade());

    private static readonly Func<int> GetShade =
        () => Random.Next(0, 256);

    public static readonly Func<Size> GetRandomSize =
        () => new Size(Random.Next(MinSize, MaxSize), Random.Next(MinSize, MaxSize));

    public static readonly Func<Point> GetRandomLocation =
        () => new Point(Random.Next(MinCoordinate, MaxCoordinate), Random.Next(MinCoordinate, MaxCoordinate));

    public static readonly Func<Rectangle, Rectangle, bool> IsInside = (rect1, rect2) =>
        rect1.Left <= rect2.Left && rect1.Top <= rect2.Top
                                 && rect1.Right >= rect2.Right && rect1.Bottom >= rect2.Bottom;
}