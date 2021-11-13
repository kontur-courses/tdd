using System.Drawing;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.FontFactory
{
    public class FontFactory
    {
        private const int MaxFontSize = 2000;

        public Font GetFont(Word word, int minCount, int maxCount)
        {
            var fontSize = word.Count <= minCount
                ? 1
                : MaxFontSize * (word.Count - minCount) / (maxCount - minCount);
            return new Font("Times new roman", fontSize, FontStyle.Regular);
        }
    }
}