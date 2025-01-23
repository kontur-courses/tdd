using SixLabors.ImageSharp;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization;

class RandomVisualisation
{
    static void Main(string[] args)
    {

        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName 
                               ?? throw new InvalidOperationException("Не удалось определить директорию проекта.");
        var imagesDirectory = Path.Combine(projectDirectory, "Images");

        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }
        var layouter = new CircularCloudLayouter(new PointF(600, 450));
        var random = new Random();
        var visualizer = new CloudVisualizer();

        var numberOfRectangles = 150;
        for (int i = 0; i < numberOfRectangles; i++)
        {
            var width = 35;
            var height = 20;
            layouter.PutNextRectangle(new SizeF(width, height));
        }
        visualizer.SaveVisualization(layouter.Rectangles, Path.Combine(imagesDirectory, $"tag_cloud_equals_rectangles.png"));

        for (int j = 1; j <= 3; j++)
        {
            layouter = new CircularCloudLayouter(new PointF(600, 450));
            for (int i = 0; i < numberOfRectangles; i++)
            {
                var width = random.Next(25, 50);
                var height = random.Next(25, 50);
                layouter.PutNextRectangle(new SizeF(width, height));
            }
            visualizer.SaveVisualization(layouter.Rectangles, Path.Combine(imagesDirectory, $"tag_cloud_random_{j}.png"));
        }
        Console.WriteLine("Визуализация была сохранена.");
    }
}