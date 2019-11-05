using System.Drawing;

namespace TagsCloudVisualization
{
    public struct Tag
    {
        public readonly string Text;
        public readonly TagStyle Style;
        public readonly Rectangle TagBox;

        public Tag(string text, TagStyle style, Rectangle tagBox)
        {
            Text = text;
            Style = style;
            TagBox = tagBox;
        }
    }
}