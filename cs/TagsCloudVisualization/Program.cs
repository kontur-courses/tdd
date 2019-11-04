using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            int count = rnd.Next(10, 50);
            List<Size> sizes = new List<Size>(count);
            for (int i = 0; i < count; i++)
            {
                int h = rnd.Next(10, 20);
                int w = h * (int)(2 + 3 * rnd.NextDouble());
                sizes.Add(new Size(w, h));
            }

            var center = new Point(rnd.Next(-100, 100), rnd.Next(-100, 100));
            CircularCloudLayouter ccl = new CircularCloudLayouter(center);
            ccl.PutRectangles(sizes);

            var filename = $"{Path.GetTempPath()}CCL_{(int)DateTime.Now.TimeOfDay.TotalSeconds}.bmp";
            ccl.SaveToFile(filename);
        }
    }
}
