using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public static class LayoutSaver
    {
        public static void SaveFailedLayoutImageAsJpeg(string path,Size imageSize, IEnumerable<Rectangle> rectangles)
        {
            var btm = new Bitmap(imageSize.Width,imageSize.Height);
            var g = Graphics.FromImage(btm);

            foreach (Rectangle r in rectangles)
                g.DrawRectangle(new Pen(Color.Blue), r);

            btm.Save(path, ImageFormat.Jpeg);
        }
    }
}
