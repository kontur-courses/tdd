using System.Drawing;
using TagsCloudVizualization.Utility;

namespace TagsCloudVizualization;

public class Program
{
    public const int ImageWidth = 800;
    public const int ImageHeight = 800;
    public const int CountRectangles = 122;
    public const string PathToImages = @"..\..\..\Images";

    public static void Main(string[] args)
    {
        var layouter = CreateLayouter();
        AddRandomRectangles(layouter);

        var image = VisualizeLayout(layouter);

        SaveImage(image);
    }

    private static CircularCloudLayouter CreateLayouter()
    {
        var center = new Point(ImageWidth / 2, ImageHeight / 2);
        var spiral = new Spiral(center, 0.02, 0.01);
        return new CircularCloudLayouter(center, spiral);
    }

    private static void AddRandomRectangles(CircularCloudLayouter layouter)
    {
        var random = new Random();
        for (int i = 0; i < CountRectangles; i++)
        {
            var rectangleSize = new Size(random.Next(5, 20), random.Next(5, 20));
            layouter.PutNextRectangle(rectangleSize);
        }
    }

    private static Bitmap VisualizeLayout(CircularCloudLayouter layouter)
    {
        return Visualizer.VisualizeRectangles(layouter.Rectangles, ImageWidth, ImageHeight);
    }

    private static void SaveImage(Bitmap image)
    {
        var fileName = $"{CountRectangles}_Rectangles.png";
        Visualizer.SaveBitmap(image, fileName, PathToImages);
    }
}