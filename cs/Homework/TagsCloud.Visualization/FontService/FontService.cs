using System;
using System.Drawing;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.FontService
{
    public class FontService : IFontService
    {
        private readonly int maxCount;
        private readonly int maxFontSize;
        private readonly int minCount;

        public FontService(int maxFontSize, int minCount, int maxCount)
        {
            this.maxFontSize = maxFontSize;
            this.minCount = minCount;
            this.maxCount = maxCount;
        }

        public Font CalculateFont(Word word)
        {
            var fontSize = word.Count <= minCount
                ? 1
                : Convert.ToInt32(maxFontSize * (word.Count - minCount) / (maxCount - minCount));
            return new Font("Times new roman", fontSize, FontStyle.Regular);
        }
    }
}