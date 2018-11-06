using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var bitmap = new Bitmap(800, 600);
            var g = Graphics.FromImage(bitmap);
            var pen = new Pen(Brushes.Black, 1);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
            g.DrawRectangles(pen, GetRandomRectanglesLayout(new Point(bitmap.Width / 2, bitmap.Height / 2)));
            bitmap.Save("1.png");
        }

        private static Rectangle[] GetRandomRectanglesLayout(Point centerPoint)
        {
            var res = new List<Rectangle>();
            var layouter = new CircularCloudLayouter(centerPoint);
            var random = new Random();

            for (int i = 0; i < 25; i++)
            {
                Size nextRectSize;

                do
                {
                    nextRectSize = new Size(random.Next(128) + 32, random.Next(64) + 32);
                } while (nextRectSize.Width < nextRectSize.Height * 2);

                res.Add(layouter.PutNextRectangle(nextRectSize));
            }

            return res.ToArray();
        }
    }
}