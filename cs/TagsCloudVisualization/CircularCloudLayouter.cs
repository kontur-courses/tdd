using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Layout layout;

    public CircularCloudLayouter(Point center)
    {
        layout = new Layout(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return AddRectangle(rectangleSize);
    }

    private Rectangle AddRectangle(Size size)
    {
        if (size.Width <= 0 || size.Height <= 0)
            throw new ArgumentException("Width and height of rectangle must be positive");

        var offsetBeforeCenter = new Size(
            size.Width / 2 + (size.Width % 2 == 0 ? 0 : 1),
            size.Height / 2 + (size.Height % 2 == 0 ? 0 : 1));

        var result = default(Rectangle);

        foreach (var coord in layout.GetNextCoord())
        {
            result = new Rectangle(coord - offsetBeforeCenter, size);
            
            if (!layout.CanPutRectangle(result))
                continue;

            layout.PutRectangle(result);
            break;
        }

        return result;
    }
}