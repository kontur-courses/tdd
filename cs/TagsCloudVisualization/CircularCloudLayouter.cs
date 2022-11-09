using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;

    private readonly Rectangle viewBoard;
    
    private readonly List<Rectangle> rectangles = new();

    public CircularCloudLayouter(Point center)
    {
        if (center.IsEmpty || center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("center point is invalid", nameof(center));
        this.center = center;
        viewBoard = new Rectangle(Point.Empty, new Size(center));
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width > viewBoard.Width || rectangleSize.Height > viewBoard.Height)
            throw new ArgumentException("rectangleSize is too huge", nameof(rectangleSize));

        if (rectangles.Count == 0)
        {
            var position = new Point(center.X - rectangleSize.Width, center.Y - rectangleSize.Height);
            var rectangle = new Rectangle(position, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        throw new NotImplementedException();
    }
}