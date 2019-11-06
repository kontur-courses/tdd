using System.Drawing;
using TagsCloudVisualization.CloudFactories;
using TagsCloudVisualization.CloudLayouters;

namespace TagsCloudVisualization
{
    public static class TagCloudBitmapCreator
    {
        public static Bitmap CreateBitmap(string[] cloudStrings, ICloudLayouter cloudLayouter,
                                          TagCloudFactory cloudFactory)
        {
            var bitmap = new Bitmap(cloudFactory.CanvasSize.Width, cloudFactory.CanvasSize.Height);
            var graphics = cloudFactory.GetGraphics(bitmap);

            graphics.FillRectangle(new SolidBrush(cloudFactory.BackgroundColor),
                                   new Rectangle(Point.Empty, cloudFactory.CanvasSize));

            var tagDrawer = cloudFactory.GetTagDrawer(graphics);

            foreach (var tag in cloudFactory.GetTags(cloudStrings, graphics, cloudLayouter))
                tagDrawer(tag);

            return bitmap;
        }
    }
}