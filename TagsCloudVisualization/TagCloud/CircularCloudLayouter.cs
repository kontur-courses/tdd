using System.ComponentModel;
using System.Drawing;
namespace TagCloud;

public class CircularCloudLayouter
{
    public readonly Point Crenter;
    public List<Rectangle> Rectangles { get; private set; }

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
        Rectangles = new();
        Crenter = center;
        Density = 0.01;
        AngleStep = 0.01;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size can't be negative");

        var rectangle = new Rectangle();
        foreach (var pointInPolar in GenerateArchimedeanSpiralRadiuses(0, Density, AngleStep))
        {
            var rectangleCenter = ToCartesianCoordinate(pointInPolar.Item1, pointInPolar.Item2);
            var position = new Point(Crenter.X + rectangleCenter.X - rectangleSize.Width / 2, 
                                     Crenter.Y + rectangleCenter.Y - rectangleSize.Height / 2);
            rectangle = new Rectangle(position, rectangleSize);
            if (!HasOverlapWith(rectangle))
            {
                Rectangles.Add(rectangle);
                break;
            }
        }
        return rectangle;
    }

    public bool HasOverlapWith(Rectangle rectangle)
    {
        return Rectangles.Any(x => x.IntersectsWith(rectangle));
    }

    private static IEnumerable<Tuple<double, double>> GenerateArchimedeanSpiralRadiuses(double offset, double density , double angleStep)
    {
        /** Archimedean Spiral  
         * Formula: r = a + b * θ,
         * a – move start by OX axis, 
         * b – curve density, 
         * θ – angle in polar system, 
         * r – radius in polar system
         */
        var angle = 0.0;
        var nextRadius = 0.0;
        while (!double.IsInfinity(nextRadius))
        {
            yield return Tuple.Create(nextRadius, angle);
            nextRadius = offset + density * angle;
            angle += angleStep;
        }
    }

    private static Point ToCartesianCoordinate(double radius, double angle)
    {
        var x = (int)Math.Round(radius * Math.Cos(angle));
        var y = (int)Math.Round(radius * Math.Sin(angle));
        return new Point(x, y);
    }
}