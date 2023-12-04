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
}