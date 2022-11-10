using System.Drawing;
using TagsCloudVisualization.Drawing;
using TagsCloudVisualization.Layouter;
using static System.Drawing.Pens;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main(string[] args)
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var drawer = new CloudDrawer(layouter, Path.Combine(Directory.GetCurrentDirectory(), "Clouds"));
        var filler = new CloudFiller.CloudFiller(layouter);
        filler.FillCloud(50, 50, 200);
        drawer.DrawCloud("square_cloud.bmp", Red);
        layouter.ClearRectanglesLayout();

        filler.FillCloud(25, 80, 200);
        drawer.DrawCloud("rectangle_cloud.bmp", Blue);
        layouter.ClearRectanglesLayout();

        filler.FillRandomCloud(200, 50);
        drawer.DrawCloud("random_cloud.bmp", Green);
        layouter.ClearRectanglesLayout();
    }
}