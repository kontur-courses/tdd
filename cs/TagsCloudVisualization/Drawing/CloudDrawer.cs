using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.CloudDrawer;
using TagsCloudVisualization.Layouter;

namespace TagsCloudVisualization.Drawing;

public class CloudDrawer : ICloudDrawer
{
    private ICloudLayouter layouter;
    private string SavePath;

    public CloudDrawer(ICloudLayouter layouter, string path)
    {
        this.layouter = layouter;
        SavePath = path;
    }

    public void DrawCloud(string filename, Pen pen)
    {
        var newbmp = new Bitmap(1000, 1000);
        var path = Path.Combine(SavePath, filename);
        using (var graphics = Graphics.FromImage(newbmp))
        {
            graphics.TranslateTransform(newbmp.Width / 2, newbmp.Height / 2);
            graphics.DrawRectangles(pen, layouter.GetTagsLayout().ToArray());
        }

        newbmp.Save(path, ImageFormat.Bmp);
        newbmp.Dispose();
    }
}