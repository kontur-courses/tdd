using System.Collections.Immutable;
using System.Drawing;
using QuadTrees;

namespace TagCloud;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly QuadTreeRect<RectWrapper> rectanglesQuadTree;
    private readonly IEnumerator<Point> spiralEnumerator;

    public CircularCloudLayouter(Point center, double spiralExpansionStep = 0.01, double spiralTwistStep = 0.01)
    {
        Center = center;
        spiralEnumerator = new SpiralPointGenerator(spiralExpansionStep, spiralTwistStep)
            .Generate(center).GetEnumerator();
        rectanglesQuadTree = new QuadTreeRect<RectWrapper>(
            int.MinValue / 2 + center.X, int.MinValue / 2 + center.Y,
            int.MaxValue, int.MaxValue);
    }

    public Point Center { get; }

    public ImmutableArray<Rectangle> Rectangles => rectanglesQuadTree
        .GetAllObjects()
        .Select(rw => rw.Rect).ToImmutableArray();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException($"Width and height of the rectangle must be positive, but {rectangleSize}");

        var rectangle = CreateNewRectangle(rectangleSize);
        while (rectanglesQuadTree.GetObjects(rectangle).Any())
            rectangle = CreateNewRectangle(rectangleSize);

        rectanglesQuadTree.Add(new RectWrapper(rectangle));

        return rectangle;
    }

    private Rectangle CreateNewRectangle(Size size)
    {
        spiralEnumerator.MoveNext();
        var point = new Point(spiralEnumerator.Current.X - size.Width / 2,
            spiralEnumerator.Current.Y - size.Height / 2);
        return new Rectangle(point, size);
    }
}