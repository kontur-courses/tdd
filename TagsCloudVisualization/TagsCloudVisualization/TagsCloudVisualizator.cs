using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class TagsCloudVisualizator
{
    private readonly ITagsCloudLayouter layouter;
    private readonly Bitmap image;
    private readonly Graphics graphics;
    
    public TagsCloudVisualizator(ITagsCloudLayouter layouter, Size imageSize)
    {
        this.layouter = layouter;
        image = new Bitmap(imageSize.Width, imageSize.Height);
        graphics = Graphics.FromImage(image);
    }

    public void DrawRectangles()
    {
        graphics.DrawRectangles(Pens.Aqua, layouter.Rectangles.ToArray());
        graphics.Save();
    }

    public void DrawShape()
    {
        var center = new Point(image.Width / 2, image.Height / 2);
        var radius = (int)layouter.Rectangles.Select(x => Math.Sqrt(Math.Pow(x.X - center.X, 2) + Math.Pow(x.Y - center.Y, 2))).Max();
        graphics.DrawEllipse(Pens.Brown, new Rectangle(new Point(center.X - radius, center.Y - radius), new Size(radius * 2, radius * 2)));
        graphics.Save();
    }

    public void SaveImage(string filename)
    {
        image.Save(filename);
    }
}