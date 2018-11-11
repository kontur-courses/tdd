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
            var pictureSize = new Size(1000,1000);
            Func<Size,Rectangle> putter = new CircularCloudLayouter(new Point(pictureSize)).PutNextRectangle;
            
            var pairs = FileReadWords($"{Environment.SpecialFolder.Desktop.ToString()}\\hamlet.txt")
                .CountUnique()
                .ToArray();
            var denominator = pairs.Max(x => x.Item2);
            
            pairs.Select(x=>Tuple.Create(x.Item1,120*x.Item2/denominator))
                .Where(x=>x.Item2>5)
                .OrderBy(x=>x.Item2)
                .DrawCloud(pictureSize,putter)
                .Save(".\\btm.png",ImageFormat.Png);
        }

        private static IEnumerable<string> FileReadWords(string path) =>
            File.ReadAllText(path)
                .ToLower()
                .Split()
                .Where(x => x != "")
                .Select(x => x.Trim(',','.','\n','\t'));
    }
}
