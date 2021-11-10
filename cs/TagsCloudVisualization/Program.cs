using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var visualiser = new TagsCloudVisualiser(new Point());
            var random = new Random();
            for(var i = 0; i < 500; i++)
            {
                visualiser.PutRectangle(new Size(4,1));
            }
            var image = visualiser.DrawCloud(new Point(50, 50), new Size(100, 100), new Size(800, 800));
            image.Save("testimage.png", ImageFormat.Png);
            Process.Start(new ProcessStartInfo(Directory.GetCurrentDirectory() + "\\testimage.png") { UseShellExecute = true });
        }
    }
}