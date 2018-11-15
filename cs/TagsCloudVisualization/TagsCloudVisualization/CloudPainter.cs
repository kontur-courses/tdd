using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class CloudPainter
    {
        private static int bitmapWidth = 700;
        private static int bitmapHeight = 700;
        public static string Filename { get; private set; }
        
        public static Bitmap CreateNewTagCloud(CircularCloudLayouter cloudLayouter, string filename)
        {
            Filename = filename;
            bitmapWidth = bitmapWidth < cloudLayouter.GetWidth + 100
                ? cloudLayouter.GetWidth + 100
                : bitmapWidth;
            bitmapHeight = bitmapHeight < cloudLayouter.GetHeight + 100
                ? cloudLayouter.GetHeight + 100
                : bitmapHeight;
            var cloudImage = new Bitmap(bitmapWidth, bitmapHeight);
            var shearCoordinates = new Point(cloudImage.Width / 2, cloudImage.Height / 2);
            for (var i = 0; i < cloudLayouter.tags.Count; i++)
            {
                cloudLayouter.tags[i] = cloudLayouter.tags[i].MakeShift(shearCoordinates);
            }
            var painter = Graphics.FromImage(cloudImage);
            painter.Clear(Color.White);
            painter.DrawRectangles(new Pen(Color.RoyalBlue), cloudLayouter.tags.ToArray());
            return cloudImage;
        }
    }
}