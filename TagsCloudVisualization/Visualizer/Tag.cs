using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public class Tag
    {
        public string Phrase { get; }
        public Font Font { get; }
        public Rectangle Rectangle { get; }

        public Tag(string phrase, Rectangle rectangle, Font font)
        {
            Phrase = phrase;
            Font = font;
            Rectangle = rectangle;
        }
    }
}
