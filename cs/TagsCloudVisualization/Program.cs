using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class Program
{
    public static void Main(string[] args)
    {
        var cloudLayouter = CreateCloudLayouter();
        var random = new Random();
        for (var i = 0; i < 100; i++)
        {
            cloudLayouter.PutNextRectangle(new Size(random.Next(50, 100), random.Next(10, 50)));
        }

        using var bitmap = CloudImageGenerator.Generate(cloudLayouter);
        CloudImageSaver.Save(bitmap, "Images", "tag-cloud-100-first-quarter.bmp");
    }

    private static ICloudLayouter CreateCloudLayouter()
    {
        return new CircularCloudLayouter(new Point(500, 200));
    }
}