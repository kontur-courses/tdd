using System.Drawing;
using TagCloud.Creators;
using TagCloud.Shapes;
using TagCloud.SpiralGenerators;

namespace TagCloud;
public class CircularCloudLayouter(
    Point center, 
    ISpiralPointGenerator generator,  
    ICloudShapeCreator shapeCreator)
{
    private readonly List<ICloudShape> _shapes = [];

    public IReadOnlyCollection<ICloudShape> Shapes => _shapes.AsReadOnly();
    
    public ICloudShape PutNextShape(Size size)
    {
        if (size.Width <= 0 || size.Height <= 0) 
            throw new ArgumentException("Shape size must be greater than zero");
        
        ICloudShape shape;

        if (_shapes.Count == 0)
            shape = shapeCreator.Create(center, size);
        else
        {
            shape = FindPlaceForShape(size);
            shape = ShiftShapeToCenter(shape);
        }

        _shapes.Add(shape);
        return shape;
    }
    
    private ICloudShape FindPlaceForShape(Size size)
    {
        while (true)
        {
            var pointOnSpiral = generator.GetNextPointOnSpiral();
            var newShape = shapeCreator.Create(pointOnSpiral, size);

            if (!_shapes.Any(s => s.IntersectsWith(newShape)))
                return newShape;
        }
    }

    private ICloudShape ShiftShapeToCenter(ICloudShape shape)
    {
        while (true)
        {
            var direction = GetDirectionToCenter(shape);

            if (direction == Point.Empty)
                return shape;

            var shifted = shape.Shift(direction.X, direction.Y);

            if (_shapes.Any(s => s.IntersectsWith(shifted)))
                return shape;

            shape = shifted;
        }
    }

    private Point GetDirectionToCenter(ICloudShape shape)
    {
        var shapeCenter = shape.Center;

        var dx = shapeCenter.X > center.X ? -1 : shapeCenter.X < center.X ? 1 : 0;
        var dy = shapeCenter.Y > center.Y ? -1 : shapeCenter.Y < center.Y ? 1 : 0;

        return new Point(dx, dy);
    }
}