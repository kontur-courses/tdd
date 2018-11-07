using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(100, 100));
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(GetRandomSize());

            var bitmap = new Bitmap(2048, 2048);
            var gr = Graphics.FromImage(bitmap);

            foreach (var r in layouter.Rectangles)
            {
                gr.FillRectangle(Brushes.LightCoral, r);
                gr.DrawRectangle(Pens.Black, r);
            }

            bitmap.Save("test.png");
        }

        public static Size GetRandomSize()
        {
            var r = new Random();
            return new Size(r.Next(10, 50), r.Next(10, 30));
        }
    }
}