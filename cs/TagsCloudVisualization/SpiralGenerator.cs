using System.Drawing;

namespace TagsCloudVisualization;

public class SpiralGenerator
{
    private readonly Point center;
    private double phi;
    private readonly double spiralStep;
    private readonly double deltaPhi;
    private Point? lastGeneratedPoint;

    public SpiralGenerator(Point center, double spiralStep = 1, double deltaPhiDegrees = (Math.PI / 180))
    {
        if (spiralStep <= 0)
            throw new ArgumentException("Шаг спирали должен быть больше 0");

        this.center = center;
        phi = 0;
        this.spiralStep = spiralStep;
        deltaPhi = deltaPhiDegrees;
    }

    public Point GetNextPoint()
    {
        Point newPoint;

        do
        {
            var radius = spiralStep * phi;
            var x = (int)Math.Round(radius * Math.Cos(phi)) + center.X;
            var y = (int)Math.Round(radius * Math.Sin(phi)) + center.Y;
            phi += deltaPhi;

            newPoint = new Point(x, y);
        } while (newPoint == lastGeneratedPoint);

        lastGeneratedPoint = newPoint;
        return newPoint;
    }
}