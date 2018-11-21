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
            for (var i = 0; i < 1000; i++)
                circularCloudLayouter.PutNextRectangle(new Size(random.Next(50, 70), random.Next(50, 70)));
            var painter = new CloudPainter("cloud");
            var image = painter.CreateNewTagCloud(circularCloudLayouter);
            painter.SaveCloudImage(image);
        }
    }
}