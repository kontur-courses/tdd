using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class ProgrammMain
    {
        static void Main(string[] args)
        {
            var rectanglesCount = 100;
            var randomRange = 200;
            var random = new Random(randomRange);
            var sizes = new Size[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
                sizes[i] = new Size(1 + random.Next(randomRange), 1 + random.Next(randomRange));
            var drawer = new TagCloudDrawing(3000, 3000, "Bitmap4.bmp",
                new Point(-3, -4));
            drawer.DrawTagCloud(sizes);
        }
    }
}