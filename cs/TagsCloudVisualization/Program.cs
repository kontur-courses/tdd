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
            for(var i = 0; i < 100; i++)
            {
                var height = random.Next(1, 5);
                sizes.Add(new Size(random.Next(1, 5), height));
               //sizes.Add(new Size(1,1));
            }
            var n = 0;
            foreach (var size in sizes)
            {
                visualiser.PutRectangle(size);
                Console.WriteLine(n++);
            }
            var image = visualiser.DrawCloud(new Size(1000, 1000));
            image.Save("testimage.png", ImageFormat.Png);
            Process.Start(new ProcessStartInfo(Directory.GetCurrentDirectory() + "\\testimage.png") { UseShellExecute = true });
        }
    }
}