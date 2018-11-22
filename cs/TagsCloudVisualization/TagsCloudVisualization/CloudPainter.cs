using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    public class CloudPainter
    {
        private const int DefaultBitmapWidth = 700;
        private const int DefaultBitmapHeight = 700;
        public string Path { get; }

        public CloudPainter(string filename)
        {
            Path = $"{Directory.GetCurrentDirectory()}\\..\\..\\TagClouds\\{filename}.png";
        }

        public CloudPainter() { }
        
        public Bitmap CreateNewTagCloud(CircularCloudLayouter tagCloud)
        {
            var bitmapSize = CalculateBitmapSize(tagCloud);
            var cloudImage = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var shearCoordinates = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);
            for (var i = 0; i < tagCloud.tags.Count; i++)
            {
                tagCloud.tags[i] = tagCloud.tags[i].MoveOn(shearCoordinates);
            }
            var painter = Graphics.FromImage(cloudImage);
            painter.Clear(Color.White);
            painter.DrawRectangles(new Pen(Color.RoyalBlue), tagCloud.tags.ToArray());
            return cloudImage;
        }

        private static Size CalculateBitmapSize(CircularCloudLayouter tagCloud)
        {
            var border = 100;
                
            var bitmapWidth = DefaultBitmapWidth < tagCloud.GetWidth + border
                ? tagCloud.GetWidth + border
                : DefaultBitmapWidth;
            var bitmapHeight = DefaultBitmapHeight < tagCloud.GetHeight + border
                ? tagCloud.GetHeight + border
                : DefaultBitmapHeight;
            return new Size(bitmapWidth, bitmapHeight);
        }

        public void SaveCloudImage(Bitmap image, string path)
        {
            image.Save(path, ImageFormat.Png);   
        }
        
        public void SaveCloudImage(Bitmap image)
        {
            image.Save(Path, ImageFormat.Png);   
        }
    }
}