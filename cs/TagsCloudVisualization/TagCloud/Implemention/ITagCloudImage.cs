using System.Drawing;
using TagCloud2;

namespace TagsCloudVisualization;

public interface ITagCloudImage : IDisposable
{
    Size Size();
    void Draw(Rectangle rec);
    void Draw(RectangleTagCloud rec);
    SizeF GetSizeWord(WordPopular word);
    void Save();
}