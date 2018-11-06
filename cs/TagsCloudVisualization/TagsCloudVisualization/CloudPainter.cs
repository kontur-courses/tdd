using System.Drawing;

namespace TagsCloudVisualization
{
    public static class CloudPainter
    {
        public static void CreateNewTagCloud(CircularCloudLayouter cloudLayouter)
        {
            var cloudImage = new Bitmap(cloudLayouter.GetRectangle.Width + 100, cloudLayouter.GetRectangle.Height + 100);
            var painter = Graphics.FromImage(cloudImage);
            painter.Clear(Color.White);
            painter.DrawRectangles(new Pen(Color.RoyalBlue), cloudLayouter.tags.ToArray());
            cloudImage.Save("NewTagCloud.jpg");
        }
    }
}