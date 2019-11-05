using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public static class CloudPainter
    {
        public static Bitmap GetImageOfCloud(CircularCloudLayouter cloud, int widthOfBorder=0)
        {
            var cloudBorders = cloud.GetImageSize();
            var image = new Bitmap(cloudBorders.Width + widthOfBorder * 2, cloudBorders.Height + widthOfBorder * 2);
            var canvas = Graphics.FromImage(image);
            canvas.Clear(Color.White);
            var rand = new Random();
            var rectangles = cloud.GetRectangles();
            foreach (var rectangle in rectangles)
            {
                var rect = rectangle;
                var r = rand.Next(256);
                var g = rand.Next(256);
                var b = rand.Next(256);
                rect.Move(-cloudBorders.X + widthOfBorder, -cloudBorders.Y + widthOfBorder);
                canvas.FillRectangle(new SolidBrush(Color.FromArgb(r, g,b)), rect);
            }
            return image;
        }
    }
}
