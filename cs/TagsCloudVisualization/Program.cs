using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        private static Random rnd = new Random();
        private static Size GetRandomSize()
        {
            var h = rnd.Next(30, 100);
            var w = rnd.Next(50, 200);
            return new Size(w, h);
        }
        static void Main(string[] args)
        {
            var ccl = new CircularCloudLayouter(new Point(500, 500));
            for (int i = 0; i < 50; i++)
            {
                var size = GetRandomSize();
                ccl.PutNextRectangle(size);
            }
            DrawHandler.DrawRectangles(ccl, "test2.bmp");
        }
    }
}