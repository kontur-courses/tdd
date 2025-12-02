using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Sample;

public static class Program
{
    public static void Main()
    {
        const int width = 400;
        const int height = 400;

        using var image = new Image<Rgba32>(width, height, Color.White);

        const int centerX = width / 2;
        const int centerY = height / 2;
        const int radius = 100;
        
        var circleColor = new Rgba32(255, 0, 0, 255);

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var dx = x - centerX;
            var dy = y - centerY;
            if (dx * dx + dy * dy <= radius * radius)
                image[x, y] = circleColor;
        }
        
        var projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..")
        );

        var outputPath = Path.Combine(projectRoot, "sample.png");
        image.Save(outputPath);
    }
}