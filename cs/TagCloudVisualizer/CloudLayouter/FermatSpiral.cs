using System.Drawing;

namespace TagCloudVisualizer.CloudLayouter;

public class FermatSpiral
{
    private readonly int step;
    private readonly Point center;
    private int phi;
    private int a;

    public FermatSpiral(Point center, int a = 1, int step = 1)
    {
        this.a = a;
        this.step = step;
        this.center = center;
        phi = 0;
    }

    public Point GetNextPoint()
    {
        if (phi == 0)
        {
            phi += step;
            return center;
        }
        
        var result = new Point((int)(a * Math.Sqrt(phi) * Math.Cos(phi) + center.X),
            (int)(a * Math.Sqrt(phi) * Math.Sin(phi)) + center.Y);

        if (a < 0)
            phi += step;
        a = -a;
        return result;
    }
}