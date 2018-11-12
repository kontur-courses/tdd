using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            const int maxFontSize = 120;
            var pictureSize = new Size(1000,1000);
            Func<Size,Rectangle> putter = new CircularCloudLayouter(new Point(pictureSize.Divide(2))).PutNextRectangle;
            
            var pairs = ReadWordsFromFile($"C:\\Users\\Avel\\Desktop\\hamlet.txt")
                .CountUnique()
                .ToArray();
            
            var denominator = pairs.Max(x => x.Item2);
            pairs.Select(x=>(x.Item1, maxFontSize*x.Item2/denominator))
                .Where(x=>x.Item2>5)
                .DrawCloud(pictureSize, putter)
                .Save(".\\btm.png",ImageFormat.Png);
        }

        private static IEnumerable<string> ReadWordsFromFile(string path) =>
            File.ReadAllText(path)
                .ToLower()
                .AsEnumerable()
                .SplitBy(Char.IsPunctuation)
                .Select(x => new string(x.ToArray()));
    }
}
