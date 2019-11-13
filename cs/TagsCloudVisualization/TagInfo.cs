using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagInfo
    {
        public readonly string Value;
        public readonly Font Font;
        public Rectangle Rectangle { get; private set; }

        public TagInfo(string value, Font font, Rectangle rectangle)
        {
            Value = value;
            Font = font;
            Rectangle = rectangle;
        }
    }
}