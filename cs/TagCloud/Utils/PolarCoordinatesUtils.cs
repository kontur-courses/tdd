namespace TagCloud.Utils;

public static class PolarCoordinatesUtils
{
    public static (float x, float y) ConvertPolarCoordsToCartesian(float radius, float degree)
    {
        var x = radius * MathF.Cos(degree);
        var y = radius * MathF.Sin(degree);
        return (x, y);
    }
}