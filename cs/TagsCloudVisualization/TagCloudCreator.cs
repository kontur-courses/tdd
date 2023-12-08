using System.Drawing.Imaging;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagCloudCreator
    {
        public void CreateCloud(RectangleF[] rectangles, string imageName)
        {
            var bitmap = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Crimson);

            graphics.Clear(Color.Black);
            graphics.DrawRectangles(pen, rectangles);

            bitmap.Save(imageName + ".jpg", ImageFormat.Jpeg);
        }
    }
}