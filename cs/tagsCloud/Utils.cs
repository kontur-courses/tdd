using System.Drawing;

namespace TagsCloud;

public class Utils
{
    private static readonly Random Random = new();
    private const int MinSize = 1;
    private const int MaxSize = 50;


    public static Color GetRandomColor() => Color.FromArgb(GetShade(), GetShade(), GetShade());


    private static int GetShade() => Random.Next(0, 256);


    public static Size GetRandomSize() =>
        new Size(Random.Next(MinSize, MaxSize), Random.Next(MinSize, MaxSize));

    public static bool IsInside(Rectangle rect1, Rectangle rect2) =>
         rect1.Left <= rect2.Left && rect1.Top <= rect2.Top 
               && rect1.Right >= rect2.Right && rect1.Bottom >= rect2.Bottom;
}