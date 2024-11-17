using System.Drawing;

namespace TagsCloudVisualization;

public class LayoutGenerator
{
    private readonly Point center;
    private readonly Size imageSize;
    private readonly CloudImageRenderer renderer;

    public LayoutGenerator(Point center, Size imageSize)
    {
        this.center = center;
        this.imageSize = imageSize; 
        renderer = new CloudImageRenderer(this.imageSize);
    }

    public void GenerateLayout(string outputFileName, int rectangleCount, Func<Random, Size> sizeGenerator)
    {
        var layouter = new CircularCloudLayouter(center);
        var random = new Random();

        for (var i = 0; i < rectangleCount; i++)
        {
            var size = sizeGenerator(random);
            layouter.PutNextRectangle(size);
        }

        var rectangles = layouter.GetRectangles();
        renderer.SaveToFile(outputFileName, rectangles);
    }
}