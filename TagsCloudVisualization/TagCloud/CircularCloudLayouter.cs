using System.Drawing;
using TagsCloudVisualization;

namespace TagCloud;

public class CircularCloudLayouter
{
    public readonly Point Center;
    private readonly List<Rectangle> rectangles = new List<Rectangle>();
    public IReadOnlyList<Rectangle> Rectangles => rectangles;
    private PolarPoint CurrentPosition { get; set; }
    private List<(PolarPoint, PolarPoint)> unusedRanges = new List<ValueTuple<PolarPoint, PolarPoint>> ();
    public double Density { get; private set; }
    public double AngleStep { get; private set; }

    public CircularCloudLayouter(Point center, double density = 0.01, double angleStep = 0.01)
    {
        Center = center;
        Density = density;
        AngleStep = angleStep;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size can't be negative");

        foreach (var polarPoint in GetAllFreeRanges())
        {
            if (TryAddRectangle(polarPoint, rectangleSize))
                return rectangles.Last();
        }
        
        throw new Exception("There is no place for the rectangle");
    }

    private bool TryAddRectangle(PolarPoint pointInPolar, Size rectangleSize)
    {
        var rectangleCenter = (Point)pointInPolar;
        var position = new Point(Center.X + rectangleCenter.X - rectangleSize.Width / 2, 
            Center.Y + rectangleCenter.Y - rectangleSize.Height / 2);

        var rectangle = new Rectangle(position, rectangleSize);
        if (HasOverlapWith(rectangle))
            return false;
        
        rectangles.Add(rectangle);
        CurrentPosition = new PolarPoint(pointInPolar.Radius, pointInPolar.Angle);
        RemoveOverlapedFromUnused();
        return true;
    }
    
    private IEnumerable<PolarPoint> GetAllFreeRanges()
    {
        return new List<IEnumerable<PolarPoint>>
            { 
                unusedRanges.SelectMany(range => GenerateArchimedeanSpiralRadius(range.Item1, range.Item2, 0, Density, AngleStep)),
                GenerateArchimedeanSpiralRadius(CurrentPosition, null, 0, Density, AngleStep)
            }.SelectMany(x => x);
    }

    private void RemoveOverlapedFromUnused()
    {
        var newRanges = new List<ValueTuple<PolarPoint, PolarPoint>> ();
        
        var isLastOverlaped = true;
        PolarPoint start = default, end = default;
        foreach (var range in unusedRanges)
        {
            foreach (var polarPoint in GenerateArchimedeanSpiralRadius(range.Item1, range.Item2, 0, Density, AngleStep))
            {
                var point = (Point)polarPoint;
                if (HasOverlapWith(new Rectangle(point.X - 1, point.Y - 1, 2, 2)))
                {
                    if (!isLastOverlaped)
                        newRanges.Add((start, end));
                    isLastOverlaped = true;
                }
                else
                {
                    if (isLastOverlaped)
                        start = polarPoint;
                    else
                        end = polarPoint;
                    isLastOverlaped = false;
                }
            }
        }

        unusedRanges = newRanges;
    }
    
    public bool HasOverlapWith(Rectangle rectangle)
    {
        foreach(var existingRectangle in rectangles)
        {
            if (existingRectangle.IntersectsWith(rectangle))
                return true;
        }

        return false;
    }

    private static IEnumerable<PolarPoint> GenerateArchimedeanSpiralRadius(PolarPoint start, PolarPoint? end, double offset, double density , double angleStep)
    {
        /** Archimedean Spiral  
         * Formula: r = a + b * θ,
         * a – move start by OX axis, 
         * b – curve density, 
         * θ – angle in polar system, 
         * r – radius in polar system
         */
        end ??= new PolarPoint(int.MaxValue / 2, 0);
        
        var nextAngle = start.Angle;
        var nextRadius = start.Radius;
        while (nextRadius < end.Value.Radius)
        {
            yield return new PolarPoint(nextRadius, nextAngle);
            nextRadius = offset + density * nextAngle;
            nextAngle += angleStep;
        }
    }

    public void Clear()
    {
        rectangles.Clear();
        unusedRanges.Clear();
        CurrentPosition = new PolarPoint();
    }
}