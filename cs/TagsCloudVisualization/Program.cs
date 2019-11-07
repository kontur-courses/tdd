using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main(string[] _)
        {
            var rnd = new Random();
            var center = new Point(rnd.Next(-100, 100), rnd.Next(-100, 100));
            var count = rnd.Next(10, 50);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutRectangles(Enumerable.Range(0, count).Select(__ => rnd.GenerateRandomSize()));

            var filename = $"{Path.GetTempPath()}CCL_{(int)DateTime.Now.TimeOfDay.TotalSeconds}.png";
            Utils.SaveRectanglesToPngFile(layouter.GetRectangles(), filename);
        }
    }
}
