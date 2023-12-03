using System.Drawing;
using System.IO;

namespace TagsCloudVisualization;

public class Program
{
    public const int ImageWidth = 1000;
    public const int ImageHeight = 1000;
    public const int CountRectangles = 500;
    public const string PathToImages = @"..\..\..\Images";

    public static void Main(string[] args)
    {
        var layouter = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));
        var random = new Random();

        for (int i = 0; i < CountRectangles; i++)
        {
            layouter.PutNextRectangle(new Size(random.Next(10, 20), random.Next(10, 20)));
        }
        var image = Visualizer.Visualize(layouter.Rectangles, ImageWidth, ImageHeight);

        Visualizer.SaveBitmap(image, $"Result{CountRectangles}Rectangles.png", PathToImages);
    }
}