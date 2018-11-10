using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud.Utility
{
    public class WordReader //TODO it will be changed in future
    {
        public static List<Size> ReadWords(string pathToWords)
        {
            var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), pathToWords));
            var sizes = sr
                .ReadToEnd()
                .Split(';')
                .Where(s => !string.IsNullOrEmpty(s));
            return sizes
                .Select(size => size.Split('x')
                    .Select(int.Parse)
                    .ToArray())
                .Select(bounds => new Size(bounds[0], bounds[1]))
                .ToList();
        }
    }
}