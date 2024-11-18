using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization;

internal class Program
{
    private const int ImageWidth = 1920;
    private const int ImageHeight = 1080;

    private const int RectanglesNumber = 100;

    private const string ImagesDirectory = "images";

    public static void Main(string[] args)
    {
        var center = new Point(ImageWidth / 2, ImageHeight / 2);
        var cloudLayouter = new CircularCloudLayouter(center);

        var rectangles = cloudLayouter.GenerateCloud(RectanglesNumber);

        var visualizer = new Visualizer();
        var bitmap = visualizer.CreateBitmap(rectangles, new Size(ImageWidth, ImageHeight));
        Directory.CreateDirectory(ImagesDirectory);

        bitmap.Save(GetPathToImages(), ImageFormat.Jpeg);
    }

    private static string GetPathToImages()
    {
        var filename = $"{RectanglesNumber}_TagCloud.jpg";
        return Path.Combine(ImagesDirectory, filename);
    }

}