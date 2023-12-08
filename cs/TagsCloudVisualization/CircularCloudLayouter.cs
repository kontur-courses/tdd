using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> placedWords = new();
    private readonly Point center;
    private readonly Size resolution;
    public int AllRectanglesArea { get; private set; }
    public double MaxRadius { get; private set; }

    public CircularCloudLayouter()
    {
        center = new Point(960, 540);
        resolution = new Size(1920, 1080);
    }

    public CircularCloudLayouter(Size resolution)
    {
        this.resolution = resolution;
        center = new Point(resolution.Width / 2, resolution.Height / 2);
    }

    public CircularCloudLayouter(Point center, Size resolution)
    {
        this.resolution = resolution;
        this.center = center;
    }

    public int GetCloudArea()
    {
        return placedWords.Sum(rectangle => rectangle.Height * rectangle.Width);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var coordinate in GetPoints(rectangleSize.Height))
        {
            var target = new Rectangle(coordinate, rectangleSize);

            if (!IntersectWithPlaced(target))
            {
                placedWords.Add(target);

                MaxRadius = Math.Max(MaxRadius, MathWithPoints.DistanceToCenter(coordinate, center));
                AllRectanglesArea += rectangleSize.Height * rectangleSize.Width;

                return target;
            }
        }

        return default;
    }

    private IEnumerable<Point> GetPoints(int fontSize)
    {
        var rnd = new Random();
        // TODO не ограничиваться сверху и делать downscale
        for (var radius = 0; radius < Math.Min(resolution.Width, resolution.Height); radius += fontSize)
        {
            // Угол рандомизирую для менее детерминированного распределения, i - счётчик для поворота только на 360
            var i = 0;
            for (var angle = rnd.Next(360); i < 360; angle++, i++)
                yield return MathWithPoints.PolarToCartesian(radius, angle, center);
        }
    }

    private bool IntersectWithPlaced(Rectangle target)
    {
        return placedWords.Any(target.IntersectsWith);
    }
}