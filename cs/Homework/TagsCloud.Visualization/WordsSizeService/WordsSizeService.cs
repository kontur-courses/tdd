using System;
using System.Drawing;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.WordsSizeService
{
    public class WordsSizeService : IWordsSizeService
    {
        public Size CalculateSize(Word word, Font font)
        {
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);
            return Size.Ceiling(graphics.MeasureString(word.Content, font));
        }
    }
}