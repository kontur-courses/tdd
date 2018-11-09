using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var rnd = new Random();
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(5, 100), rnd.Next(5, 100)));

            var bitmap = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, 1000, 1000);
            foreach (var rect in layouter.Rectangles)
                graphics.DrawRectangle(Pens.Black, rect);

            bitmap.Save("LastRunSample\\sample.png", ImageFormat.Png);
        }
    }
}
