using System;
using GeometryObjects;
using UserInterface;


namespace MainProgramm
{
    public class Programm
    {
        static void Main(string[] args)
        {
            var tagCloud = new CircularCloudLayouter(new Point(1000, 1000));
            var rectanglesCount = 300;
            var randomRange = 200;
            var random = new Random(2);
            var sizes = new Size[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
            {
                sizes[i] = new Size(1, 1);
                while (sizes[i].Width <= 2 * sizes[i].Height)
                    sizes[i] = new Size(1 + random.Next(randomRange), 1 + random.Next(randomRange));
            }
            var rectangles = new Rectangle[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
                rectangles[i] = tagCloud.PutNextRectangle(sizes[i]);
            var drawer = new DrawingClass(3000, 3000, "Bitmap3.bmp");
            drawer.DrawTagCloud(tagCloud);
        }
    }
}