using System.Drawing;

namespace TagsCloudVisualization.Tags
{
    public class TagStyle
    {
        public static readonly StringFormat TextFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public readonly Color TextColor;
        public readonly Font Font;

        public TagStyle(Color textColor, Font font)
        {
            TextColor = textColor;
            Font = font;
        }
    }
}