using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var visualiser = new TagsCloudVisualiser(new Point());
            var random = new Random();
            var sizes = new List<Size>();
            for(var i = 0; i < 10; i++)
            {
                var height = random.Next(1, 4);
                sizes.Add(new Size(random.Next(height, height * 5), height));
                //visualiser.PutRectangle(new Size(1,1));
            }
            foreach (var size in sizes)
            {
                visualiser.PutRectangle(size);
            }
            /*visualiser.PutRectangle(new Size(10, 10));
            visualiser.PutRectangle(new Size(20, 10));
            visualiser.PutRectangle(new Size(10, 20));
            visualiser.PutRectangle(new Size(10, 10));
            visualiser.PutRectangle(new Size(10, 10));*/
            var image = visualiser.DrawCloud(new Size(800, 800));
            image.Save("testimage.png", ImageFormat.Png);
            Process.Start(new ProcessStartInfo(Directory.GetCurrentDirectory() + "\\testimage.png") { UseShellExecute = true });
        }
    }
}