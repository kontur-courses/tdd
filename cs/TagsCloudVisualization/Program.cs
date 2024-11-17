using System.Drawing;
using TagsCloudVisualization.Visualizers;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization;

public static class Program
{
    private const int imageWidth = 1500;
    private const int imageHeight = 1500;

    public static void Main()
    {
        var imageSize = new Size(imageWidth, imageHeight);
        var center = new Point(imageSize.Width / 2, imageSize.Height / 2);
        var layouter = new CircularCloudLayouter(center);
        layouter.GenerateCloud();
        var rectangles = layouter.GeneratedRectangles;
        var visualizer = new SimpleCloudVisualizer();
        visualizer.CreateBitmap(rectangles, imageSize);
    }
}