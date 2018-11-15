using System.Drawing;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var random = new Random();
            for (var i = 0; i < 500; i++)
                circularCloudLayouter.PutNextRectangle(new Size(random.Next(50, 70), random.Next(50, 70)));
            var image = CloudPainter.CreateNewTagCloud(circularCloudLayouter, "cloud");
            var path = $"{Directory.GetCurrentDirectory()}\\..\\..\\TagClouds\\{CloudPainter.Filename}.png";
            image.Save(path, ImageFormat.Png);
        }
    }
}