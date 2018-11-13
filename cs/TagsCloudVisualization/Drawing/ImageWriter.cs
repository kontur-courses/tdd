using System.Drawing;
using TagsCloudVisualization.Geom;

namespace TagsCloudVisualization.Drawing
{
    public class ImageWriter : ILayouterWriter
    {
        public readonly string ImageName;
        public readonly int ImageWidth;
        public readonly int ImageHeight;

        public ImageWriter(string imageName, int imageWidth=1024, int imageHeight=1024)
        {
            ImageName = imageName;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
        }

        public void WriteLayout(CircularCloudLayouter layouter)
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var graphics = Graphics.FromImage(bitmap);

            foreach (var r in layouter.Rectangles)
            {
                graphics.FillRectangle(Brushes.LightCoral, r);
                graphics.DrawRectangle(Pens.Black, r);
            }

            bitmap.Save(ImageName);
        }
    }
}
