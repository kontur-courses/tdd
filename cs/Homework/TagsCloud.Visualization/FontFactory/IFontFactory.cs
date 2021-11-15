using System.Drawing;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.FontFactory
{
    public interface IFontFactory
    {
        Font GetFont(Word word, int minCount, int maxCount);
    }
}