using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Programm
    {
        static void Main(string[] args)
        {
            var rectanglesCount = 100;
            var randomRange = 200;
            var random = new Random(randomRange);
            var sizes = new Size[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
                sizes[i] = new Size(random.Next(1, randomRange), random.Next(1, randomRange));
            var drawer = new TagCloudDrawing(1000, 1000, "Bitmap4.bmp",
                new Point(-3, -4));
            drawer.DrawTagCloud(sizes);
        }
    }
}