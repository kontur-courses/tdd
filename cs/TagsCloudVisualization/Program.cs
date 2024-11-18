using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.SpiralLayouter;
using TagsCloudVisualization.SpiralLayouter.PointGenerator;

namespace TagsCloudVisualization;

internal class Program
{
    private const int IMAGE_WIDTH = 1920;
    private const int IMAGE_HEIGHT = 1080;

    private const int RECTANGLE_COUNT = 100;
    private const int MAX_RECT_WIDTH = 75;
    private const int MAX_RECT_HEIGHT = 75;

    private const string MEDIA_ROOT = "images";
    
    public static void Main(string[] args)
    {
        var random = new Random();
        
        var center = new Point(IMAGE_WIDTH / 2, IMAGE_HEIGHT / 2);
        var spiralLayouter = new SpiralCloudLayouter(center, new PolarArchimedesSpiral(1, 1));
        
        var rectangleSizes = new Size[RECTANGLE_COUNT]
                .Select(_ => (random.Next(5, MAX_RECT_WIDTH), random.Next(5, MAX_RECT_HEIGHT)))
                .Select(rawSize => new Size(rawSize.Item1, rawSize.Item2))
                .ToArray();
        var imageSize = new Size(IMAGE_WIDTH, IMAGE_HEIGHT);
        
        var bitmap = BitmapGenerator.GenerateWindowsBitmap(spiralLayouter, imageSize, rectangleSizes);
        var saver = new BitmapSaver(MEDIA_ROOT);
        saver.SaveBitmap(bitmap, $"{RECTANGLE_COUNT}.jpeg");
    }
}