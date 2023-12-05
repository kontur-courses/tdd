using System.Drawing;

namespace TagsCloudVizualization;

public class Program
{
    public const int ImageWidth = 720;
    public const int ImageHeight = 720;
    public const int CountRectangles = 10000;
    public const string PathToImages = @"..\..\..\Images";

    public static void Main(string[] args)
    {
        var layouter = CreateLayouter();
        GenerateRandomRectangles(layouter);

        var image = VisualizeLayout(layouter);

        SaveImage(image);
    }

    private static CircularCloudLayouter CreateLayouter()
    {
        var center = new Point(ImageWidth / 2, ImageHeight / 2);
        return new CircularCloudLayouter(center);
    }

    private static void GenerateRandomRectangles(CircularCloudLayouter layouter)
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
