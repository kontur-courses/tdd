using System.Drawing;

namespace TagCloudVisualization.Layout;

public class CloudCompactor(Point center, RectangleCollisionIndex index)
{
    public Rectangle MoveTowardsCenter(Rectangle rect)
    {
        bool moved;
        do
        {
            moved = TryShift(ref rect, dx: Math.Sign(center.X - Center(rect).X), dy: 0)
                    | TryShift(ref rect, dx: 0, dy: Math.Sign(center.Y - Center(rect).Y));
        } while (moved);
        return rect;
    }

    private bool TryShift(ref Rectangle rect, int dx, int dy)
    {
        throw new NotImplementedException();
    }

    private Point Center(Rectangle rect) 
        => new(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
}