using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var visualiser = new TagsCloudVisualiser(new Point(10, 10));
            visualiser.PutRectangle(new Size(4, 2));
            visualiser.PutRectangle(new Size(2, 2));
            visualiser.PutRectangle(new Size(2, 2));
            //visualiser.PutRectangle(new Size(4, 4));
            var image = visualiser.DrawCloud(new Point(), new Size(20, 20), new Size(800, 800));
            image.Save("testimage.png", ImageFormat.Png);
        }
    }
}