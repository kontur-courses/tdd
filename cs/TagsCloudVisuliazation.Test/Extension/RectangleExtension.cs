using System.Drawing;

namespace TagsCloudVisuliazation.Test.Extension;

public static class RectangleExtension
{
    public static bool HasNeighbor(this Rectangle cur, List<Rectangle> rectangles)
        => rectangles.Any(
            rectangle => cur.Bottom == rectangle.Top || cur.Bottom + 1 == rectangle.Top ||
                         cur.Left == rectangle.Right || cur.Left - 1 == rectangle.Right ||
                         cur.Right == rectangle.Left || cur.Right + 1 == rectangle.Left ||
                         cur.Top == rectangle.Bottom || cur.Top - 1 == rectangle.Bottom);
}