using System.Drawing;

namespace TagsCloudVisualization;

class Program
{
    static void Main(string[] args)
    {
        var distribution = new SpiralPointDistributor(new PointF(500, 500));
        var layouter = new CircularCloudLayouter(distribution);
        var rectangles = new List<RectangleF>();
        for (var i = 0; i < 50; i++)
            rectangles.Add(layouter.PutNextRectangle(new SizeF(50, 50)));
        var visualizer = new Visualizer(rectangles);
        visualizer.SaveVisualizationAsPng("result.png");
    }
}