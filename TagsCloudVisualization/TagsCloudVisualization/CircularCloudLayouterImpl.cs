using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouterImpl : ICircularCloudLayouter
{
    private static readonly float TracingStep = 0.001f;
    private static readonly float MaxTracingDistance = 1000f;
    private static readonly int MaxCycleCount = 7;

    public Point CloudCenter
    {
        get => _cloudCenter;
        set
        {
            if (_generatedLayout.Count > 0)
            {
                throw new InvalidOperationException("Can not change cloud center after generation start");
            }
            _cloudCenter = value;
        }
    }
    
    public IEnumerable<Rectangle> Layout => _generatedLayout.AsEnumerable();

    private Point _cloudCenter;
    private List<Rectangle> _generatedLayout = new();
    private double _nextAngle;
    private double _angleStep = Math.PI / 2;
    private int _currentCycle;
    private int CurrentCycle
    {
        get => _currentCycle;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            if (value < MaxCycleCount)
            {
                _currentCycle = value;
                _angleStep = Math.PI / Math.Pow(2, value);
            }
        }
    }
    
    public CircularCloudLayouterImpl(Point center)
    {
        CloudCenter = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width, "Width");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height, "Height");
        
        if (_generatedLayout.Count == 0)
        {
            var rectangle = new Rectangle(
                CloudCenter.X - rectangleSize.Width / 2,
                CloudCenter.Y - rectangleSize.Height / 2,
                rectangleSize.Width,
                rectangleSize.Height);
            _generatedLayout.Add(rectangle);
            return rectangle;
        }

        var nextRectangle = GetNextRectangle(rectangleSize);
        _generatedLayout.Add(nextRectangle);
        return nextRectangle;
    }

    private Rectangle GetNextRectangle(Size rectangleSize)
    {
        Rectangle result = Rectangle.Empty;
        var found = false;
        while (!found)
        {
            var direction = GetNextDirection();
            var step = 0.0f;
            while (step < 1f && !found)
            {
                (step, var availablePos) = FindNextAvailablePosByTracingLine(direction, step);
                found = TryFindGoodRectanglePosition(availablePos, rectangleSize, out result);
            }
        }

        return result;
    }

    private bool TryFindGoodRectanglePosition(Point posToPlace, Size rectangleSize, out Rectangle result)
    {
        var possibleOptions = new Rectangle[]
        {
            new Rectangle(
                posToPlace.X,
                posToPlace.Y,
                rectangleSize.Width,
                rectangleSize.Height),
            new Rectangle(
                posToPlace.X - rectangleSize.Width,
                posToPlace.Y,
                rectangleSize.Width,
                rectangleSize.Height),
            new Rectangle(
                posToPlace.X,
                posToPlace.Y - rectangleSize.Height,
                rectangleSize.Width,
                rectangleSize.Height),
            new Rectangle(
                posToPlace.X - rectangleSize.Width,
                posToPlace.Y - rectangleSize.Height,
                rectangleSize.Width,
                rectangleSize.Height)
        };
        
        foreach (var option in possibleOptions)
        {
            bool intersects = false;
            foreach (var rectangle in _generatedLayout)
            {
                if (rectangle.IntersectsWith(option))
                {
                    intersects = true;
                    break;
                }
            }

            if (!intersects)
            {
                result = option;
                return true;
            }
        }
        
        result = Rectangle.Empty;
        return false;
    }

    private (float, Point) FindNextAvailablePosByTracingLine(PointF direction, float startingStep = 0.0f)
    {
        var nextPos = new PointF(
            CloudCenter.X + direction.X * MaxTracingDistance * TracingStep,
            CloudCenter.Y + direction.Y * MaxTracingDistance * TracingStep);
        var currentStep = startingStep == 0.0f ? TracingStep : startingStep;
        var notInRectangle = false;
        while (!notInRectangle)
        {
            notInRectangle = true;
            foreach (var rectangle in _generatedLayout)
            {
                if (rectangle.ContainsFloat(nextPos))
                {
                    notInRectangle = false;
                    break;
                }
            }
            currentStep += TracingStep;
            nextPos = new PointF(
                CloudCenter.X + direction.X * MaxTracingDistance * currentStep,
                CloudCenter.Y + direction.Y * MaxTracingDistance * currentStep);
        }

        return (currentStep, Point.Truncate(nextPos));
    }

    private PointF GetNextDirection()
    {
        var x = (float)Math.Cos(_nextAngle);
        var y = (float)Math.Sin(_nextAngle);
        _nextAngle += _angleStep;
        if (Math.Abs(_nextAngle - Math.PI * 2) < 1e-12f)
        {
            _nextAngle = 0;
            CurrentCycle++;
        }
        return new PointF(x, y);
    }
}