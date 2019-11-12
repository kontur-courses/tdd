using System.Drawing;

namespace TagsCloudVisualization
{
    public class Tag
    {
        public readonly Size Size;
        public readonly string Text;
        public readonly Font Font;

        public Tag(Size size, string text, Font font)
        {
            Size = size;
            Text = text;
            Font = font;
        }
    }
}
