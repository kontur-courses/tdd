using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace TagsCloudVisualization
{
    class CloudVisualizer
    {
        public static void VisualizeCloud(Cloud cloud, string filename, Size imageSize)
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var g = Graphics.FromImage(image);
            var pen = new Pen(Color.Black);
            var rectangles = cloud.Rectangles;
            g.FillRectangle(new SolidBrush(Color.Red), rectangles[0]);
            foreach (var rectangle in rectangles)
            {
                g.DrawRectangle(pen, rectangle);
            }
            image.Save(filename, ImageFormat.Png);
        }
    }
}
