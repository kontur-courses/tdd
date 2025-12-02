using System.Drawing;

namespace TagsCloudVisualization;

public static class PaletteGenerator
{
    private const float Epsilon = 0.0001f;
    private const int MaxHueDegrees = 360;
    private const float MaxRgbValue = 255f;
    private const float OneThird = 1f / 3f;
    private const float OneSixth = 1f / 6f;
    private const float TwoThirds = 2f / 3f;
    private const float OneHalf = 1f / 2f;
    private const int DefaultHueVariation = 20;
    private const float MinSaturationFactor = 0.6f;
    private const float SaturationRange = 1.0f - 0.6f;
    private const float MinLightness = 0.3f;
    private const float LightnessRange = 0.8f - 0.3f;

public static Color[] GenerateColorPalette(int count, Color baseColor)
{
    if (count <= 0)
        return [];

    var colors = new Color[count];
    var rnd = Random.Shared;
    
    var (hue, saturation, _) = RgbToHsl(baseColor);
    
    for (var i = 0; i < count; i++)
    {
        var variedHue = hue + rnd.Next(-DefaultHueVariation, 
                                        DefaultHueVariation + 1);
        variedHue = NormalizeHue(variedHue);
        
        var variedSaturation = saturation * (MinSaturationFactor + 
                                            rnd.NextSingle() * SaturationRange);
        variedSaturation = Math.Clamp(variedSaturation, 0f, 1f);
        
        var variedLightness = MinLightness + 
                             rnd.NextSingle() * LightnessRange;
        
        colors[i] = HslToRgb(variedHue, variedSaturation, variedLightness);
    }
    
    return colors;
}

private static (float Hue, float Saturation, float Ligtness) RgbToHsl(Color color)
{
    var r = color.R / MaxRgbValue;
    var g = color.G / MaxRgbValue;
    var b = color.B / MaxRgbValue;

    var max = Math.Max(r, Math.Max(g, b));
    var min = Math.Min(r, Math.Min(g, b));
    
    var hue = 0f;
    var saturation = 0f;
    var lightness = (max + min) / 2f;

    if (Math.Abs(max - min) < Epsilon)
        return (hue * MaxHueDegrees, saturation, lightness);
    
    var delta = max - min;
    saturation = lightness > 0.5f ? 
        delta / (2f - max - min) : 
        delta / (max + min);
    
    if (Math.Abs(max - r) < Epsilon)
        hue = (g - b) / delta + (g < b ? 6f : 0f);
    else if (Math.Abs(max - g) < Epsilon)
        hue = (b - r) / delta + 2f;
    else if (Math.Abs(max - b) < Epsilon)
        hue = (r - g) / delta + 4f;
        
    hue /= 6f;

    return (hue * MaxHueDegrees, saturation, lightness);
}

private static Color HslToRgb(float hue, float saturation, float lightness)
{
    hue /= MaxHueDegrees;
    
    var q = lightness < 0.5f ? 
        lightness * (1f + saturation) : 
        lightness + saturation - lightness * saturation;
    
    var p = 2f * lightness - q;

    var red = HueToRgb(p, q, hue + OneThird);
    var green = HueToRgb(p, q, hue);
    var blue = HueToRgb(p, q, hue - OneThird);

    return Color.FromArgb(
        (int)(red * MaxRgbValue),
        (int)(green * MaxRgbValue),
        (int)(blue * MaxRgbValue));
}

private static float HueToRgb(float p, float q, float t)
{
    if (t < 0f) t += 1f;
    if (t > 1f) t -= 1f;

    return t switch
    {
        < OneSixth => p + (q - p) * 6f * t,
        < OneHalf => q,
        < TwoThirds => p + (q - p) * (TwoThirds - t) * 6f,
        _ => p
    };
}

private static float NormalizeHue(float hue)
{
    hue %= MaxHueDegrees;
    return hue < 0 ? hue + MaxHueDegrees : hue;
}
}