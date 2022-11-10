using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Layouter;

namespace TagsCloudVisualization.Drawing;

public class CloudDrawer : ICloudDrawer
{
    private ICloudLayouter layouter;
    private string savePath;

    public CloudDrawer(ICloudLayouter layouter, string path)
    {
        this.layouter = layouter;
        savePath = path;
    }

    public void DrawCloud(string filename, Pen pen)
    {
        var newbmp = new Bitmap(1000, 1000);
        var path = Path.Combine(savePath, filename);
        using (var graphics = Graphics.FromImage(newbmp))
        {
            graphics.TranslateTransform(newbmp.Width / 2, newbmp.Height / 2);
            graphics.DrawRectangles(pen, layouter.GetRectanglesLayout().ToArray());
        }

        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        newbmp.Save(path, ImageFormat.Bmp);
        newbmp.Dispose();
    }
}