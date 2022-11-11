using System.Drawing;
using QuadTrees.QTreeRect;

namespace TagCloud;

public class RectWrapper : IRectQuadStorable
{
    public RectWrapper(Rectangle rect)
    {
        Rect = rect;
    }

    public Rectangle Rect { get; }
}