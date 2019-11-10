using System;
using System.IO;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main(string[] _)
        {
            var rnd = new Random();
            var center = rnd.NextPoint(-100, 100, -100, 100);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutRectangles(rnd.NextSizes(10, 30, 2, 5, 10, 50));

            var filename = $"{Path.GetTempPath()}CCL {DateTime.Now:yyyy-MM-dd HH-mm-ss}.png";
            Utils.SaveRectanglesToPngFile(layouter.GetRectangles(), filename);
        }
    }
}
