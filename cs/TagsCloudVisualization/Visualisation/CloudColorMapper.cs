using SixLabors.ImageSharp.PixelFormats;
namespace TagCloud.Visualisation;

internal static class CloudColorMapper
{
    public static Rgba32 ToRgba32(this CloudColor color) =>
        color switch
        {
            CloudColor.White    => new Rgba32(255, 255, 255, 255),
            CloudColor.Black    => new Rgba32(0,   0,   0,   255),
            CloudColor.Gray     => new Rgba32(200, 200, 200, 255),
            CloudColor.DarkGray => new Rgba32(100, 100, 100, 255),
            CloudColor.Red      => new Rgba32(255, 0,   0,   255),
            CloudColor.Green    => new Rgba32(0,   255, 0,   255),
            CloudColor.Blue     => new Rgba32(0,   0,   255, 255),
            _                   => new Rgba32(255, 255, 255, 255)
        };
}