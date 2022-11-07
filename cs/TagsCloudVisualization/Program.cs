using System.Drawing;
using TagsCloudVisualization.CloudFiller;
using TagsCloudVisualization.Drawing;
using TagsCloudVisualization.Layouter;
using static System.Drawing.Pens;

public static class Program
{
    public static void Main(string[] args)
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var drawer = new CloudDrawer(layouter, Path.Combine(Directory.GetCurrentDirectory(), "Clouds"));
        var filler = new CloudFiller(layouter);
        filler.FillCloud(50, 50, 200);
        drawer.DrawCloud("square_cloud.bmp", Red);
        layouter.ClearLayout();

        filler.FillCloud(25, 80, 200);
        drawer.DrawCloud("rectangle_cloud.bmp", Blue);
        layouter.ClearLayout();

        filler.FillRandomCloud(200, 50);
        drawer.DrawCloud("random_cloud.bmp", Green);
        layouter.ClearLayout();
    }
}