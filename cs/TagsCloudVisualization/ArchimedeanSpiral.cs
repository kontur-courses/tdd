namespace TagsCloudVisualization;

public class ArchimedeanSpiral
{
    public readonly double K;

    public ArchimedeanSpiral(double k)
    {
        K = k;
    }

    public static double GetX(double rho, double phi) => rho * Math.Cos(phi);

    public static double GetY(double rho, double phi) => rho * Math.Sin(phi);

    public double GetPhi(double rho) => rho / K;

    public double GetRho(double phi) => phi * K;
}
