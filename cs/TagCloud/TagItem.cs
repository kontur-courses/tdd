using System.Drawing;

namespace TagCloud
{
    public class TagItem
    {
        public string Word { get; }
        public Rectangle Rectangle { get; }

        public TagItem(string word, Rectangle rectangle)
        {
            Word = word;
            Rectangle = rectangle;
        }
    }
}