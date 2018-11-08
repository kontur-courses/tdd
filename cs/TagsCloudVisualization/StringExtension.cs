using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public static class StringExtension
    {
        public static Size GetSurroundingRectangleSize(this string text, Font font) => TextRenderer.MeasureText(text, font);

        public static Dictionary<string, int> GetFrequency(this string text) =>
            string.Concat(text
                    .Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                .Split(null)
                .Where(s => s.Any())
                .GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .ToDictionary(g => g.Key, g => g.Count());
    }
}
