using System.Drawing;

namespace TagsCloudVisualization;

public class WordData
{
    public string Word { get; }
    public Font WordFont { get; }
    public Size Size { get; }

    public WordData(string word, Font font, Size size)
    {
        Word = word;
        WordFont = font;
        Size = size;
    }
}