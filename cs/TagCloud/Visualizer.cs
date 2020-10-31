using System;
using System.Drawing;

namespace TagCloud
{
    public class Visualizer
    {
        public static void Main()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(500, 500));
            foreach (var size in CreateRandomRectangles(30))
            {
                cloudLayouter.PutNextRectangle(size);
            }

            var rectsArray = cloudLayouter.Rectangles.ToArray();
            var bitmap = new Bitmap(1000, 1000);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangles(Brushes.Chocolate, rectsArray);
            g.DrawRectangles(new Pen(Brushes.Aqua), rectsArray);
            g.DrawEllipse(new Pen(Color.Aqua), 500, 500, 2, 2);
            bitmap.Save("test.bmp");
        }

        private static Size[] CreateRandomRectangles(int amount)
        {
            var sizes = new Size[amount];
            var rnd = new Random();
            for (var index = 0; index < amount; index++)
            {
                var width = rnd.Next(10) + 50;
                var height = rnd.Next(10) + 50;
                sizes[index] = new Size(width, height); 
            }

            return sizes;
        }
    }
}