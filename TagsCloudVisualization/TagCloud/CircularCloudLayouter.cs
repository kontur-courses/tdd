using System.ComponentModel;
using System.Drawing;
using TagsCloudVisualization;

namespace TagCloud;

public class CircularCloudLayouter
{
    public readonly Point Center;
    private readonly List<Rectangle> rectangles;
    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    /// <summary>
    /// Density check step. Applies only to the next added rectangles
    /// </summary>
    public double Density { get; set; }

    /// <summary>
    /// Angle check step. Applies only to the next added rectangles
    /// </summary>
    public double AngleStep { get; set; }

    public CircularCloudLayouter(Point center)
    {
        rectangles = new();
        Center = center;
        Density = 0.01;
        AngleStep = 0.01;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size can't be negative");

        
        foreach (var pointInPolar in GenerateArchimedeanSpiralRadiuses(0, Density, AngleStep))
        {
            var rectangleCenter = (Point)pointInPolar;
            var position = new Point(Center.X + rectangleCenter.X - rectangleSize.Width / 2, 
                                     Center.Y + rectangleCenter.Y - rectangleSize.Height / 2);

            var rectangle = new Rectangle(position, rectangleSize);
            if (!HasOverlapWith(rectangle))
            {
                rectangles.Add(rectangle);
                return rectangle;
            }
        }
        throw new Exception("There is no place for the rectangle");
    }

    public bool HasOverlapWith(Rectangle rectangle)
    {
        return rectangles.Any(x => x.IntersectsWith(rectangle));
    }

    private static IEnumerable<PolarPoint> GenerateArchimedeanSpiralRadiuses(double offset, double density , double angleStep)
    {
        /** Archimedean Spiral  
         * Formula: r = a + b * θ,
         * a – move start by OX axis, 
         * b – curve density, 
         * θ – angle in polar system, 
         * r – radius in polar system
         */
        var nextAngle = 0.0;
        var nextRadius = 0.0;
        while (nextRadius < int.MaxValue / 2)
        {
            yield return new PolarPoint(nextRadius, nextAngle);
            nextRadius = offset + density * nextAngle;
            nextAngle += angleStep;
        }
    }

    public void Clear()
    {
        rectangles.Clear();
    }
}