using System.Drawing;

namespace TagCloud
{
    public class TagItem
    {
        public string Word { get; }
        public Rectangle Bounds { get; }

        public TagItem(string word, Rectangle bounds)
        {
            Word = word;
            Bounds = bounds;
        }
    }
}