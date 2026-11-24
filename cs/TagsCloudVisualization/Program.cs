using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization;

public class Program
{
    public static void Main(string[] args)
    {
        var directory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Images"));

        Directory.CreateDirectory(directory);

        var renderer = new CloudRenderer(
            padding: 10,
            pen: Pens.Black,
            brush: Brushes.LightGray,
            backgroundColor: Color.White);

        var random = new Random(Seed: 1);

        // 100 одинаковых квадратов
        GenerateCloud(directory, "100squares.png", renderer,
            Enumerable.Repeat(new Size(40, 40), 100));

        // 300 прямоугольников случайных размеров
        GenerateCloud(directory, "300rectangles.png", renderer,
            Enumerable.Range(0, 300)
                .Select(_ => new Size(
                    random.Next(20, 100),
                    random.Next(15, 50))));

        // 1000 прямоугольников случайных размеров
        GenerateCloud(directory, "1000rectangles.png", renderer,
            Enumerable.Range(0, 1000)
                .Select(_ => new Size(
                    random.Next(20, 100),
                    random.Next(15, 50))));
    }

    private static void GenerateCloud(
        string directory,
        string fileName,
        CloudRenderer renderer,
        IEnumerable<Size> sizes)
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));

        foreach (var size in sizes)
            layouter.PutNextRectangle(size);

        using var image = renderer.Render(layouter.Rectangles);

        var path = Path.Combine(directory, fileName);
        image.Save(path, ImageFormat.Png);
    }
}