using System.Drawing;

namespace TagsCloudVisualization;

public class CloudVisualizer
{
    private readonly ImageCreator creator;
    private readonly CircularCloudLayouter layouter;

    public CloudVisualizer(ImageCreator creator, CircularCloudLayouter layouter)
    {
        this.creator = creator;
        this.layouter = layouter;
    }

    public void CreateAndSave(int countRectangles, int countImage, string currentDirectoryPath)
    {
        for (var i = 0; i < countImage; i++)
        {
            var rectangles = PutRectangles(countRectangles);
            var image = creator.CreateImage(rectangles);
            var imageName = $"cloud_{i + 1}_with_{countRectangles}_rectangles";
            ImageSaver.Save($"{currentDirectoryPath}/Images/{imageName}.png", image);
            layouter.Reset();
        }
    }

    private List<Rectangle> PutRectangles(int countRectangles)
    {
        var random = new Random();
        var result = new List<Rectangle>();
        for (var i = 0; i < countRectangles; i++)
        {
            var width = random.Next(5, 20);
            var height = random.Next(5, 20);
            result.Add(layouter.PutNextRectangle(new Size(width, height)));
        }

        return result;
    }
}