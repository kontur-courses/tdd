using System.Drawing;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.FontService
{
    public interface IFontService
    {
        Font CalculateFont(Word word);
    }
}